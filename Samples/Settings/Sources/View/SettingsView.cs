using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.View
{
    public class SettingsView : MonoBehaviour, IView
    {
        #region fields

        #region public fields

        public event Action OnClickPlus;

        #endregion

        #region serialize fields

        [SerializeField] private Button _plusButton;
        [SerializeField] private TMP_Text _countClickedText;

        #endregion

        #endregion

        #region unity api

        private void Awake()
        {
            _plusButton.onClick.AddListener(OnClickPlusHandle);
        }

        #endregion

        #region public api

        public void SetCountClicked(int count)
        {
            _countClickedText.text = count.ToString();
        }

        #endregion

        #region private api

        private void OnClickPlusHandle()
        {
            OnClickPlus?.Invoke();
        }

        #endregion

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}