using System.Threading.Tasks;
using Itibsoft.PanelManager;

namespace Itibsoft.MVP
{
    public interface IPresenter : IViewHandler, IModelHandler
    {
        public Task Start();
    }
}