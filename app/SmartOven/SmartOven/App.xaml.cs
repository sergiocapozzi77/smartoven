using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartOvenV2.Views;
using Syncfusion.Licensing;
using PizzaTime.Bootstrap;
using SmartOvenV2.Managers;
using SmartOvenV2.Services;

namespace SmartOvenV2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense(Licenses.SyncFusion);
            AppContainer.RegisterDependencies();
            //DependencyService.Register<MockDataStore>();
            this.MainPage = new MainPage();

            var appStatus = AppContainer.Resolve<IAppStatusManager>();
            appStatus.Restore();
        }

        protected override void OnStart()
        {
            AppCenter.Start(Licenses.AppCenter,
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            var statusPoller = AppContainer.Resolve<IStatusPoller>();
            statusPoller.Pause();

            var appStatus = AppContainer.Resolve<IAppStatusManager>();
            appStatus.Persist();
        }

        protected override void OnResume()
        {
            var statusPoller = AppContainer.Resolve<IStatusPoller>();
            statusPoller.Resume();

            var appStatus = AppContainer.Resolve<IAppStatusManager>();
            appStatus.Restore();
        }
    }
}
