using Assets.Project.Scripts.UI;
using Assets.Project.Scripts.UI.Common;

namespace Project.Scripts.PanelManager.Builder
{
    public class PanelManagerBuilder
    {
        private IPanelControllerFactory _panelControllerFactory;
        private PanelDispatcher _panelDispatcher;
        
        public static PanelManagerBuilder Create()
        {
            return new PanelManagerBuilder();
        }

        public PanelManagerBuilder SetupPanelDispatcher(PanelDispatcher panelDispatcher)
        {
            _panelDispatcher = panelDispatcher;

            return this;
        }

        public PanelManagerBuilder SetupPanelControllerFactory(IPanelControllerFactory panelControllerFactory)
        {
            _panelControllerFactory = panelControllerFactory;

            return this;
        }

        public PanelManager Build()
        {
            return new PanelManager(_panelControllerFactory, _panelDispatcher);
        }
    }
}