using System;
using System.Collections.Generic;
using System.Reflection;

namespace Itibsoft.PanelManager
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

        public static void SetMeta(IPanel panel, PanelAttribute meta)
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

        public static void InvokeConstructorMethod(IPanel panel)
        {
            InvokeMethod(panel, _panelConstructorReflectionCached, "Constructor");
        }

        public static void InvokeOnOpenMethod(IPanel panel)
        {
            InvokeMethod(panel, _panelOpenReflectionCached, "OnOpen");
        }

        public static void InvokeOnCloseMethod(IPanel panel)
        {
            InvokeMethod(panel, _panelCloseReflectionCached, "OnClose");
        }

        private static MethodInfo GetMethodForName(object instance, string nameMethod)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return instance.GetType().GetMethod(nameMethod, BINDING_FLAGS);
        }
        
        private static void InvokeMethod(IPanel panel, Dictionary<IPanel, MethodInfo> reflectionCache, string methodName)
        {
            if (reflectionCache.TryGetValue(panel, out var method))
            {
                method?.Invoke(panel, default);
                return;
            }

            method = GetMethodForName(panel, methodName);

            if (method != default)
            {
                method.Invoke(panel, default);
                reflectionCache.Add(panel, method);
            }
        }
    }
}