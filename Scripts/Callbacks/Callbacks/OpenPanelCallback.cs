namespace Itibsoft.PanelManager
{
    public class OpenPanelCallback : IPanelCallback
    {
        public readonly IViewMono Panel;

        public OpenPanelCallback(IViewMono panel)
        {
            Panel = panel;
        }
    }
}