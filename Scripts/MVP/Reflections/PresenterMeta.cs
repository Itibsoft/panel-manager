using System;
using JetBrains.Annotations;
using UnityEngine.Scripting;

namespace Itibsoft.PanelManager.Tests.Reflections
{
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class PresenterMeta : PreserveAttribute, IPanelMeta
    {
        public PanelType PanelType { get; set; }
        public Type Model { get; set; }
        public int Order { get; set; }
        public string AssetId { get; set; }
    }
}