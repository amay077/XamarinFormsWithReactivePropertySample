using System;
using Codeplex.Reactive;
using System.Reactive.Linq;

namespace FormsWithRxProperty.ViewModels
{
    public class FirstViewModel
    {

        private readonly ReactiveProperty<string> _hogeName = 
            new ReactiveProperty<string>("Hoge");
        public ReactiveProperty<string> HogeName
        {
            get { return _hogeName; }
        }

        public ReactiveCommand Clear
        {
            get;
            private set;
        }

        public FirstViewModel()
        {
            this.Clear = _hogeName
                .Select(x => x.Equals("clear"))
                .ToReactiveCommand();
            this.Clear.Subscribe(_ => _hogeName.Value = "");
        }
        
    }
}

