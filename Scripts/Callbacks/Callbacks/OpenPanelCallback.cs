namespace Itibsoft.PanelManager
{
    public class OpenPanelCallback : IPanelCallback
    {
        public IPanel Panel { get; }

        public OpenPanelCallback(IPanel panel)
        {
            Panel = panel;
        }
    }
}