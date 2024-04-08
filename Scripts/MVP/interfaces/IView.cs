using Itibsoft.PanelManager;

namespace a
{
    public interface IView
    {
        public PanelState State { get; }
        public IPanelMeta Meta { get; }
    }
}