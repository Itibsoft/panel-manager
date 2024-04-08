using System;
using Itibsoft.MVP;

namespace Itibsoft.PanelManager
{
    public interface IViewHandler : IDisposable
    {
        public void Open();
        public void Close();

        public IView GetView();
    }
}