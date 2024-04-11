using System;
using System.Collections.Generic;
using Itibsoft.MVP;
using Itibsoft.PanelManager.Tests.Reflections;

namespace Itibsoft.PanelManager.Tests
{
    public class PresenterFactory
    {
        protected readonly IPanelFactory PanelFactory;
        
        public PresenterFactory(IPanelFactory panelFactory)
        {
            PanelFactory = panelFactory;
        }
        
        public IPresenter Create(Type typePresenter, Type typeModel, IPanelMeta meta)
        {
            var panel = PanelFactory.Create(meta);
            var model = CreateInstance<IModel>(typeModel);

            return Create(typePresenter, model, meta);
        }
        
        public IPresenter Create(Type typePresenter, IModel model, IPanelMeta meta)
        {
            var panel = PanelFactory.Create(meta);

            var parameters = new object []
            {
                model,
                panel
            };

            var presenter = CreateInstance<IPresenter>(typePresenter, parameters);

            return presenter;
        }

        private static TInstance CreateInstance<TInstance>(Type type, params object[] arguments) where TInstance : class
        {
            return Activator.CreateInstance(type, arguments) as TInstance;
        }
    }
}