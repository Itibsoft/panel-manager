using UnityEngine;

namespace Itibsoft.PanelManager.Sample.Demo
{
    public class Main : MonoBehaviour
    {
#if REFLEX
        [Reflex.Attributes.Inject] private IPanelManager _panelManager;
        
        private void Start()
        {
            var window = _panelManager.LoadPanel<DemoWindowController>();
            
            window.Open();
        }
#endif
    }
}

