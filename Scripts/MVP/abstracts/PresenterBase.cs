using a;

namespace Itibsoft.PanelManager.Tests
{
    public class PresenterBase<TModel, TView> : IPresenter 
        where TModel : IModel 
        where TView : IView
    {
        protected TView View { get; }
        protected TModel Model { get; }

        public void Open()
        {
            
        }

        public void Close()
        {
            
        }

        protected virtual void OnViewOpen_Before() { }
        protected virtual void OnViewOpened_After() { }
        
        protected virtual void OnModelLoad_Before() { }
        protected virtual void OnModelLoaded_After() { }
    }
}