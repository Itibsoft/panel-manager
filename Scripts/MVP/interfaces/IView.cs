using Itibsoft.PanelManager;

namespace Itibsoft.MVP
{
    public interface IView
    {
        public PanelState State { get; }
        public IPanelMeta Meta { get; }
    }
}