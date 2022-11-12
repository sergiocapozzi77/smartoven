using PizzaTime.Bootstrap;
using SmartOvenV2.ViewModels;
using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace SmartOvenV2.Utilities
{
    internal class ViewModelLocator
    {
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool),
                typeof(ViewModelLocator), default(bool),
                propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Element view))
            {
                return;
            }

            var viewType = view.GetType();
            if (viewType.FullName != null)
            {
                var viewModel = LoadViewModel(viewType);
                if (viewModel != null)
                {
                    view.BindingContext = viewModel;
                }
            }
        }

        public static object LoadViewModel(Type viewType)
        {
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return null;
            }

            return AppContainer.Resolve(viewModelType);
        }
    }
}
