using System;
using System.Collections.Generic;

namespace Itibsoft.PanelManager
{
#if EXTENJECT
    [JetBrains.Annotations.UsedImplicitly]
#endif
    public class PanelManager : IPanelManager
    {
        #region Fields

        #region Private Fields

        private readonly IPanelControllerFactory _panelControllerFactory;
        private readonly PanelDispatcher _panelDispatcher;

        private readonly Dictionary<ushort, IPanelController> _panelsCashed = new();

        #endregion

        #endregion;

        #region Constructors

#if EXTENJECT
        public PanelManager(IPanelControllerFactory panelControllerFactory, PanelDispatcher panelDispatcher)
        {
            _panelControllerFactory = panelControllerFactory;
            _panelDispatcher = panelDispatcher;
        }
#else
        public PanelManager(IPanelControllerFactory panelControllerFactory, PanelDispatcher panelDispatcher)
        {
            _panelControllerFactory = panelControllerFactory;
            _panelDispatcher = panelDispatcher;

            if (_panelControllerFactory == default)
            {
#if ADDRESSABLES
                var panelFactory = new External.AddressablesPanelFactory();
#else
                var panelFactory = new ResourcesPanelFactory();
#endif

                _panelControllerFactory = new PanelControllerFactory(panelFactory);
            }

            _panelDispatcher ??= PanelDispatcher.Create();
        }
#endif

        #endregion

        #region Public API (Methods)

        public TPanelController LoadPanel<TPanelController>() where TPanelController : IPanelController
        {
            var meta = PanelReflector.GetMeta<TPanelController>();
            var type = typeof(TPanelController);
            var hash = type.GetStableHash();

            if (_panelsCashed.TryGetValue(hash, out var controller))
            {
                return (TPanelController)controller;
            }

            controller = _panelControllerFactory.Create<TPanelController>(meta);

            var panel = controller.GetPanel();

            PanelReflector.SetMeta(panel, meta);

            _panelDispatcher.Cache(panel);

            PanelReflector.InvokeConstructorMethod(panel);

            controller.RegisterCallback<OpenPanelCallback>(OnHandleOpenPanel);
            controller.RegisterCallback<ClosePanelCallback>(OnHandleClosePanel);
            controller.RegisterCallback<ReleasePanelCallback>(OnReleasePanelHandle);

            _panelsCashed.Add(hash, controller);

            return (TPanelController)controller;
        }

        private void OnReleasePanelHandle(ReleasePanelCallback callback)
        {
            var controller = callback.PanelController;
            var panel = controller.GetPanel();

            var type = controller.GetType();
            var hash = type.GetStableHash();

            panel.Dispose();
            panel.SetActive(false);

            controller.Dispose();

            var panelGameObject = panel.GetGameObject();

#if ADDRESSABLES
            UnityEngine.AddressableAssets.Addressables.ReleaseInstance(panelGameObject);
#else
            UnityEngine.Object.DestroyImmediate(panelGameObject);
            UnityEngine.Resources.UnloadUnusedAssets();
#endif

            _panelsCashed.Remove(hash);
        }

        #endregion

        #region Private API (Methods)

        #region Callback Handlers

        private void OnHandleOpenPanel(OpenPanelCallback callback)
        {
            var panel = callback.Panel;

            switch (panel.Meta.PanelType)
            {
                case PanelType.Window:
                    _panelDispatcher.SetWindow(panel);
                    break;
                case PanelType.Overlay:
                    _panelDispatcher.SetOverlay(panel);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            PanelReflector.InvokeOnOpenMethod(panel);
        }

        private void OnHandleClosePanel(ClosePanelCallback callback)
        {
            var panel = callback.Panel;

            PanelReflector.InvokeOnCloseMethod(panel);

            _panelDispatcher.Cache(panel);
        }

        #endregion

        #endregion

#if EXTENJECT
        [JetBrains.Annotations.UsedImplicitly]
        public class Factory : Zenject.PlaceholderFactory<IPanelManager>
        {
            
        }
#endif
    }
}