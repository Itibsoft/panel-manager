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
        
        public virtual TPresenter Create<TPresenter>(Type typeModel, IPanelMeta meta) where TPresenter : IPresenter
        {
            var typePresenter = typeof(TPresenter);
            
            var presenter = Create(typePresenter, typeModel, meta);

            return (TPresenter)presenter;
        }
        
        public IPresenter Create(Type typePresenter, Type typeModel, IPanelMeta meta)
        {
            var panel = PanelFactory.Create(meta);

            var parameters = new List<object>();

            if (meta is PresenterMeta presenterMeta)
            {
                var model = CreateInstance(typeModel);
                parameters.Add(model);
            }

            parameters.Add(panel);

            var presenter = CreateInstance(typePresenter, parameters.ToArray());

            return (IPresenter)presenter;
        }

        public virtual object CreateInstance(Type type, params object[] arguments)
        {
            var instance = Activator.CreateInstance(type, arguments);
            return instance;
        }
    }
}