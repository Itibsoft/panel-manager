using Itibsoft.PanelManager.Tests.Reflections;

namespace Itibsoft.PanelManager
{
    public interface IPanelFactory
    {
        public IViewMono Create(IPanelMeta meta);
    }
}