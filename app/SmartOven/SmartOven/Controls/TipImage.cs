using Syncfusion.XForms.Border;
using Syncfusion.XForms.PopupLayout;
using System;
using Xamarin.Forms;

namespace SmartOvenV2.Controls
{
    public class TipImage : Label
    {
        public static readonly BindableProperty TipTextProperty =
            BindableProperty.Create(nameof(TipImage), typeof(string), typeof(TipImage), null);

        public string TipText
        {
            get => (string)this.GetValue(TipTextProperty);
            set => this.SetValue(TipTextProperty, value);
        }

        public static readonly BindableProperty TipColorProperty =
    BindableProperty.Create(nameof(TipImage), typeof(Color), typeof(TipImage), null);

        public Color TipColor
        {
            get => (Color)this.GetValue(TipColorProperty);
            set => this.SetValue(TipColorProperty, value);
        }

        public TipImage()
        {
            var tap = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);
            this.VerticalOptions = LayoutOptions.Center;
            this.HorizontalOptions = LayoutOptions.Center;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            var popup = new SfTipPopupView();
            popup.ShowPopUp(TipText, TipColor);
        }
    }

    public class SfTipPopupView : SfPopupLayout
    {
        public void ShowPopUp(string tipText, Color tipColor)
        {
            DataTemplate templateView;

            templateView = new DataTemplate(
                () =>
                {
                    var border = new SfBorder();
                    border.VerticalOptions = LayoutOptions.FillAndExpand;
                    border.HorizontalOptions = LayoutOptions.FillAndExpand;
                    border.BorderWidth = 15;
                    border.BorderColor = Color.Transparent;

                    var label = new Label();
                    {
                        VerticalOptions = LayoutOptions.Center;
                    };
                    
                    label.Text = tipText;
                    label.Style = (Style)Application.Current.Resources["TipTextStyle"];
                    // label.HorizontalTextAlignment = TextAlignment.Center;
                    border.Content = label;
                    return border;
                });

            this.PopupView.ShowFooter = false;
            this.PopupView.ShowCloseButton = false;
            this.ClosePopupOnBackButtonPressed = true;
            this.PopupView.ContentTemplate = templateView;
            this.PopupView.ShowHeader = false;
            this.PopupView.ShowFooter = false;
            this.PopupView.HeightRequest = 400;
            this.Padding = new Thickness(15);
            this.Show(false);
        }
    }
}
