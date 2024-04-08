using System.Threading.Tasks;
using a;
using JetBrains.Annotations;

namespace Itibsoft.PanelManager.Tests
{
    public abstract class PresenterBase<TModel, TView> : IPresenter 
        where TModel : IModel 
        where TView : IView
    {
        protected MVPManager Manager { get; [UsedImplicitly] set; }
        protected TView View { get; }
        protected TModel Model { get; }

        protected PresenterBase(TModel model, TView view)
        {
            Model = model;
            View = view;
        }
        
        public async Task Start()
        {
            await Model.OnDataFetch_Before();
            await OnModelLoad_Before();

            var data = await Model.FetchData();
            
            await Model.OnDataFetched_After(data);
            await OnModelLoaded_After();
        }

        public void Open()
        {
            OnViewOpen_Before();
            
            Manager.RequestOpen(this, status =>
            {
                OnViewOpened_After();
            });
        }

        public void Close()
        {
            Manager.RequestClose(this, status =>
            {
                
            });
        }
        
        public IView GetView()
        {
            return View;
        }
        
        public IModel GetModel()
        {
            return Model;
        }

        protected virtual void OnViewOpen_Before() { }
        protected virtual void OnViewOpened_After() { }

        protected virtual Task OnModelLoad_Before() { return Task.CompletedTask; }
        protected virtual Task OnModelLoaded_After() { return Task.CompletedTask; }

        public void Dispose()
        {
            
        }
    }
}