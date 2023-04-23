using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using SmartOvenV2;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using UIKit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Syncfusion.XForms.iOS.PopupLayout;
using Plugin.LocalNotification;

namespace SmartOvenV2.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();
            Syncfusion.SfGauge.XForms.iOS.SfGaugeRenderer.Init();
            new SfBusyIndicatorRenderer();
            AppCenter.Start("b0addd48-99da-4f83-a149-708bc2c36f2c",
                typeof(Analytics), typeof(Crashes));
            SfPopupLayoutRenderer.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            LocalNotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
            base.WillEnterForeground(uiApplication);
        }
    }
}
