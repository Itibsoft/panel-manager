using System;
using System.Threading.Tasks;
using Itibsoft.MVP;
using JetBrains.Annotations;
using Settings.Shared;
using UniRx;
using UnityEngine;

namespace Settings.Model
{
    [UsedImplicitly]
    public class SettingsModel : ISettingsModel
    {
        public ReactiveProperty<int> ClickedCountProperty { get; } = new (0);
        
        public SettingsData Data { get; private set; }

        public SettingsModel()
        {
            ClickedCountProperty
                .Where(_ => Data != default)
                .Subscribe(value =>
                {
                    Data.CountClicked = value;
                    
                    var json = JsonUtility.ToJson(Data);
                    PlayerPrefs.SetString("settings", json);
                });
        }
        
        public Task<IData> FetchData()
        {
            var json = PlayerPrefs.GetString("settings");
            
            Data = JsonUtility.FromJson<SettingsData>(json) ?? new SettingsData();

            ClickedCountProperty.Value = Data.CountClicked;

            return Task.FromResult((IData)Data);
        }
        
        public Task OnDataFetch_Before()
        {
            return Task.CompletedTask;
        }

        public Task OnDataFetched_After(IData data)
        {
            var settingsData = data as SettingsData;
            Debug.Log(settingsData!.CountClicked);
            
            return Task.CompletedTask;
        }
    }
}