namespace Itibsoft.PanelManager.Sample.Demo
{
    [Panel(PanelType = PanelType.Window, Order = 0, AssetId = "DemoOverlay")]
    public class DemoOverlayController : PanelControllerBase<DemoOverlay>
    {
        public DemoOverlayController(DemoOverlay panel) : base(panel)
        {
            panel.OnCloseOverlay += Close;
        }
    }
}