using System;
using a;

namespace Samples.Tests
{
    public interface ISettingsView : IView
    {
        public event Action OnClickPlus;
        public void SetCountClicked(int count);
    }
}