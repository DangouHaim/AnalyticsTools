using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awesomium.Core;
using Awesomium.Windows;
using Awesomium.Windows.Controls;
using System.Net;
using System.Windows;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AnalyticsTools
{
    class YandexDirect : IYandexDirect
    {
        public event EventHandler<AuthEventArgs> OnAuthorize = (s, e) => { };
        private static string AppID = "a04d4df25f224449918ac7ab5c0ab2cd";
        private static string AppPass = "314aa4f3bb5f4a6cad7608c5ef65991b";
        private static string AuthURI = "https://oauth.yandex.ru/authorize?response_type=code&client_id=";
        private static string AuthTokenURI = "https://oauth.yandex.ru/token?";
        private static string AuthCode = "";


        public static class API4
        {
            public static string SB = "https://api-sandbox.direct.yandex.ru/v4/json/";
            public static string SBLive = "https://api-sandbox.direct.yandex.ru/live/v4/json/";
            public static string V4 = "https://api.direct.yandex.ru/v4/json/";
            public static string V4Live = "https://api.direct.yandex.ru/live/v4/json/";
        }

        public static class API5
        {
            public static string SBCompaigns = "https://api-sandbox.direct.yandex.com/json/v5/campaigns";
            public static string V5Compaigns = "https://api.direct.yandex.com/json/v5/campaigns";
        }

        public void Report()
        {
            throw new NotImplementedException();
        }

        public void Authorize(WebControl c)
        {
            c.Source = new Uri(AuthURI + AppID);
            c.AddressChanged += (s, e) =>
            {
                var data = e.Url;
                var tp = data.AbsoluteUri.ToLower();
                if (tp.IndexOf("code=") > -1)
                {
                    c.Visibility = Visibility.Hidden;
                    AuthCode = tp.Split(new string[] { "code=" }, StringSplitOptions.None)[1];
                    /*
                        grant_type = authorization_code
                        code = <код_подтверждения>
                        client_id = <идентификатор_приложения>
                        client_secret = <пароль_приложения>
                    */
                    try
                    {
                        using (WebClient cl = new WebClient())
                        {
                            var param = new NameValueCollection();
                            param["grant_type"] = "authorization_code";
                            param["code"] = AuthCode;
                            param["client_id"] = AppID;
                            param["client_secret"] = AppPass;

                            var response = cl.UploadValues(AuthTokenURI, "POST", param);
                            string responseInString = Encoding.UTF8.GetString(response);

                            JObject j = JObject.Parse(responseInString);
                            string token = (string)j["access_token"];
                            OnAuthorize(this, new AuthEventArgs() { Token = token });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("YandexDirect => Authorize:\n" + ex.Message + "\n\n" + ex.StackTrace);
                    }
                }
            };
        }

        public List<Balance> GetBalance(string token, object[] param)
        {
            List<Balance> sums = new List<Balance>();

            using (WebClient cl = new WebClient())
            {
                string data = JsonConvert.SerializeObject(new Method()
                {
                    method = "GetBalance",
                    param = param,
                    token = token
                });

                var response = cl.UploadString(API4.SB, "POST", data);

                JObject j = JObject.Parse(response);
                var sd = from p in j["data"] select new Balance {
                    CampaignId = (string)p["CampaignID"],
                    Sum = (string)p["Sum"],
                    Rest = (string)p["Rest"]
                };
                sums = sd.ToList();
            }
            return sums;
        }

        public List<string> GetClientsList(string token)
        {
            List<string> users = new List<string>();
            var addData = "";
            try
            {
                using (WebClient cl = new WebClient())
                {
                    string data = JsonConvert.SerializeObject(new Method()
                    {
                        method = "GetClientsList",
                        token = token
                    });

                    var response = cl.UploadString(API4.SB, "POST", data);
                    addData = response;
                    JObject j = JObject.Parse(response);
                    var sd = from p in j["data"] select (string)p["Login"];
                    foreach (string s in sd)
                    {
                        users.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("YandexDirect => GetClientsList:\n" + ex.Message + "\n\nresponse: \n" + addData + "\n\n" + ex.StackTrace);
            }
            return users;
        }

        public List<Compaign> GetCampaignsIds(string token, string clientLogin, Params param)
        {
            List<Compaign> cps = new List<Compaign>();
            using (WebClient cl = new WebClient())
            {
                cl.Headers.Add("Authorization", "Bearer " + token);
                cl.Headers.Add("Client-Login", clientLogin);

                string data = JsonConvert.SerializeObject(new MethodV5()
                {
                    method = "get",
                    _params = param
                });

                data = data.Replace("_params", "params");

                var response = cl.UploadString(API5.SBCompaigns, "POST", data);
                JObject j = JObject.Parse(response);

                var sds = from r in j["result"]["Campaigns"]
                         select new Compaign {
                             Id = (string)r["Id"],
                             Name = (string)r["Name"]
                         };

                cps = sds.ToList();
            }
            return cps;
        }

    }
}
