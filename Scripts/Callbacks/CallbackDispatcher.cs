using System;
using System.Collections.Generic;

namespace Itibsoft.PanelManager
{
    public class CallbackDispatcher
    {
        private readonly Dictionary<Type, List<Delegate>> _callbacks = new();

        public void RegisterCallback<TCallback>(PanelCallbackDelegate<TCallback> callback) where TCallback : IPanelCallback
        {
            if (_callbacks.TryGetValue(typeof(TCallback), out var value))
            { 
                value.Add(callback);
                return;
            }

            _callbacks.Add(typeof(TCallback), new List<Delegate> { callback });
        }

        public void UnRegisterCallback<TCallback>(PanelCallbackDelegate<TCallback> callback) where TCallback : IPanelCallback
        {
            if (_callbacks.TryGetValue(typeof(TCallback), out var value))
            {
                value.Remove(callback);
            }
        }

        public void InvokeCallback<TCallback>(TCallback callbackValue) where TCallback : IPanelCallback
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