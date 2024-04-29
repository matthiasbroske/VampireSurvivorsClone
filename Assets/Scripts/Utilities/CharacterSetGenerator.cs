#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Vampire
{
    public class CharacterSetGenerator : MonoBehaviour
    {
        [SerializeField] private string _characterSetFilePath = "Assets/Fonts/CharacterSet.txt";
        [SerializeField, TextArea] private string _defaultCharacters = "1234567890-=！@#￥%……&*()~:\"{}[]|\\?/<>,.;'+abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ【】「」。：，";
        [SerializeField] private StringTableCollection[] _stringTables;
        
        void Start()
        {
            StringBuilder stringBuilder = new StringBuilder();
            // Add default characters
            stringBuilder.Append(_defaultCharacters);
            // Add every character in every string table
            foreach (var table in _stringTables)
            {
                for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
                {
                    var locale = LocalizationSettings.AvailableLocales.Locales[i].Identifier;
                    stringBuilder.Append(table.GenerateCharacterSet(locale));
                }
            }
            // Save it to character set
            File.WriteAllText(_characterSetFilePath, stringBuilder.ToString());
            Debug.Log("Character Set Generated!");
        }
    }
}

#endif
