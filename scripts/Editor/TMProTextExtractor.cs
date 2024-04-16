using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TMProTextExtractor
{
    [MenuItem("Tools/Extract All Texts from TMPro-components")]
    private static void ExtractTexts()
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("Scene Name;GameObject Name;Text Content");  // Header row for CSV with semicolon delimiter

        // Store the current active scene to return to it after processing
        Scene currentActiveScene = EditorSceneManager.GetActiveScene();

        // Iterate through all scenes in the build settings
        foreach (EditorBuildSettingsScene ebsScene in EditorBuildSettings.scenes)
        {
            if (ebsScene.enabled)
            {
                // Load the scene additively
                Scene scene = EditorSceneManager.OpenScene(ebsScene.path, OpenSceneMode.Additive);

                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (var root in rootObjects)
                {
                    ExtractFromGameObject(root, csv, scene.name);  // Pass scene name to function
                }

                // Close the scene if it's not the original active scene
                if (scene.path != currentActiveScene.path)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }

        // Write to file safely with UTF-8 encoding including BOM
        try
        {
            // Ensure UTF-8 encoding with BOM
            var utf8WithBom = new UTF8Encoding(true);
            File.WriteAllText("Assets/TMProTexts.csv", csv.ToString(), utf8WithBom);
            AssetDatabase.Refresh();
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to write file: " + e.Message);
        }

        // Re-open the original scene if it was closed
        if (!currentActiveScene.isLoaded)
        {
            EditorSceneManager.OpenScene(currentActiveScene.path);
        }
    }

    private static void ExtractFromGameObject(GameObject gameObject, StringBuilder csv, string sceneName)
    {
        TextMeshProUGUI[] texts = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI text in texts)
        {
            // Ensure text content is CSV-safe by replacing semicolons and newlines
            string sanitizedText = text.text.Replace(";", ",").Replace("\n", " ").Replace("\r", " ");
            // Use text.gameObject.name to get the name of the GameObject that has the TextMeshPro component
            csv.AppendLine($"{sceneName};{text.gameObject.name};{sanitizedText}");
        }
    }
}
