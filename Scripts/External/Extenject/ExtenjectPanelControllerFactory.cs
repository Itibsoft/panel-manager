#if EXTENJECT 

using System;
using Zenject;

namespace Itibsoft.PanelManager.External
{
    public class ExtenjectPanelControllerFactory : PanelControllerFactory
    {
        private readonly DiContainer _diContainer;

        public ExtenjectPanelControllerFactory(IPanelFactory panelFactory, DiContainer diContainer) : base(panelFactory)
        {
            _diContainer = diContainer;
        }

        public override object CreateInstance(Type type, params object[] arguments)
        {
            return _diContainer.Instantiate(type, arguments);
        }
    }
}

#endif