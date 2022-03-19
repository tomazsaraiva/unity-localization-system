#region Includes
using System;
#endregion

namespace TS.LocalizationSystem.Classes
{
    [Serializable]
    public class Country
    {
        #region Serializables

        public int id;
        public string name;
        public string iso_code;
        public int numeric_code;

        #endregion
    }
}