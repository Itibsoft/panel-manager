using System.Threading.Tasks;
using Itibsoft.PanelManager;
using Itibsoft.PanelManager.Tests;
using Itibsoft.PanelManager.Tests.Reflections;
using UnityEngine;

namespace Samples.Tests
{
    [PresenterMeta(
        PanelType = PanelType.Window, 
        Order = 0, 
        AssetId = "panels/overlays/settings_view"
    )]
    public class SettingsPresenter : PresenterBase<ISettingsModel, ISettingsView>
    {
        public SettingsPresenter(ISettingsModel model, ISettingsView view) : base(model, view)
        {
            view.OnClickPlus += OnView_ClickPlusHandle;
            model.OnChangedClicked += OnModel_ChangedClickedHandle;
        }

        protected override void OnViewOpened_After() { }
        protected override void OnViewOpen_Before() { }

        protected override async Task OnModelLoaded_After()
        {
            Debug.LogError("OnModelLoaded_After");

            await Task.Delay(2 * 1000);
            
            View.SetCountClicked(Model.Data.CountClicked);
        }

        protected override async Task OnModelLoad_Before()
        {
            Debug.LogError("OnModelLoad_Before");
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