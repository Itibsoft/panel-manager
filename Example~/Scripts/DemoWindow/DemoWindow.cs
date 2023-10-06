using System;
using Itibsoft.PanelManager;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Demo
{
    public class DemoWindow : PanelBase
    {
        [SerializeField] private Button _clickMeButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _releaseButton;

        public event Action OnClick; 
        public event Action OnCloseWindow; 
        public event Action OnReleaseWindow; 

        [UsedImplicitly]
        private void Constructor()
        {
            Debug.Log("DemoWindow.Constructor");
        }
        
        [UsedImplicitly]
        private void OnOpen()
        {
            _clickMeButton.onClick.AddListener(OnClickButtonHandle);
            _closeButton.onClick.AddListener(OnCloseButtonHandle);
            _releaseButton.onClick.AddListener(OnReleaseButtonHandle);
            
            Debug.Log("DemoWindow.OnOpen");
        }

        [UsedImplicitly]
        private void OnClose()
        {
            _clickMeButton.onClick.RemoveListener(OnClickButtonHandle);
            _closeButton.onClick.RemoveListener(OnCloseButtonHandle);
            _releaseButton.onClick.RemoveListener(OnReleaseButtonHandle);
            
            Debug.Log("DemoWindow.OnClose");
        }
        
        private void OnClickButtonHandle()
        {
            Debug.Log("DemoWindow.Click");
            OnClick?.Invoke();
        }

        private void OnCloseButtonHandle()
        {
            Debug.Log("DemoWindow.Close");
            OnCloseWindow?.Invoke();
        }
        
        private void OnReleaseButtonHandle()
        {
            Debug.Log("DemoWindow.Release");
            OnReleaseWindow?.Invoke();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            Debug.Log("DemoWindow.Dispose");
        }
    }
}