#region Includes
using System;
using System.Collections.Generic;

using UnityEngine;
#endregion

namespace TS.LocalizationSystem.Scriptables
{
    [Serializable]
    public class TextCollection : ScriptableObject
    {
        #region Variables

        // Not pretty but dictionaries are not serialized
        [SerializeField] private List<string> _keys;
        [SerializeField] private List<string> _values;

        #endregion

        public string Localize(string key)
        {
            if (_keys == null || _keys.Count == 0) return null;
            if (!_keys.Contains(key))
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning(key + " not found");
#endif
                return null;
            }
            return _values[_keys.IndexOf(key)];
        }

#if UNITY_EDITOR

        public void Add(string key, string value)
        {
            if (_keys == null)
            {
                _keys = new List<string>();
            }

            if (_keys.Contains(key))
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning(key + " already exists with value " + value);
#endif
                return;
            }

            _keys.Add(key);

            if (_values == null)
            {
                _values = new List<string>();
            }
            _values.Add(value);
        }
#endif
    }
}