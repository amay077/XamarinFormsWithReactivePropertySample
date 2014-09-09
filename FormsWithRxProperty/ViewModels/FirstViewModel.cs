using System;
using Codeplex.Reactive;
using System.Reactive.Linq;

namespace FormsWithRxProperty.ViewModels
{
    public class FirstViewModel
    {
        private readonly ReactiveProperty<string> _inputText = 
            new ReactiveProperty<string>("Hoge");
        public ReactiveProperty<string> InputText 
        { 
            get { return _inputText; }
        }

        public ReactiveProperty<string> DisplayText
        {
            get; private set;
        }

        public ReactiveCommand Clear
        {
            get; private set;
        }

        public FirstViewModel()
        {
            // DisplayText は、InputText の変更から1秒後に大文字にして更新
            this.DisplayText = _inputText
                .Delay(TimeSpan.FromSeconds(1))
                .Select(x => x.ToUpper())
                .ToReactiveProperty();

            // InputText が `clear` の時に実装可能
            this.Clear = _inputText
                .Select(x => x.Equals("clear"))
                .ToReactiveCommand();
            // 実行されたら、InputText を空にする
            this.Clear.Subscribe(_ => _inputText.Value = String.Empty);
        }
        
    }
}

