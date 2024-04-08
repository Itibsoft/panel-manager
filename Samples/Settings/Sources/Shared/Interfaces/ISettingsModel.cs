using System;
using Itibsoft.MVP;

namespace Settings.Shared
{
    public interface ISettingsModel : IModel<SettingsData>
    {
        public event Action OnChangedClicked;
        public void PlusClick();
    }
}