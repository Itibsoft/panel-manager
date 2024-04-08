using System;
using System.Threading.Tasks;
using Itibsoft.PanelManager.Tests;
using UnityEngine;

namespace Samples.Tests
{
    public class Main : MonoBehaviour 
    {
        private void Start()
        {
            var mvp = new MVPManager();
            var settingsPresenter = mvp.LoadPanel<SettingsPresenter, SettingsModel>();

            _ = settingsPresenter.Start();
            settingsPresenter.Open();
        }
    }
}