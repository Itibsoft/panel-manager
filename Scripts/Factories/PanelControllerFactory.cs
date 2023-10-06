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
            
            var extraArguments = new object[]
            {
                panel
            };

            var controller = Activator.CreateInstance(type, extraArguments);

            return (TPanelController)controller;
        }
    }
}