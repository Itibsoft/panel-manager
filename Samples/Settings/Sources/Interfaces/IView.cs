using System;

namespace Settings
{
    public interface IView : IDisposable
    {
        public event Action OnClickPlus;
        public void SetCountClicked(int count);
    }
}