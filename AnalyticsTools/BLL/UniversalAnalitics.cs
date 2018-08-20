
namespace AnalyticsTools
{
    class UniversalAnalytics
    {
        private readonly IGoogleAnalytics _googleAnalytics;
        private readonly IYandexDirect _yandexAnalytics;

        public UniversalAnalytics()
        {
            _googleAnalytics = new GoogleAnalytics();
            _yandexAnalytics = new YandexDirect();
        }

        public UniversalAnalytics(IGoogleAnalytics googleBehaviour, IYandexDirect yandexBehaviour)
        {
            _googleAnalytics = googleBehaviour;
            _yandexAnalytics = yandexBehaviour;
        }

        public void Report()
        {
            _googleAnalytics.Report();
            _yandexAnalytics.Report();
            ReportCore();
        }

        protected virtual void ReportCore()
        {
            //Override for your own logic of report
        }

        public IGoogleAnalytics Google()
        {
            return _googleAnalytics;
        }

        public IYandexDirect Yandex()
        {
            return _yandexAnalytics;
        }
    }
}
