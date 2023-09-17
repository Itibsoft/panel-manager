namespace Assets.Project.Scripts.UI.EventsCallbacks
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