using System;
using System.Collections;
using Itibsoft.MVP;
using JetBrains.Annotations;

namespace Itibsoft.PanelManager
{
    public abstract class PanelControllerBase<TPanel> : IPanelController<TPanel> where TPanel : IViewMono
    {
        public TPanel Panel { get; }
        
        protected IPanelManager PanelManager { get; [UsedImplicitly] set; }
        
        private readonly CallbackDispatcher _callbackDispatcher;
        
        protected PanelControllerBase(TPanel panel)
        {
            Panel = panel;
            
            _callbackDispatcher = new CallbackDispatcher();
        }
        
        public void Open()
        {
            var openCallback = new OpenPanelCallback(Panel);
            _callbackDispatcher.InvokeCallback(openCallback);
        }

        public void Close()
        {
            var closeCallback = new ClosePanelCallback(Panel);
            _callbackDispatcher.InvokeCallback(closeCallback);
        }

        public void Release()
        {
            var closeCallback = new ReleasePanelCallback(this);
            _callbackDispatcher.InvokeCallback(closeCallback);
        }

        public IView GetView()
        {
            return Panel;
        }

        public void RegisterCallback<TCallback>(PanelCallbackDelegate<TCallback> callback) where TCallback : IPanelCallback
        {
            _callbackDispatcher.RegisterCallback(callback);
        }

        public void UnRegisterCallback<TCallback>(PanelCallbackDelegate<TCallback> callback) where TCallback : IPanelCallback
        {
            _callbackDispatcher.UnRegisterCallback(callback);
        }

        protected virtual void OnLoad() { }

        protected virtual IEnumerator OnAfterOpen() { yield return null; }
        protected virtual IEnumerator OnOpen()  { yield return null; }
        protected virtual IEnumerator OnBeforeOpen()  { yield return null; }
        
        protected virtual IEnumerator OnAfterClose()  { yield return null; }
        protected virtual IEnumerator OnClose()  { yield return null; }
        protected virtual IEnumerator OnBeforeClose()  { yield return null; }

        protected virtual void OnUnload() { }
        
        void IDisposable.Dispose()
        {
            OnUnload();
        }
    }
}