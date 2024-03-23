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

        #region Public Fields

        public PanelDispatcher PanelDispatcher { get; }

        #endregion
        
        #region Private Fields

        private readonly IPanelControllerFactory _panelControllerFactory;

        private readonly Dictionary<ushort, IPanelController> _panelsCashed = new();

        #endregion

        #endregion;

        #region Initialize

        public PanelManager(IPanelControllerFactory panelControllerFactory, PanelDispatcher panelDispatcher)
        {
#if EXTENJECT
            _panelControllerFactory = panelControllerFactory;
            PanelDispatcher = panelDispatcher;
#else
            _panelControllerFactory = panelControllerFactory;
            
            if (_panelControllerFactory == default)
            {
#if ADDRESSABLES
                var panelFactory = new External.AddressablesPanelFactory();
#else
                var panelFactory = new ResourcesPanelFactory();
#endif

                _panelControllerFactory = new PanelControllerFactory(panelFactory);
            }

            if (panelDispatcher == default)
            {
                panelDispatcher = PanelDispatcherBuilder.Create().Build();
            }

            PanelDispatcher = panelDispatcher;
#endif
        }


        #endregion

        #region Public API (Methods)

        public TPanelController LoadPanel<TPanelController>() where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            
            var controller = LoadPanelInternal(type);
            
            return (TPanelController)controller;
        }
        
        public void ClosePanels(params Type[] typeControllers)
        {
            for (int index = 0, count = typeControllers.Length; index < count; index++)
            {
                var typeController = typeControllers[index];

                var controller = LoadPanelInternal(typeController);
                controller.Close();
            }
        }

        public void OpenPanels(params Type[] typeControllers)
        {
            for (int index = 0, count = typeControllers.Length; index < count; index++)
            {
                var typeController = typeControllers[index];

                var controller = LoadPanelInternal(typeController);
                controller.Open();
            }
        }

        private IPanelController LoadPanelInternal(Type typePanelController)
        {
            var meta = PanelReflector.GetMeta(typePanelController);
            var hash = typePanelController.GetStableHash();

            if (_panelsCashed.TryGetValue(hash, out var controller))
            {
                return controller;
            }

            controller = _panelControllerFactory.Create(typePanelController, meta);

            var panel = controller.GetPanel();

            PanelReflector.SetPanelManager(controller, this);
            PanelReflector.SetMeta(panel, meta);

            PanelDispatcher.Cache(panel);

            PanelReflector.InvokeConstructorMethod(panel);
            PanelReflector.InvokeOnLoadMethod(controller);

            controller.RegisterCallback<OpenPanelCallback>(OnHandleOpenPanel);
            controller.RegisterCallback<ClosePanelCallback>(OnHandleClosePanel);
            controller.RegisterCallback<ReleasePanelCallback>(OnReleasePanelHandle);

            _panelsCashed.Add(hash, controller);

            return controller;
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
                    PanelDispatcher.SetWindow(panel);
                    break;
                case PanelType.Overlay:
                    PanelDispatcher.SetOverlay(panel);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            PanelReflector.InvokeOnOpenMethod(panel);
        }

        private void OnHandleClosePanel(ClosePanelCallback callback)
        {
            var panel = callback.Panel;

            PanelReflector.InvokeOnCloseMethod(panel);

            PanelDispatcher.Cache(panel);
        }

        private void OnReleasePanelHandle(ReleasePanelCallback callback)
        {
            var controller = callback.PanelController;
            var panel = controller.GetPanel();

            var type = controller.GetType();
            var hash = type.GetStableHash();

            PanelReflector.ClearCached(panel);

            PanelDispatcher.Release(panel);

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

        #endregion

#if EXTENJECT
        [JetBrains.Annotations.UsedImplicitly]
        public class Factory : Zenject.PlaceholderFactory<PanelManager>
        {
        }
#endif
    }
}