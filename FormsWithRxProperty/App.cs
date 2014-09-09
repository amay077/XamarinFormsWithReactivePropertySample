using System;
using Xamarin.Forms;
using FormsWithRxProperty.Pages;

namespace FormsWithRxProperty
{
    public class App
    {
        public static Page GetMainPage()
        {	
            return new FirstPage();
        }
    }
}

