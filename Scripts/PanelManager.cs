using System;
using System.Collections.Generic;
using System.Reflection;

namespace Itibsoft.PanelManager
{
    public class PanelManager : IPanelManager
    {
        private IPanelControllerFactory _panelControllerFactory;
        private PanelDispatcher _panelDispatcher;
        
        private Dictionary<ushort, IPanelController> _panelsCashed;

        public PanelManager(IPanelControllerFactory panelControllerFactory, PanelDispatcher panelDispatcher)
        {
            _panelControllerFactory = panelControllerFactory;
            _panelDispatcher = panelDispatcher;

            _panelControllerFactory ??= new DefaultPanelControllerFactory();
            _panelDispatcher ??= PanelDispatcher.Create();

            _panelsCashed = new Dictionary<ushort, IPanelController>();
        }

        #region Public API (Methods)
        
        public TPanelController LoadPanel<TPanelController>() where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            var hash = type.GetStableHash();
            var meta = GetMeta<TPanelController>();
            
            if (_panelsCashed.TryGetValue(hash, out var controller))
            {
                return (TPanelController)controller;
            }
            
            controller = _panelControllerFactory.Create<TPanelController>(meta);
            
            var panel = controller.GetPanel();

            var propertyType = panel.GetType().GetProperty("Type", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (propertyType != null)
            {
                var setter = propertyType.GetSetMethod(true);
                
                if (setter != null)  setter.Invoke(panel, new object[] { meta.PanelType });
                else throw new Exception("No set method found for property 'Type'");
            }

            _panelDispatcher.Cache(panel);
            
            InvokeConstructorMethod(panel);

            controller.RegisterCallback<OpenPanelCallback>(OnHandleOpenPanel);
            controller.RegisterCallback<ClosePanelCallback>(OnHandleClosePanel);

            _panelsCashed.Add(hash, controller);
            
            return (TPanelController)controller;
        }

        #endregion

        #region Private API (Methods)

        #region Callback Handlers

        private void OnHandleOpenPanel(OpenPanelCallback callback)
        {
            var panel = callback.Panel;

            switch (panel.Type)
            {
                case PanelType.Window: _panelDispatcher.SetWindow(panel); break;
                case PanelType.Overlay: _panelDispatcher.SetOverlay(panel); break;
                default: throw new ArgumentOutOfRangeException();
            }

            InvokeOnOpenMethod(panel);
        }

        private void OnHandleClosePanel(ClosePanelCallback callback)
        {
            var panel = callback.Panel;
            
            InvokeOnCloseMethod(panel);

            _panelDispatcher.Cache(panel);
        }

        #endregion

        #region Reflection Utils

        private PanelAttribute GetMeta<TPanelController>() where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            var meta = type.GetCustomAttribute<PanelAttribute>();

            if (meta == default)
            {
                throw new Exception($"Not found Attribute.Panel for controller: {type.Name}");
            }

            return meta;
        }

        private void InvokeConstructorMethod(IPanel panel) => InvokeMethodForName(panel, "Constructor");
        private void InvokeOnOpenMethod(IPanel panel) => InvokeMethodForName(panel, "OnOpen");
        private void InvokeOnCloseMethod(IPanel panel) => InvokeMethodForName(panel, "OnClose");

        private void InvokeMethodForName(object instance, string nameMethod)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var method = instance.GetType().GetMethod(nameMethod, BINDING_FLAGS);

            if (method != default)
            {
                method.Invoke(instance, default);
            }
        }

        #endregion

        #endregion
    }
}