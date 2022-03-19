# Localization System for Unity Projects

## Add localization.csv file

1.  Create a localization.csv file to store the languages and strings that need translation;

|                |                 |                 |
| -------------- | --------------- | --------------- |
| Key            | en              | pt              |
| LABEL_LANGUAGE | Select language | Escolher idioma |
| LABEL_HELLO    | Hey Guys        | Olá Pessoal     |
| BUTTON_HELLO   | Say hello       | Dizer olá       |

2.  Place the localization.csv file in the Editor directory on the project root folder  `Assets/Editor/`

2. Open the Localization System from `Window/Localization System`;
	1. It processes the localization.csv file automatically each time the window is opened;
	2. If you change the file while the window is open, press `Process File`;
	3. Press `Update Resources` to create the language asset files and generate the set of constants.

## Define locale settings
    
1.  Add the supported locales to the `Locale` enum in the `LocalConfig.cs`.
    
```csharp
/// <summary>
/// All the locales supported by the application.
/// https://www.science.co.il/language/Locale-codes.php
/// </summary>
public enum Locale
{
    NotDefined,
    en_US,
    pt_PT
}
```

2.  Add the supported languages and language codes to the `Language` and `LanguageCode` enums in the `LanguageConfig.cs`.

```csharp
/// <summary>
/// All the languages available in the application.
/// </summary>
public enum Language
{
    English = 0,
    Portuguese = 1
}

/// <summary>
/// Codes for all languages avaiable in the application.
/// </summary>
public enum LanguageCode
{
    en = 0,
    pt = 1
}
```

3.  Configure both the Languages and Locales in the `LocaleSettings.cs`.

```csharp
/// <summary>
/// List of application supported languages.
/// </summary>
private static readonly Dictionary<Language, LanguageConfig> Languages = new Dictionary<Language, LanguageConfig>()
{
    {
        Language.English, new LanguageConfig()
        {
            InternalCode = LanguageCode.en,
            ReferenceType = Language.English,
            Name = "English",
            ReadingDirection = LanguageReadingDirection.LeftToRight
        }
    },
    {
        Language.Portuguese, new LanguageConfig()
        {
            InternalCode = LanguageCode.pt,
            ReferenceType = Language.Portuguese,
            Name = "Portuguese",
            ReadingDirection = LanguageReadingDirection.LeftToRight
        }
    }
};
```

```csharp
/// <summary>
/// List of application supported locales.
/// </summary>
public static readonly Dictionary<Locale, LocaleConfig> Locales = new Dictionary<Locale, LocaleConfig>()
{
    {
        Locale.en_US, new LocaleConfig()
        {
            //Name = "English - United States",
            Name = "English",
            ReferenceType = Locale.en_US,
            Language = Languages[Language.English],
            SystemLanguages = new SystemLanguage[]
            {
                SystemLanguage.English
            },
            DateFormat = "dd-MM-yyyy",
            DateSeparator = "/",
            DateEndianness = LocaleConfig.EndiannessType.Little
        }
    },
    {
        Locale.pt_PT, new LocaleConfig()
        {
            //Name = "Portuguese - Portugal",
            Name = "Português",
            ReferenceType = Locale.pt_PT,
            Language = Languages[Language.Portuguese],
            SystemLanguages = new SystemLanguage[]
            {
                SystemLanguage.Portuguese
            },
            DateFormat = "dd-MM-yyyy",
            DateSeparator = "-",
            DateEndianness = LocaleConfig.EndiannessType.Little
        }
    }
};
```

4. Define the default locale in the `LocaleSettings.cs`.
```csharp
public static LocaleConfig GetDefault()
{
	return Locales[Locale.en_US];
}
```

## Load the default locale

Load the default locale at app start. You can also listen to the `LocaleChanged` event to get updates when the locale changes.

```csharp
private void Start()
{
	// Necessary to update all the app with the default locale.
	// Or if the locale is being saved as a pref, set the saved one.
	LocaleManager.LocaleChanged += LocaleManager_LocaleChanged;
	LocaleManager.SetDefaultLocale();
}

private void LocaleManager_LocaleChanged(LocaleConfig current, int updateNumber)
{
	// locale changed
}
```

## Localize UI Text Components

The `LocalizedLabel.cs` component updates automatically the label text with the localized text when the app starts and each time the app locale is changed at runtime.

1. Add a `LocalizedLabel.cs` component to all Text Components that use localized texts and assign the correct keys.

## Use localized strings

When setting a localized string in code, call the `LocaleManager.Localize` method and pass the correct key. The method returns the value according to the selected locale.

```csharp
var localizedString = LocaleManager.Localize(LocalizationKeys.LABEL_HELLO);
```