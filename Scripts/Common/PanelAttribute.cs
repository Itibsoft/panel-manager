using System;
using Project.Scripts.PanelManager.Enum;

namespace Assets.Project.Scripts.UI.Common
{
    public class PanelAttribute : Attribute
    {
        public PanelType PanelType;
        public string AssetId;
    }
}