using System;
using System.ComponentModel;
using System.Windows.Input;
using Codeplex.Reactive;
using Codeplex.Reactive.Extensions;
using System.Reactive.Linq;

namespace FormsWithRxProperty.ViewModels
{
    public class SecondViewModel : INotifyPropertyChanged
    {
        public ReactiveProperty<string> ValidationAttr { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string _myName = "HoGe";
        public string MyName 
        {
            get { return _myName; }
            set 
            { 
                if (_myName == value)
                    return;

                _myName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MyName"));
            }
        }

        public ReactiveProperty<string> LowerText { get; private set; }

        private ICommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand = 
                    new Xamarin.Forms.Command(() => MyName = "XAAAAMAAARIN!!"));
            }
        }

        public SecondViewModel()
        {
            this.LowerText = this.ObserveProperty(x => x.MyName)
                .Select(x => x.ToLower())
                .ToReactiveProperty();
        }

    }
}

