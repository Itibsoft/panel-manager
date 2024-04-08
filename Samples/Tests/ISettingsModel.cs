using System;
using a;

namespace Samples.Tests
{
    public interface ISettingsModel : IModel<SettingsData>
    {
        public event Action OnChangedClicked;
        public void PlusClick();
    }
}