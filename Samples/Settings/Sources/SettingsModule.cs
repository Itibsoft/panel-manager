using Settings.Model;
using Settings.Presenter;
using Settings.View;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Settings
{
    public class SettingsModule : System.IDisposable
    {
        #region fields

        private const string VIEW_ASSET_PATH = "panels/overlays/settings_view";

        private readonly SettingsPresenter _presenter;

        #endregion

        #region initialize

        private SettingsModule(SettingsPresenter presenter)
        {
            _presenter = presenter;
        }
        
        #region builder

        public static SettingsModule Create(Canvas canvas)
        {
            var model = new SettingsModel();
            var view = CreateSettingsView(canvas);

            var presenter = new SettingsPresenter(model, view);

            return new SettingsModule(presenter);
        }

        private static SettingsView CreateSettingsView(Canvas canvas)
        {
            var prefab = Addressables
                .LoadAssetAsync<GameObject>(VIEW_ASSET_PATH)
                .WaitForCompletion();

            var instance = Object.Instantiate(prefab);

            var view = instance.GetComponent<SettingsView>();
            
            view.transform.SetParent(canvas.transform);

            var rectTransform = view.GetComponent<RectTransform>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            rectTransform.localScale = Vector3.one;

            return view;
        }

        #endregion

        #endregion

        public void Dispose()
        {
            _presenter?.Dispose();
        }
    }
}