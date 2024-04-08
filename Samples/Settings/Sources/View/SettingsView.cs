using System;
using Itibsoft.PanelManager;
using Settings.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.View
{
    public class SettingsView : PanelBase, ISettingsView
    {
        public event Action OnClickPlus;
        
        [SerializeField] private Button _plusButton;
        [SerializeField] private TMP_Text _countClickedText;
        
        private void Awake()
        {
            _plusButton.onClick.AddListener(OnClickPlusHandle);
        }
        
        public void SetCountClicked(int count)
        {
            _countClickedText.text = count.ToString();
        }
        
        private void OnClickPlusHandle()
        {
            OnClickPlus?.Invoke();
        }
        
    }
}