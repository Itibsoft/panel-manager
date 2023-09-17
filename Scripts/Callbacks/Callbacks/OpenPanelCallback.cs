namespace Assets.Project.Scripts.UI.EventsCallbacks
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