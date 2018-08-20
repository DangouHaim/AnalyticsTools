using Awesomium.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnalyticsTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static string _yToken = "";
        private UniversalAnalytics _analytics;
        private ObservableCollection<string> _users;
        private ObservableCollection<Compaign> _companies;
        private bool _selected;

        WebSession webSession = WebCore.CreateWebSession("session", WebPreferences.Default);

        public MainWindow()
        {
            InitializeComponent();
            PreInit();
        }

        private void PreInit()
        {
            Browser.WebSession = webSession;
        }

        private void Init()
        {
            _analytics = new UniversalAnalytics(new GoogleAnalytics(), new YandexDirect());
            _users = new ObservableCollection<string>();
            _companies = new ObservableCollection<Compaign>();

            UserList.ItemsSource = _users;
            CompanyList.ItemsSource = _companies;

            Authorize();
        }

        private void Authorize()
        {
            _analytics.Yandex().OnAuthorize += OnAuthorize;
            _analytics.Yandex().Authorize(Browser);
        }

        private void OnAuthorize(object sender, AuthEventArgs e)
        {
            _yToken = e.Token;
            
            foreach(var v in _analytics.Yandex().GetClientsList(_yToken))
            {
                _users.Add(v);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void GetCompaigns()
        {
            var i = UserList.SelectedIndex;
            if (i > -1)
            {
                Balance.Content = "";
                _companies.Clear();
                var p = new Params()
                {
                    FieldNames = new[] { "Id", "Name" }
                };
                foreach (var v in _analytics.Yandex().GetCampaignsIds(_yToken, _users[i], p ))
                {
                    _companies.Add(v);
                }
            }
        }

        private void GetBalance()
        {
            var i = CompanyList.SelectedIndex;
            if(i > -1)
            {
                string res = "";
                foreach(var v in _analytics.Yandex().GetBalance(_yToken, new object[] { _companies[i].Id }))
                {
                    res += v + "; ";
                }
                res = res.Trim().Remove(res.Length - 2, 1);
                Balance.Content = res;
            }
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selected = true;
            GetCompaigns();
        }

        private void UserList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_selected)
            {
                GetCompaigns();
            }
            else
            {
                _selected = false;
            }
        }

        private void CompanyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selected = true;
            GetBalance();
        }

        private void CompanyList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(!_selected)
            {
                GetBalance();
            }
            else
            {
                _selected = false;
            }
        }
    }
}
