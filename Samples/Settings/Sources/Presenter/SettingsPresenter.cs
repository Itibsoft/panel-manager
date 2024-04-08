using System.Threading.Tasks;
using Itibsoft.PanelManager;
using Itibsoft.PanelManager.Tests;
using Itibsoft.PanelManager.Tests.Reflections;
using Settings.Shared;
using UniRx;
using UnityEngine;

namespace Settings.Presenter
{
    [PresenterMeta(
        PanelType = PanelType.Window, 
        Order = 0, 
        AssetId = "panels/overlays/settings_view"
    )]
    public class SettingsPresenter : PresenterBase<ISettingsModel, ISettingsView>
    {
        private bool _isReady;
        
        public SettingsPresenter(ISettingsModel model, ISettingsView view) : base(model, view)
        {
            view.IncrementValueCommand.Subscribe(_ => model.ClickedCountProperty.Value++);
            model.ClickedCountProperty.Subscribe(value => View.ClickedValueProperty.Value = value.ToString());
        }

        protected override void OnViewOpened_After()
        {
            Debug.Log("End Open");
        }

        protected override void OnViewOpen_Before()
        {
            Debug.Log("Start Open");
        }

        protected override Task OnModelLoaded_After()
        {
            Debug.LogError("OnModelLoaded_After");

            return Task.CompletedTask;
        }

        protected override async Task OnModelLoad_Before()
        {
            Debug.LogError("OnModelLoad_Before");
            
            View.ClickedValueProperty.Value = "Not loaded data...";

            await Task.Delay(2 * 1000);
        }
    }
}