﻿using System;
using Xamarin.Forms;
using FormsWithRxProperty.ViewModels;

namespace FormsWithRxProperty.Pages
{
    public class FirstPage : ContentPage
    {
        public FirstPage()
        {
            var entry = new Entry
            {
                Text = "Hello, Forms!",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var button = new Button
            {
                Text = "Clear (type 'clear' to enable)",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            this.BindingContext = new FirstViewModel();
            entry.SetBinding<FirstViewModel>(Entry.TextProperty, vm=>vm.HogeName.Value);
            label.SetBinding<FirstViewModel>(Label.TextProperty, vm=>vm.HogeName.Value);
            button.SetBinding<FirstViewModel>(Button.CommandProperty, vm=>vm.Clear);

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
                }
            };
        }
    }
}
