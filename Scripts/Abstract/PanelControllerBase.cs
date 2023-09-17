namespace Itibsoft.PanelManager
{
    public abstract class PanelControllerBase<TPanel> : IPanelController<TPanel> where TPanel : IPanel
    {
        private readonly CallbackDispatcher _callbackDispatcher;
        public TPanel Panel { get; }
        
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
    }
}