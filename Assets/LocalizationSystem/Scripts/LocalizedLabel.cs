#if UNITY_EDITOR
using System.Reflection;

using UnityEditor;
#endif

#region Includes
using UnityEngine;
using UnityEngine.UI;

using TS.LocalizationSystem.Extensions;

using System.Linq;
#endregion

namespace TS.LocalizationSystem
{
    [RequireComponent(typeof(Text)), ExecuteInEditMode]
    public class LocalizedLabel : MonoBehaviour
    {
        #region Variables

        [Header("Configuration")]
        [SerializeField] private bool _useCaps;

        [SerializeField, HideInInspector] private string _localizationKey;

        public delegate void OnTextUpdated(LocalizedLabel sender, string text);
        public event OnTextUpdated TextUpdated;

        private Text _label;
        private int _updateNumber;

        #endregion

        private void Awake()
        {
            _label = GetComponent<Text>();
            if (_label == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("No Text Component found");
#endif
            }
        }

        private void OnEnable()
        {
            LocaleManager.LocaleChanged += LocaleManager_LocaleChanged;

            if (_updateNumber != LocaleManager.UpdateNumber)
            {
                _updateNumber = LocaleManager.UpdateNumber;

                UpdateLabel();
            }
        }
        private void OnDisable()
        {
            LocaleManager.LocaleChanged -= LocaleManager_LocaleChanged;
        }

        private void LocaleManager_LocaleChanged(LocaleConfig current, int updateNumber)
        {
            _updateNumber = updateNumber;

            UpdateLabel();
        }

        public void UpdateLabel()
        {
            string label = LocaleManager.Localize(_localizationKey);

            if (!string.IsNullOrEmpty(label) && _useCaps)
            {
                label = label.ToUpper();
            }

            _label.text = label;

            TextUpdated?.Invoke(this, _label.text);
        }
        public void ClearLabel()
        {
            _label.text = null;
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(LocalizedLabel))]
        public class LocalizedLabelInspector : Editor
        {
            #region Variables

            private LocalizedLabel _target;
            private SerializedProperty _localizationKey;

            private Vector2 _scrollVector;

            private FieldInfo[] _keys;
            private string _searchKeywords;

            #endregion

            private void OnEnable()
            {
                _target = (LocalizedLabel)target;
                _localizationKey = serializedObject.FindProperty("_localizationKey");
                _keys = GetLocalizationKeys();
            }
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

                if (GUILayout.Button("Update Keys"))
                {
                    _keys = GetLocalizationKeys();
                }

                if (!_keys.IsNullOrEmpty())
                {
                    if (!string.IsNullOrEmpty(_localizationKey.stringValue))
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Selected Key", EditorStyles.boldLabel);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(_localizationKey.stringValue);
                        if (GUILayout.Button("Clear"))
                        {
                            _localizationKey.stringValue = null;
                            serializedObject.ApplyModifiedProperties();

                            _target.ClearLabel();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.LabelField(LocaleManager.Localize(_localizationKey.stringValue), EditorStyles.miniLabel);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Search", EditorStyles.boldLabel);
                    _searchKeywords = EditorGUILayout.TextField(_searchKeywords);
                    if (!string.IsNullOrEmpty(_searchKeywords))
                    {
                        FieldInfo[] fields = _keys.Where(k => k.Name.Contains(_searchKeywords)).ToArray();
                        if (!fields.IsNullOrEmpty())
                        {
                            _scrollVector = EditorGUILayout.BeginScrollView(_scrollVector, EditorStyles.inspectorDefaultMargins);

                            for (int i = 0; i < fields.Length; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(fields[i].Name);

                                if (GUILayout.Button("Select"))
                                {
                                    _localizationKey.stringValue = fields[i].Name;
                                    serializedObject.ApplyModifiedProperties();

                                    _target.UpdateLabel();
                                }

                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.LabelField(LocaleManager.Localize(fields[i].Name), EditorStyles.miniLabel);
                            }

                            EditorGUILayout.EndScrollView();
                        }
                        else
                        {
                            EditorGUILayout.LabelField("No keys found", EditorStyles.miniLabel);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No keys found in LocalizationKeys.cs", EditorStyles.helpBox);
                }
            }

            private FieldInfo[] GetLocalizationKeys()
            {
                FieldInfo[] fields = typeof(LocalizationKeys).GetFields();
                if (fields.IsNullOrEmpty())
                {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogWarning("Keys not found");
#endif
                    return null;
                }

                return fields;
            }
        }
#endif
    }
}