using Itibsoft.PanelManager;
using UnityEngine;

namespace Example.Demo
{
    public class StartApp : MonoBehaviour
    {
        private void Awake()
        {
            var panelManager = PanelManagerBuilder
                .Create()
                .Build();

            var demoWindowController = panelManager.LoadPanel<DemoWindowController>();
            
            demoWindowController.Open();
        }
    }
}