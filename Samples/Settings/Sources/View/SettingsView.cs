using Itibsoft.PanelManager;
using Settings.Shared;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.View
{
    public class SettingsView : PanelBase, ISettingsView
    {
        public ReactiveCommand IncrementValueCommand { get; } = new();
        public ReactiveProperty<string> ClickedValueProperty { get; } = new("0");
        
        [SerializeField] private Button _plusButton;
        [SerializeField] private TMP_Text _countClickedText;
        
        private void Awake()
        {
            IncrementValueCommand.BindTo(_plusButton);
            ClickedValueProperty.Subscribe(value => _countClickedText.text = value);
        }
    }
}