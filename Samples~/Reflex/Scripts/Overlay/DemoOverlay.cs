using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Itibsoft.PanelManager.Sample.Demo
{
    public class DemoOverlay : PanelBase
    {
        public event Action OnCloseOverlay;
        
        [SerializeField] private Button _closeButton;

        [UsedImplicitly]
        private void OnOpen()
        {
            _closeButton.onClick.AddListener(OnCloseHandle);
        }
        
        [UsedImplicitly]
        private void OnClose()
        {
            _closeButton.onClick.RemoveListener(OnCloseHandle);
        }
        
        private void OnCloseHandle()
        {
            OnCloseOverlay?.Invoke();
        }
    }
}