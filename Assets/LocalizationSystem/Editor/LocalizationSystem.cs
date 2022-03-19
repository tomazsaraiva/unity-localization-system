#if UNITY_EDITOR

#region Includes
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TS.LocalizationSystem.Extensions;
using TS.LocalizationSystem.Scriptables;
using TS.LocalizationSystem.Utils;

using UnityEditor;

using UnityEngine;
#endregion

namespace AttentionLab.Editor
{
    [ExecuteInEditMode]
    public class LocalizationSystem : EditorWindow
    {
        [MenuItem("Window/Localization System")]
        static void Open()
        {
            GetWindow<LocalizationSystem>(false, "Localization System", true);
        }

        #region Variables

        private const string LOCALE_PATH = "Editor/localization.csv";
        private const string RESOURCES_PATH = "Resources/Locales";
        private const string RESOURCES_PATH_FORMAT = "Assets/" + RESOURCES_PATH + "/{0}.asset";

        private Vector2 _scrollPos;

        private string _localeFilePath;

        private List<string> _keys;
        private Dictionary<string, List<string>> _languages;

        #endregion

        private void OnEnable()
        {
            _localeFilePath = Path.GetFullPath(Path.Combine(Application.dataPath, LOCALE_PATH));

            // Load localization file
            try
            {
                if (File.Exists(_localeFilePath))
                {
                    ProcessLocalizationFile(_localeFilePath);
                }
            }
            catch (Exception e)
            {
                // die silently
            }
        }
        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            EditorGUILayout.Space();

            if (File.Exists(_localeFilePath))
            {
                if (GUILayout.Button("Process File"))
                {
                    ProcessLocalizationFile(_localeFilePath);
                }

                if (_languages != null)
                {
                    if (_languages.Count > 0)
                    {
                        EditorGUILayout.LabelField("Detected " + _languages.Count + " languages and " + _keys.Count + " keys");
                        foreach (var language in _languages.Keys)
                        {
                            EditorGUILayout.LabelField(language, EditorStyles.miniLabel);
                        }

                        if (GUILayout.Button("Update Resources"))
                        {
                            UpdateLocalizationAssets();
                            UpdateLocalizationConstants();
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("No languages detected in Editor/Tools/localization.csv does not exist.", EditorStyles.helpBox);
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("Editor/Tools/localization.csv does not exist.", EditorStyles.helpBox);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void ProcessLocalizationFile(string filePath)
        {
            List<Dictionary<string, object>> data = CSVReader.Read(filePath);

            if (data.IsNullOrEmpty())
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("No data found in " + filePath);
#endif
                return;
            }

            _keys = new List<string>();
            string[] languages = null;

            for (int i_lines = 0; i_lines < data.Count; i_lines++)
            {
                Dictionary<string, object> fields = data[i_lines];

                // Analyze first line to get number of languages.
                if (i_lines == 0)
                {
                    if (fields.Count < 3)
                    {
                        Debug.LogError("No languages detected");
                        return;
                    }

                    // Keep only the languages.
                    languages = fields.Values.Select(o => o.ToString()).ToArray().Skip(1).ToArray();

                    // Init the language dictionary.
                    _languages = new Dictionary<string, List<string>>();
                    for (int i_lang = 0; i_lang < languages.Length; i_lang++)
                    {
                        _languages.Add(languages[i_lang].ToLower(), new List<string>());
                    }
                    continue;
                }

                //Add the key.
                _keys.Add(fields["Key"].ToString());

                // Keep only the translations.
                string[] values = fields.Values.Select(o => o.ToString()).ToArray().Skip(1).ToArray();

                // Go through each translation and fill in the language dictionary.
                for (int i_lang = 0; i_lang < values.Length; i_lang++)
                {
                    string lang = languages[i_lang];
                    string text = values[i_lang];

                    List<string> texts = _languages[lang.ToLower()];
                    texts.Add(text);
                }
            }

            //foreach (var key in _languages.Keys)
            //{
            //    Debug.Log(key + ", " + _languages[key].Print(true));
            //}
        }

        private void UpdateLocalizationAssets()
        {
            if (_languages == null || _languages.Count == 0) return;

            foreach (var lang in _languages.Keys)
            {
                // Create/Update asset file.
                string path = string.Format(RESOURCES_PATH_FORMAT, lang);

                TextCollection asset = CreateInstance<TextCollection>();

                CreateResourcesPath();
                AssetDatabase.CreateAsset(asset, path);
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();

                // Fill in the file with the translations
                List<string> texts = _languages[lang];
                for (int i = 0; i < texts.Count; i++)
                {
                    asset.Add(_keys[i], texts[i]);
                }

                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        private void UpdateLocalizationConstants()
        {
            if (_keys.IsNullOrEmpty()) return;

            CodeTypeDeclaration targetClass = new CodeTypeDeclaration("LocalizationKeys");
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public;

            CodeNamespace keys = new CodeNamespace("TS.LocalizationSystem");
            keys.Types.Add(targetClass);

            CodeCompileUnit targetUnit = new CodeCompileUnit();
            targetUnit.Namespaces.Add(keys);

            for (int i = 0; i < _keys.Count; i++)
            {
                CodeMemberField key = new CodeMemberField();
                key.Attributes = MemberAttributes.Public | MemberAttributes.Const;
                key.Type = new CodeTypeReference(typeof(System.String));
                key.Name = _keys[i];
                key.InitExpression = new CodePrimitiveExpression(_keys[i]);
                targetClass.Members.Add(key);
            }

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            string path = Path.GetFullPath(Application.dataPath + "/LocalizationKeys.cs");

            using (StreamWriter sourceWriter = new StreamWriter(path))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }

        private void CreateResourcesPath()
        {
            var resourcesPath = Path.GetFullPath(Path.Combine(Application.dataPath, RESOURCES_PATH));
            
            if (Directory.Exists(resourcesPath)) { return; }
            Directory.CreateDirectory(resourcesPath);
        }
    }
}

#endif