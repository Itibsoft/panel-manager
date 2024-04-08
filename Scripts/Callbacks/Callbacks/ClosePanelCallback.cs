namespace Itibsoft.PanelManager
{
    public class ClosePanelCallback : IPanelCallback
    {
        public readonly IViewMono Panel;

        public ClosePanelCallback(IViewMono panel)
        {
            Panel = panel;
        }
    }
}