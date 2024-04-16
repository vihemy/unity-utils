using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectTextExtractor
{
    [MenuItem("Tools/Extract All Texts from ScriptableObjects")]
    public static void ExtractTextsFromScriptableObjects()
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("Path;Field Content");

        List<Type> types = new List<Type> { typeof(UIData), typeof(IntroData), typeof(ResultData) }; // Ensure all types are included

        foreach (var type in types)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{type.Name}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (so != null)
                {
                    // Use the name of the ScriptableObject as the base path
                    ExtractTextFromObject(so, csv, type, so.name); // Using ScriptableObject's name as the base path
                }
            }
        }

        var utf8WithBom = new UTF8Encoding(true);
        File.WriteAllText("Assets/ScriptableObjectTexts.csv", csv.ToString(), utf8WithBom);
        AssetDatabase.Refresh();
    }

    private static void ExtractTextFromObject(object obj, StringBuilder csv, Type type, string path)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            // Build the path incrementally to reflect hierarchy
            string currentPath = $"{path}.{field.Name}";
            if (field.FieldType == typeof(string))
            {
                string text = (string)field.GetValue(obj);
                if (!string.IsNullOrEmpty(text))
                {
                    // Append line with path and field content
                    AppendCsvLine(csv, currentPath, text);
                }
            }
            else if (field.FieldType.IsArray && field.FieldType.GetElementType().IsClass)
            {
                var array = field.GetValue(obj) as Array;
                if (array != null)
                {
                    foreach (var element in array)
                    {
                        ExtractTextFromObject(element, csv, field.FieldType.GetElementType(), currentPath);
                    }
                }
            }
            else if (field.FieldType.IsClass)
            {
                var nestedObject = field.GetValue(obj);
                if (nestedObject != null)
                {
                    ExtractTextFromObject(nestedObject, csv, field.FieldType, currentPath);
                }
            }
        }
    }

    private static void AppendCsvLine(StringBuilder csv, string path, string text)
    {
        // Sanitize text to ensure CSV integrity
        string sanitizedText = text.Replace(";", ",").Replace("\n", " ").Replace("\r", " ");
        // Format line to include only path and text content
        csv.AppendLine($"{path};{sanitizedText}");
    }
}
