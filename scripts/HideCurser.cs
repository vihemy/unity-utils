using UnityEngine;

public class HideCurser : MonoBehaviour
{
    [SerializeField] private bool showCurserInEditor = false;
    [SerializeField] private bool showCurserInBuild = false;
    private void Awake()
    {
        if (Application.isEditor)
        {
            // Code to run when the game is running in the editor
            Cursor.visible = showCurserInEditor;
        }
        else
        {
            // Code to run when the game is running as an autonomous build
            Cursor.visible = showCurserInBuild;
        }
    }
}
