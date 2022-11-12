
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SmartOvenV2.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngredientsView : ContentPage
    {
        public IngredientsView()
        {
            this.InitializeComponent();
        }
    }
}