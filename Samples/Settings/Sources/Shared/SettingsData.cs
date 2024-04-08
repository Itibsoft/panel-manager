using System.Runtime.Serialization;
using Itibsoft.MVP;

namespace Settings.Shared
{
    [DataContract]
    public class SettingsData : IData
    {
        [DataMember] public int CountClicked;
    }
}