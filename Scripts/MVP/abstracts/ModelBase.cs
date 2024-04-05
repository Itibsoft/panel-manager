using a;

namespace Itibsoft.PanelManager.Tests
{
    public abstract class ModelBase<TData> : IModel
        where TData : IData
    {
        protected TData Data { get; }
        
        protected abstract TData FetchData();
        
        protected virtual void OnDataFetch_Before() { }
        protected virtual void OnDataFetched_After(TData data) { }
    }
}