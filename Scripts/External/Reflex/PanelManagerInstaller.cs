#if REFLEX

using Reflex.Core;

namespace Itibsoft.PanelManager.External
{
    public static class PanelManagerInstaller
    {
        public static void Install(ContainerBuilder containerBuilder, PanelDispatcher panelDispatcher = default)
        {
#if ADDRESSABLES
            var panelFactory = new ResourcesPanelFactory();
#else
            var panelFactory = new AddressablesPanelFactory();
#endif
			var panelControllerFactory = new ReflexPanelControllerFactory(panelFactory);
            
            var panelManager = PanelManagerBuilder
                .Create()
                .SetPanelDispatcher(panelDispatcher)
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