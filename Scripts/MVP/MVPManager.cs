using System;
using System.Collections.Generic;
using a;

namespace Itibsoft.PanelManager.Tests
{
    public class MVPManager
    {
        public PanelDispatcher PanelDispatcher;
        private PresenterFactory _presenterFactory;
        
        private readonly Dictionary<ushort, IPresenter> _presenters = new();
        
        public MVPManager()
        {
            PanelDispatcher = PanelDispatcherBuilder.Create().Build();
            
#if ADDRESSABLES
            var panelFactory = new External.AddressablesPanelFactory();
#else
            var panelFactory = new ResourcesPanelFactory();
#endif
            
            _presenterFactory = new PresenterFactory(panelFactory);
        }
        
        public TPresenter LoadPanel<TPresenter, TModel>() 
            where TPresenter : IPresenter 
            where TModel : IModel
        {
            var type = typeof(TPresenter);
            var typeModel = typeof(TModel);
            
            var presenter = LoadPanelInternal(type, typeModel);
            
            PanelReflector.SetPresenter(presenter, this);
            
            return (TPresenter)presenter;
        }
        
        public void RequestOpen(IPresenter presenter, Action<bool> callback)
        {
            var view = presenter.GetView();

            if (view is IViewMono viewMono)
            {
                switch (viewMono.Meta.PanelType)
                {
                    case PanelType.Window:
                        PanelDispatcher.SetWindow(viewMono);
                        break;
                    case PanelType.Overlay:
                        PanelDispatcher.SetOverlay(viewMono);
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            
            callback?.Invoke(true);
        }

        public void RequestClose(IPresenter presenter, Action<bool> callback)
        {
            var view = presenter.GetView();

            if (view is IViewMono viewMono)
            {
                PanelDispatcher.Cache(viewMono);
            }
            
            callback?.Invoke(true);
        }
        
        private IPresenter LoadPanelInternal(Type typePresenter, Type typeModel)
        {
            var meta = PanelReflector.GetMeta(typePresenter);
            var hash = typePresenter.GetStableHash();

            if (_presenters.TryGetValue(hash, out var presenter))
            {
                return presenter;
            }

            presenter = _presenterFactory.Create(typePresenter, typeModel, meta);

            var panel = presenter.GetView();

            if (panel is IViewMono viewMono)
            {
                PanelReflector.SetMeta(viewMono, meta);
                PanelDispatcher.Cache(viewMono);
            }
            
            _presenters.Add(hash, presenter);

            return presenter;
        }
    }
}