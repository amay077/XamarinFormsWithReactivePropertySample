using System;
using Xamarin.Forms;
using FormsWithRxProperty.ViewModels;

namespace FormsWithRxProperty.Pages
{
    public class SecondPage : ContentPage
    {
        public SecondPage()
        {
            this.Title = "Second Page";

            var entry = new Entry
            {
                Text = "Hello, Forms!",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            var button = new Button
            {
                Text = "Clear (type 'clear' to enable)",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            this.Content = new StackLayout
            {
                Padding = new Thickness(50f),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Vertical,
                Children = 
                {
                    entry,
                    label,
                    button
                },
            };

            // ViewModel との Binding
            this.BindingContext = new SecondViewModel();
            entry.SetBinding<SecondViewModel>(Entry.TextProperty, vm=>vm.MyName);
            label.SetBinding<SecondViewModel>(Label.TextProperty, vm=>vm.LowerText.Value);
            button.SetBinding<SecondViewModel>(Button.CommandProperty, vm=>vm.ResetCommand);

        }
    }
}

