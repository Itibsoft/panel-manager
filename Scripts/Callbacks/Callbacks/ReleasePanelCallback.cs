namespace Itibsoft.PanelManager
{
    public class ReleasePanelCallback : IPanelCallback
    {
        public readonly IPanelController PanelController;
        
        public ReleasePanelCallback(IPanelController panelController)
        {
            PanelController = panelController;
        }
    }
}