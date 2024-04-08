#if ADDRESSABLES

using UnityEngine.AddressableAssets;

namespace Itibsoft.PanelManager.External
{
    public class AddressablesPanelFactory : IPanelFactory
    {
        public IViewMono Create(IPanelMeta meta)
        {
            var panel = Addressables
                .InstantiateAsync(meta.AssetId)
                .WaitForCompletion()
                .GetComponent<PanelBase>();

            return panel;
        }
    }
}

#endif