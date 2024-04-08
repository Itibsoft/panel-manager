using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Itibsoft.PanelManager
{
    public class ResourcesPanelFactory : IPanelFactory
    {
        public IViewMono Create(IPanelMeta meta)
        {
            var panelPrefab = Resources.Load<PanelBase>(meta.AssetId);

            if (panelPrefab == default)
            {
                throw new Exception($"Not found asset in Resources for path: {meta.AssetId}");
            }

            var panel = Object.Instantiate(panelPrefab);

            return panel;
        }
    }
}