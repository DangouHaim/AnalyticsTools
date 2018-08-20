using System;
using System.Collections.Generic;
using Awesomium.Windows.Controls;

namespace AnalyticsTools
{
    interface IYandexDirect
    {
        /*
        ID: a04d4df25f224449918ac7ab5c0ab2cd
        Пароль: 314aa4f3bb5f4a6cad7608c5ef65991b
        Callback URL: https://oauth.yandex.ru/verification_code
        */
        event EventHandler<AuthEventArgs> OnAuthorize;

        void Report();

        void Authorize(WebControl c);

        List<Balance> GetBalance(string token, object[] param);

        List<string> GetClientsList(string token);

        List<Compaign> GetCampaignsIds(string token, string clientLogin, Params param);

    }
}
