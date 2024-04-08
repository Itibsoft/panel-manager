using System;
using System.Collections.Generic;
using System.Reflection;
using a;
using Itibsoft.PanelManager.Tests;
using Itibsoft.PanelManager.Tests.Reflections;

namespace Itibsoft.PanelManager
{
    internal static class PanelReflector
    {
        private static readonly Dictionary<object, MethodInfo> _panelConstructorReflectionCached = new();
        private static readonly Dictionary<object, MethodInfo> _panelOpenReflectionCached = new();
        private static readonly Dictionary<object, MethodInfo> _panelCloseReflectionCached = new();
        private static readonly Dictionary<object, MethodInfo> _controllerLoadReflectionCached = new();

        internal static IPanelMeta GetMeta<TPanelController>() where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);

            return GetMeta(type);
        }

        internal static IPanelMeta GetMeta(Type panelControllerType)
        {
            if (panelControllerType.GetCustomAttribute<PanelAttribute>() is not IPanelMeta meta)
            {
                meta = panelControllerType.GetCustomAttribute<PresenterMeta>();
            }

            if (meta == default)
            {
                throw new Exception($"Not found Attribute.Panel for controller: {panelControllerType.Name}");
            }

            return meta;
        }

        internal static void SetMeta(IViewMono panel, IPanelMeta meta)
        {
            var propertyType = panel.GetType().GetProperty("Meta",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (propertyType != null)
            {
                var setter = propertyType.GetSetMethod(true);

                if (setter != null) setter.Invoke(panel, new object[] { meta });
                else throw new Exception("No set method found for property 'Meta'");
            }
        }

        internal static void SetPanelManager(IPanelController controller, IPanelManager panelManager)
        {
            var propertyType = controller.GetType().GetProperty("PanelManager",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (propertyType != null)
            {
                var setter = propertyType.GetSetMethod(true);

                if (setter != null) setter.Invoke(controller, new object[] { panelManager });
                else throw new Exception("No set method found for property 'PanelManager'");
            }
        }
        
        internal static void SetPresenter(IPresenter controller, MVPManager manager)
        {
            var propertyType = controller.GetType().GetProperty("Manager",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (propertyType != null)
            {
                var setter = propertyType.GetSetMethod(true);

                if (setter != null) setter.Invoke(controller, new object[] { manager });
                else throw new Exception("No set method found for property 'MVPManager'");
            }
        }
        
        internal static void InvokeConstructorMethod(IViewMono panel)
        {
            InvokeMethod(panel, _panelConstructorReflectionCached, "Constructor");
        }

        internal static void InvokeOnOpenMethod(object instance)
        {
            InvokeMethod(instance, _panelOpenReflectionCached, "OnOpen");
        }

        internal static void InvokeOnCloseMethod(object instance)
        {
            InvokeMethod(instance, _panelCloseReflectionCached, "OnClose");
        }
        
        internal static void InvokeOnLoadMethod(object instance)
        {
            InvokeMethod(instance, _controllerLoadReflectionCached, "OnLoad");
        }
        
        internal static void ClearCached(IViewMono panel)
        {
            _panelConstructorReflectionCached.Remove(panel);
            _panelOpenReflectionCached.Remove(panel);
            _panelCloseReflectionCached.Remove(panel);
        }

        private static MethodInfo GetMethodForName(object instance, string nameMethod)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return instance.GetType().GetMethod(nameMethod, BINDING_FLAGS);
        }
        
        private static void InvokeMethod(object panel, IDictionary<object, MethodInfo> reflectionCache, string methodName)
        {
            if (reflectionCache.TryGetValue(panel, out var method))
            {
                method?.Invoke(panel, default);
                return;
            }

            method = GetMethodForName(panel, methodName);

            if (method == default)
            {
                return;
            }
            
            method.Invoke(panel, default);
            reflectionCache.Add(panel, method);
        }
    }
}