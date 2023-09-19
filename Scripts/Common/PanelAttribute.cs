using System;
using JetBrains.Annotations;
using UnityEngine.Scripting;

namespace Itibsoft.PanelManager
{
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PanelAttribute : PreserveAttribute
    {
        public PanelType PanelType;
        public string AssetId;
    }
}