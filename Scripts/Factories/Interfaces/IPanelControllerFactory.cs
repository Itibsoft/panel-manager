namespace Itibsoft.PanelManager
{
    public interface IPanelControllerFactory
    {
        public TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController;
    }
}