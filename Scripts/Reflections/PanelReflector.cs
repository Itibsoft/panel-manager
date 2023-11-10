using System;
using System.Collections.Generic;
using System.Reflection;

namespace Itibsoft.PanelManager.Reflections
{
    public static class PanelReflector
    {
        private static readonly Dictionary<IPanel, MethodInfo> _panelConstructorReflectionCached = new();
        private static readonly Dictionary<IPanel, MethodInfo> _panelOpenReflectionCached = new();
        private static readonly Dictionary<IPanel, MethodInfo> _panelCloseReflectionCached = new();
        
        public static PanelAttribute GetMeta<TPanelController>() where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            var meta = type.GetCustomAttribute<PanelAttribute>();

            if (meta == default)
            {
                throw new Exception($"Not found Attribute.Panel for controller: {type.Name}");
            }

            return meta;
        }

        public static void InvokeConstructorMethod(IPanel panel)
        {
            if (_panelConstructorReflectionCached.TryGetValue(panel, out var method))
            {
                method?.Invoke(panel, default);
                return;
            }
            
            method = GetMethodForName(panel, "Constructor");
            
            if (method != default)
            {
                method.Invoke(panel, default);
                _panelConstructorReflectionCached.Add(panel, method);
            }
        }

        public static void InvokeOnOpenMethod(IPanel panel)
        {
            if (_panelOpenReflectionCached.TryGetValue(panel, out var method))
            {
                method?.Invoke(panel, default);
                return;
            }
            
            method = GetMethodForName(panel, "OnOpen");
            
            if (method != default)
            {
                method.Invoke(panel, default);
                _panelOpenReflectionCached.Add(panel, method);
            }
        }

        public static void InvokeOnCloseMethod(IPanel panel)
        {
            if (_panelCloseReflectionCached.TryGetValue(panel, out var method))
            {
                method?.Invoke(panel, default);
                return;
            }
            
            method = GetMethodForName(panel, "OnClose");
            
            if (method != default)
            {
                method.Invoke(panel, default);
                _panelCloseReflectionCached.Add(panel, method);
            }
        }

        private static MethodInfo GetMethodForName(object instance, string nameMethod)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return instance.GetType().GetMethod(nameMethod, BINDING_FLAGS);
        }
    }
}