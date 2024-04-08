using System;
using System.Threading.Tasks;
using a;
using Itibsoft.PanelManager.Tests;
using JetBrains.Annotations;
using UnityEngine;

namespace Samples.Tests
{
    [UsedImplicitly]
    public class SettingsModel : ISettingsModel
    {
        public event Action OnChangedClicked;
        
        public SettingsData Data { get; private set; }
        
        public Task<IData> FetchData()
        {
            var json = PlayerPrefs.GetString("settings");
            
            Data = JsonUtility.FromJson<SettingsData>(json) ?? new SettingsData();

            return Task.FromResult((IData)Data);
        }
        
        public Task OnDataFetch_Before()
        {
            return Task.CompletedTask;
        }

        public Task OnDataFetched_After(IData data)
        {
            var settingsData = data as SettingsData;
            Debug.Log(settingsData.CountClicked);
            
            return Task.CompletedTask;
        }
     
        public void PlusClick()
        {
            Data.CountClicked++;

            var json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString("settings", json);
            
            OnChangedClicked?.Invoke();
        }
    }
}