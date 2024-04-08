using System.Threading.Tasks;
using Itibsoft.PanelManager;
using Itibsoft.PanelManager.Tests;
using Itibsoft.PanelManager.Tests.Reflections;
using Settings.Shared;
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
            view.OnClickPlus += OnView_ClickPlusHandle;
            model.OnChangedClicked += OnModel_ChangedClickedHandle; 
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

            View.SetCountClicked(Model.Data.CountClicked);

            return Task.CompletedTask;
        }

        protected override async Task OnModelLoad_Before()
        {
            Debug.LogError("OnModelLoad_Before");
            
            View.SetCountClicked(0);

            await Task.Delay(2 * 1000);
        }
        
        private void OnView_ClickPlusHandle()
        {
            Model.PlusClick();
        }
        
        private void OnModel_ChangedClickedHandle()
        {
            View.SetCountClicked(Model.Data.CountClicked);
        }
    }
}