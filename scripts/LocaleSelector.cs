using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class LocaleSelector : MonoBehaviour
// inspiration: https://www.youtube.com/watch?v=qcXuvd7qSxg 

{
    // Start is called before the first frame update
    private bool active = false;
    public void ChangeLocale(int localeID)
    {
        if(active==true) // prevents against unwanted multiple calls
            return;
        StartCoroutine(SetLocale(localeID));
    }
    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation; // makes sure LocalizationSettings is initialized
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false;
    }
}
