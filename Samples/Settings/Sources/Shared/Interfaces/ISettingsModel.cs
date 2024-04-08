using System;
using Itibsoft.MVP;
using UniRx;

namespace Settings.Shared
{
    public interface ISettingsModel : IModel<SettingsData>
    {
        public ReactiveProperty<int> ClickedCountProperty { get; }
    }
}