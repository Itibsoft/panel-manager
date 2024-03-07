using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Itibsoft.PanelManager.Sample.Demo
{
    public class DemoWindow : PanelBase
    {
        public event Action OnOverlayOpen;
        
        [SerializeField] private Button _openOverlayButton;

        [UsedImplicitly]
        private void OnOpen()
        {
            _openOverlayButton.onClick.AddListener(OnOverlayOpenHandle);
        }
        
        [UsedImplicitly]
        private void OnClose()
        {
            _openOverlayButton.onClick.RemoveListener(OnOverlayOpenHandle);
        }
        
        private void OnOverlayOpenHandle()
        {
            OnOverlayOpen?.Invoke();
        }
    }
}