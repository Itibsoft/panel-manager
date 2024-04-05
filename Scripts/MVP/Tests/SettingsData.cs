using System.Runtime.Serialization;
using a;

namespace Itibsoft.PanelManager.Tests.Tests
{
    [DataContract]
    public class SettingsData : IData
    {
        [DataMember] public int CountClicked;
    }
}