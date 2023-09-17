using Assets.Project.Scripts.UI.Common;

namespace Assets.Project.Scripts.UI
{
    public interface IPanelControllerFactory
    {
        public TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController;
    }
}