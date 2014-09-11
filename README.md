Sample for Xamarin.Forms and ReactiveProperty
======================================

# SCREENSHOT

![](https://dl.dropboxusercontent.com/u/264530/qiita/using_xamarin_forms_with_reactiveproperty_03.gif)

# CODE

## ViewModel

```csharp
// Property and Command implementations by Rx
this.DisplayText = _inputText
    .Delay(TimeSpan.FromSeconds(1))
    .Select(x => x.ToUpper())
    .ToReactiveProperty();

this.Clear = _inputText
    .Select(x => x.Equals("clear"))
    .ToReactiveCommand();
this.Clear.Subscribe(_ => _inputText.Value = String.Empty);
```

## Page(Xamarin.Forms)

```csharp
// Bind to ViewModel
this.BindingContext = new FirstViewModel();
entry.SetBinding<FirstViewModel>(Entry.TextProperty, vm=>vm.InputText.Value);
label.SetBinding<FirstViewModel>(Label.TextProperty, vm=>vm.DisplayText.Value);
button.SetBinding<FirstViewModel>(Button.CommandProperty, vm=>vm.Clear);
```


# LINKS

## ReactiveProperty

* [ReactiveProperty オーバービュー - かずきのBlog@hatena](http://okazuki.hatenablog.com/entry/2014/05/07/014133)
* [ReactiveProperty - MVVM Extensions for Rx - Download: ReactiveProperty v1.0](https://reactiveproperty.codeplex.com/releases/view/132232)
* [neue cc - ReactiveProperty : Rx + MVVMへの試み](http://neue.cc/2011/08/26_341.html)
* [ReactivePropertyを使いたい人のための、ReactiveExtensions入門（その１） | 泥庭](http://yone64.wordpress.com/2014/06/20/reactiveproperty%E3%82%92%E4%BD%BF%E3%81%84%E3%81%9F%E3%81%84%E4%BA%BA%E3%81%AE%E3%81%9F%E3%82%81%E3%81%AE%E3%80%81reactiveextensions%E5%85%A5%E9%96%80%EF%BC%88%E3%81%9D%E3%81%AE%EF%BC%91%EF%BC%89/)

## Xamarin.Forms

* [Xamarin.Forms | Xamarin](http://developer.xamarin.com/guides/cross-platform/xamarin-forms/)
* [Xamarin.Formsの基本構想と仕組み - Build Insider](http://www.buildinsider.net/mobile/insidexamarin/14)
* [Xamarin.Forms - Build Insider](http://www.buildinsider.net/tagcloud?tag=Xamarin.Forms)
* [Xamarin.Forms ListViewでTwitter風のレイアウトを作成してみました（機種依存コードなし） - SIN@SAPPOROWORKSの覚書](http://furuya02.hatenablog.com/entry/2014/08/08/003036)
