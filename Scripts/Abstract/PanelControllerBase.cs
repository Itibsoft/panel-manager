namespace Itibsoft.PanelManager
{
    public abstract class PanelControllerBase<TPanel> : IPanelController<TPanel> where TPanel : IPanel
    {
        public TPanel Panel { get; }
        
        private readonly CallbackDispatcher _callbackDispatcher;
        
        protected PanelControllerBase(TPanel panel)
        {
            Panel = panel;
            
            _callbackDispatcher = new CallbackDispatcher();
        }
        
        public void Open()
        {
            OnOpenPanel();
            
            var openCallback = new OpenPanelCallback(Panel);
            _callbackDispatcher.InvokeCallback(openCallback);
        }

        public void Close()
        {
            OnClosePanel();
            
            var closeCallback = new ClosePanelCallback(Panel);
            _callbackDispatcher.InvokeCallback(closeCallback);
        }

        public IPanel GetPanel()
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

        public void Release()
        {
            var closeCallback = new ReleasePanelCallback(this);
            _callbackDispatcher.InvokeCallback(closeCallback);
        }

        public virtual void Dispose()
        {
            
        }

        protected virtual void OnOpenPanel()
        {
            
        }
        
        protected virtual void OnClosePanel()
        {
            
        }
    }
}