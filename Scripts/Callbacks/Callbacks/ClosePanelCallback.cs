namespace Itibsoft.PanelManager
{
    public class ClosePanelCallback : IPanelCallback
    {
        public readonly IPanel Panel;

        public ClosePanelCallback(IPanel panel)
        {
            Panel = panel;
        }
    }
}