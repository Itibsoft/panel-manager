using System.Threading.Tasks;

namespace a
{
    public interface IModel
    {
        public Task<IData> FetchData();
        
        public virtual Task OnDataFetch_Before() { return Task.CompletedTask; }
        public virtual Task OnDataFetched_After(IData data) { return Task.CompletedTask; }
    }

    public interface IModel<out TData> : IModel where TData : IData
    {
        public TData Data { get; }
    }
}