#region Includes
using System;

using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.LocalizationSystem.Demo
{
    public class LanguageSystemDemo : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private LanguageButton _buttonPrefab;
        [SerializeField] private HorizontalLayoutGroup _layout;
        [SerializeField] private Text _labelHello;
        [SerializeField] private Button _buttonHello;

        #endregion

        private void Start()
        {
            // Necessary to update all the app with the default locale.
            // Or if the locale is being saved as a pref, set the saved one.
            LocaleManager.SetDefaultLocale();

            foreach (var locale in LocaleSettings.Locales.Values)
            {
                var button = Instantiate(_buttonPrefab, _layout.transform);
                button.Pressed = LanguageButton_Pressed;
                button.SetLocale(locale);
            }
            
            _buttonHello.onClick.AddListener(ButtonHello_OnClick);
        }

        private void LanguageButton_Pressed(LanguageButton button, Locale locale)
        {
            LocaleManager.SetLocale(locale);
        }

        private void ButtonHello_OnClick()
        {
            _labelHello.text = LocaleManager.Localize(LocalizationKeys.LABEL_HELLO);
        }
    }
}