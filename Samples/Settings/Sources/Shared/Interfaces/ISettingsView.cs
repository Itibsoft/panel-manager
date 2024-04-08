using System;
using Itibsoft.MVP;
using UniRx;

namespace Settings.Shared
{
    public interface ISettingsView : IView
    {
        public ReactiveProperty<string> ClickedValueProperty { get; }
        public ReactiveCommand IncrementValueCommand { get; }
    }
}