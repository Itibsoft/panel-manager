using System.Threading.Tasks;
using Itibsoft.PanelManager;

namespace a
{
    public interface IPresenter : IViewHandler, IModelHandler
    {
        public Task Start();
    }
}