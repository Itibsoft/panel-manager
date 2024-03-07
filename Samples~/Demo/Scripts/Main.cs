using UnityEngine;

namespace Itibsoft.PanelManager.Sample.Demo
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            var panelManager = PanelManagerBuilder.Create().Build();
            
            var window = panelManager.LoadPanel<DemoWindowController>();
            
            window.Open();
        }
    }
}

