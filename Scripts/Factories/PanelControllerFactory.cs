using System;

namespace Itibsoft.PanelManager
{
    public class PanelControllerFactory : IPanelControllerFactory
    {
        private readonly IPanelFactory _panelFactory;
        
        public PanelControllerFactory(IPanelFactory panelFactory)
        {
            _panelFactory = panelFactory;
        }
        
        public virtual TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            
            var panel = _panelFactory.Create(meta);
            
            var extraArguments = new object[]
            {
                panel
            };

            var controller = Activator.CreateInstance(type, extraArguments);

            return (TPanelController)controller;
        }
    }
}