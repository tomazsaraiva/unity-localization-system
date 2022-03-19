#region Includes
using System;

using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.LocalizationSystem.Demo
{
    public class LanguageButton : MonoBehaviour
    {
        #region Variables

        public delegate void OnPressed(LanguageButton button, Locale locale);
        public OnPressed Pressed;

        [Header("Inner")]
        [SerializeField] private Text _label;

        private Button _button;
        private Locale _locale;

        #endregion

        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        private void Start()
        {
            _button.onClick.AddListener(Button_OnClick);
        }

        public void SetLocale(LocaleConfig locale)
        {
            _locale = locale.ReferenceType;
            _label.text = locale.Language.Name;
        }

        private void Button_OnClick()
        {
            Pressed?.Invoke(this, _locale);
        }
    }
}