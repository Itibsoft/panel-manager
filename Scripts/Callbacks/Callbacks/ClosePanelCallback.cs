namespace Itibsoft.PanelManager
{
    public class ClosePanelCallback : IPanelCallback
    {
        public IPanel Panel { get; }

        public ClosePanelCallback(IPanel panel)
        {
            Panel = panel;
        }
    }
}