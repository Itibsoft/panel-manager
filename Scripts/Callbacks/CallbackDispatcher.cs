using System;
using System.Collections.Generic;
using Assets.Project.Scripts.UI;

namespace Project.Scripts.PanelManager.EventsCallbacks
{
    public class CallbackDispatcher
    {
        private readonly Dictionary<Type, List<Delegate>> _callbacks;

        public CallbackDispatcher()
        {
            _callbacks = new Dictionary<Type, List<Delegate>>();
        }
        
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