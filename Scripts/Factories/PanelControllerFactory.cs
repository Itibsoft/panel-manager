using System;

namespace Itibsoft.PanelManager
{
    public class PanelControllerFactory : IPanelControllerFactory
    {
        protected readonly IPanelFactory PanelFactory;
        
        public PanelControllerFactory(IPanelFactory panelFactory)
        {
            PanelFactory = panelFactory;
        }
        
        public virtual TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            
            var controller = Create(type, meta);

            return (TPanelController)controller;
        }

        public IPanelController Create(Type typePanelController, PanelAttribute meta)
        {
            var panel = PanelFactory.Create(meta);

            var controller = CreateInstance(typePanelController, panel);

            return (IPanelController)controller;
        }

        public virtual object CreateInstance(Type type, params object[] arguments)
        {
            var instance = Activator.CreateInstance(type, arguments);
            return instance;
        }
    }
}