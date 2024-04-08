using System.Runtime.Serialization;
using a;

namespace Samples.Tests
{
    [DataContract]
    public class SettingsData : IData
    {
        [DataMember] public int CountClicked;
    }
}