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
            
            var panel = PanelFactory.Create(meta);

            var controller = CreateInstance(type, panel);

            return (TPanelController)controller;
        }

        public virtual object CreateInstance(Type type, params object[] arguments)
        {
            var instance = Activator.CreateInstance(type, arguments);
            return instance;
        }
    }
}