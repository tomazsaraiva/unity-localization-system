#region Includes
using System;
using System.Collections.Generic;

using TS.LocalizationSystem;
#endregion

namespace TS.LocalizationSystem.Classes
{
    [Serializable]
    public class Language
    {
        #region Serializables

        public int id;
        public string name;
        public string direction;
        public string language_code;
        public string locale;
        public Country country;
        public long created_at;
        public long updated_at;

        #endregion

        public int Id { get { return id; } }
        public string IetfTag { get { return locale; } }
        public string LanguageCode { get { return language_code; } }
        public LocaleConfig Locale { get { return LocaleSettings.GetLocale(this); } }
    }

    [Serializable]
    public class LanguagesData
    {
        #region Serializables

        public List<Language> data;
        
        #endregion

        public List<Language> Languages { get { return data; } }
    }
}