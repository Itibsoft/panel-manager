namespace Itibsoft.PanelManager
{
    public class OpenPanelCallback : IPanelCallback
    {
        public readonly IPanel Panel;

        public OpenPanelCallback(IPanel panel)
        {
            Panel = panel;
        }
    }
}