#if EXTENJECT

namespace Itibsoft.PanelManager.External
{
    [JetBrains.Annotations.UsedImplicitly]
    public class PanelManagerInstaller : Zenject.Installer<PanelDispatcher, PanelManagerInstaller>
    {
        private readonly PanelDispatcher _panelDispatcher;

        public PanelManagerInstaller(PanelDispatcher panelDispatcher = default)
        {
            _panelDispatcher = panelDispatcher == default ? panelDispatcher : PanelDispatcher.Create();
        }

        public override void InstallBindings()
        {
            Container
                .Bind<IPanelFactory>()
#if ADDRESSABLES
                .To<AddressablesPanelFactory>()
#else
                .To<ResourcesPanelFactory>()
#endif
                .AsSingle()
                .Lazy();

            Container
                .Bind<IPanelControllerFactory>()
                .To<ExtenjectPanelControllerFactory>()
                .AsSingle()
                .Lazy();

            var arguments = new[]
            {
                _panelDispatcher
            };

            var panelManager = Container.Instantiate<PanelManager>(arguments);

            Container
                .Bind<IPanelManager>()
                .FromInstance(panelManager)
                .AsSingle()
                .Lazy();
        }
    }
}

#endif