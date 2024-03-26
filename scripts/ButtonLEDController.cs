using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ardity ressources: https://github.com/dwilches/Ardity 
// https://raw.githubusercontent.com/dwilches/Ardity/master/UnityProject/Ardity%20-%20Setup%20Guide.pdf

// overview of nextmosphere codes: https://nexmosphere.com/document/XN-165%20Quick%20Start%20Guide.pdf

public class ButtonLEDController : MonoBehaviour
{
    [SerializeField] private SerialController serialController;
    void Start()
    {
        serialController.SetTearDownFunction(turnOffLEDs);
    }

    public void turnOnLED(Language language)
    {
        switch (language)
        {
            case Language.da:
                serialController.SendSerialMessage("X001A[3]");
                serialController.SendSerialMessage("X002A[3]");
                break;
            case Language.en:
                serialController.SendSerialMessage("X001A[12]");
                serialController.SendSerialMessage("X002A[12]");
                break;
            case Language.de:
                serialController.SendSerialMessage("X001A[48]");
                serialController.SendSerialMessage("X002A[48]");
                break;
        }
    }

    private void turnOffLEDs() // SEEMS TO BE A BUG --> ONLY FIRST SERIANCONTROLLER FUNCTIONCALL IS EXECUTED BEFIRO TEARDOWN
    {
        print("Executing teardown");
        serialController.SendSerialMessage("X001A[0]");
        serialController.SendSerialMessage("X002A[0]");
    }
}
