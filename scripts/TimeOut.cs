using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TimeOut : MonoBehaviour
{
    [SerializeField] private float timeToReset;

    private void Start()
    {
        if (IsNotFirstScene())
        {
            ResetTimer();// Invokes timedown-method when scene is loaded (new gamemanager for each scene)
        }
    }
    private void Update()
    {

        //load first scene if this is not 1. scene & if there have been no touch-input for x seconds
        if (IsNotFirstScene() && AnyInputDetected())
        {
            ResetTimer();
        }

    }

    private void ResetTimer()
    {
        CancelInvoke("LoadFirstScene"); // prior invoke is cancelled. Otherwise, starts invokes every second
        Invoke("LoadFirstScene", timeToReset); // new invoke is set
    }

    private bool IsNotFirstScene()
    {
        return SceneManager.GetActiveScene().buildIndex != 0;
    }

    private bool AnyInputDetected()
    {
        return Input.touchCount != 0 || Input.anyKeyDown;
    }

    private void LoadFirstScene()
    {
        SceneLoader.Instance.LoadSceneByIndex(0);
    }
}
