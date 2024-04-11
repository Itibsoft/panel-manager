using System.Threading.Tasks;
using Itibsoft.PanelManager.Tests;
using Settings.Model;
using Settings.Presenter;
using Settings.Shared;
using UnityEngine;

namespace Samples.Tests
{
    public class Main : MonoBehaviour
    {
        private ISettingsModel _settings;
        
        private void Start()
        {
            _settings = new SettingsModel();
            
            var mvp = new MVPManager();
            var settingsPresenter = mvp.LoadPanel<SettingsPresenter>(_settings); 

            settingsPresenter.Start();
            settingsPresenter.Open();

            StartIncrementor();
        }

        private async Task StartIncrementor()
        {
            while (true)
            {
                _settings.ClickedCountProperty.Value++;
                
                await Task.Delay(1);
                await Task.Yield();
            }
        }
    }
}