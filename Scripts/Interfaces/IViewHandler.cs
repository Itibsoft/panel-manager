using System;
using a;

namespace Itibsoft.PanelManager
{
    public interface IViewHandler : IDisposable
    {
        public void Open();
        public void Close();

        public IView GetView();
    }
}