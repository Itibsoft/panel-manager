using UnityEngine;

namespace Itibsoft.PanelManager.Sample.Demo
{
    public class Main : MonoBehaviour
    {
        [Reflex.Attributes.Inject] private IPanelManager _panelManager;
        
        private void Start()
        {
            var window = _panelManager.LoadPanel<DemoWindowController>();
            
            window.Open();
        }
    }
}

