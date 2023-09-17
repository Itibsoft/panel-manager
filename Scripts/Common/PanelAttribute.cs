using System;

namespace Itibsoft.PanelManager
{
    public class PanelAttribute : Attribute
    {
        public PanelType PanelType;
        public string AssetId;
    }
}