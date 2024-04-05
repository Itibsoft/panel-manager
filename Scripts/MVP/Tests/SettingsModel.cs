using UnityEngine;

namespace Itibsoft.PanelManager.Tests.Tests
{
    public class SettingsModel : ModelBase<SettingsData>
    {
        protected override SettingsData FetchData()
        {
            var data = new SettingsData
            {
                CountClicked = 0
            };

            return data;
        }

        protected override void OnDataFetch_Before()
        {
            Debug.Log("SettingsModel.OnDataFetched_Before");
        }

        protected override void OnDataFetched_After(SettingsData data)
        {
            Debug.Log(data.CountClicked);
        }
    }
}