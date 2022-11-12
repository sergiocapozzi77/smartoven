using SmartOvenV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartOvenV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OvenView : ContentPage
    {
        OvenViewModel vm;

        public OvenView()
        {
            this.BindingContextChanged += (t, e) =>
            {
                vm = (OvenViewModel)this.BindingContext;
            };


            InitializeComponent();
        }


        private async void MarkerPointer_BottomValueChangeCompleted(object sender, Syncfusion.SfGauge.XForms.PointerValueChangedEventArgs e)
        {
            vm.SetBottomDesiredTemperature(e.Value);
        }

        private async void MarkerPointer_TopValueChangeCompleted(object sender, Syncfusion.SfGauge.XForms.PointerValueChangedEventArgs e)
        {
            vm.SetTopDesiredTemperature(e.Value);
        }

        private void MarkerPointer_TopValueChanging(object sender, Syncfusion.SfGauge.XForms.PointerValueChangingEventArgs e)
        {
            vm.TopDesiredTemperatureChanging(e.NewValue);
        }

        private void MarkerPointer_BottomValueChanging(object sender, Syncfusion.SfGauge.XForms.PointerValueChangingEventArgs e)
        {
            vm.BottomDesiredTemperatureChanging(e.NewValue);
        }
    }
}