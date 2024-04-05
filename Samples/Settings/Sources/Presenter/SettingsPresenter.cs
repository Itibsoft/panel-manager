namespace Settings.Presenter
{
    public class SettingsPresenter : System.IDisposable
    {
        #region fields

        #region private fields

        private readonly IModel _model;
        private readonly IView _view;

        #endregion

        #endregion

        #region initialize

        public SettingsPresenter(IModel model, IView view)
        {
            _model = model;
            _view = view;

            view.SetCountClicked(_model.CountClicked);
            view.OnClickPlus += OnClickPlusHandle;
        }

        #endregion

        #region private api

        private void OnClickPlusHandle()
        {
            var newValue = _model.CountClicked + 1;
            
            _model.CountClicked = newValue;
            _view.SetCountClicked(newValue);
        }

        #endregion

        public void Dispose()
        {
            _model.Dispose();
            _view.Dispose();
        }
    }
}