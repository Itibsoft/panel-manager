using Itibsoft.PanelManager;
using UnityEngine;

namespace Example.Demo
{
    [Panel(PanelType = PanelType.Window, AssetId = "DemoWindow/Prefabs/DemoWindow.prefab")]
    public class DemoWindowController : PanelControllerBase<DemoWindow>
    {
        public DemoWindowController(DemoWindow panel) : base(panel)
        {
            panel.OnClick += OnClickHandle;
            panel.OnCloseWindow += OnCloseWindowHandle;
            panel.OnReleaseWindow += OnReleaseWindowHandle;
        }
        
        private void OnClickHandle()
        {
            Debug.Log("DemoWindowController.Click");
        }
        
        private void OnCloseWindowHandle()
        {
            Debug.Log("DemoWindowController.Close");
            Close();
        }
        
        private void OnReleaseWindowHandle()
        {
            Debug.Log("DemoWindowController.Release");
            Release();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Debug.Log("DemoWindowController.Dispose");
        }
    }
}