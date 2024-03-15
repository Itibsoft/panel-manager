namespace Itibsoft.PanelManager.Sample.Demo
{
    [Panel(PanelType = PanelType.Window, Order = 0, AssetId = "DemoWindow")]
    public class DemoWindowController : PanelControllerBase<DemoWindow>
    {
        private DemoOverlayController _overlay;
        
        public DemoWindowController(DemoWindow panel) : base(panel)
        {
            panel.OnOverlayOpen += OnOpenOverlayHandle;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            _overlay = PanelManager.LoadPanel<DemoOverlayController>();
        }

        private void OnOpenOverlayHandle()
        {
            _overlay.Open();
        }
    }
}