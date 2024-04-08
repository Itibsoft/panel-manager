using System;
using Itibsoft.MVP;

namespace Settings.Shared
{
    public interface ISettingsView : IView
    {
        public event Action OnClickPlus;
        public void SetCountClicked(int count);
    }
}