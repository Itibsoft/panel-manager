#if REFLEX

using Reflex.Core;
using UnityEngine;

namespace Itibsoft.PanelManager.External
{
    public class PanelManagerInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private PanelDispatcher _panelDispatcher;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            var panelFactory = new ResourcesPanelFactory();
            var panelControllerFactory = new ReflexPanelControllerFactory(panelFactory);
            
            var panelManager = PanelManagerBuilder
                .Create()
                .SetPanelDispatcher(_panelDispatcher)
                .SetPanelControllerFactory(panelControllerFactory)
                .Build();
            
            containerBuilder.AddSingleton(panelManager, typeof(IPanelManager));
            
            containerBuilder.OnContainerBuilt += container =>
            {
                panelControllerFactory.SetContainer(container);
            };
        }
    }
}

#endif