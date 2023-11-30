using System;
using System.Collections.Generic;

namespace Itibsoft.PanelManager
{
    internal sealed class CallbackDispatcher
    {
        private readonly Dictionary<Type, List<Delegate>> _callbacks = new();

        internal void RegisterCallback<TCallback>(PanelCallbackDelegate<TCallback> callback)
            where TCallback : IPanelCallback
        {
            if (_callbacks.TryGetValue(typeof(TCallback), out var value))
            {
                value.Add(callback);
                return;
            }

            _callbacks.Add(typeof(TCallback), new List<Delegate> { callback });
        }

        internal void UnRegisterCallback<TCallback>(PanelCallbackDelegate<TCallback> callback)
            where TCallback : IPanelCallback
        {
            if (_callbacks.TryGetValue(typeof(TCallback), out var value))
            {
                value.Remove(callback);
            }
        }

        internal void InvokeCallback<TCallback>(TCallback callbackValue) where TCallback : IPanelCallback
        {
            if (_callbacks.TryGetValue(typeof(TCallback), out var value))
            {
                value.ForEach(@delegate =>
                {
                    var panelCallback = (PanelCallbackDelegate<TCallback>)@delegate;
                    panelCallback?.Invoke(callbackValue);
                });
            }
        }
    }
}