using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class GateSwitchManager : MonoBehaviour
{
    public GameObject gateToHide;  // Assign the gate to be hidden in the Inspector
    public int switchesActivated = 0; // Counter for activated switches

    public void ActivateSwitch()
    {
        switchesActivated++;

        if (switchesActivated >= 3)  // Check if all 3 switches are activated
        {
            gateToHide.SetActive(false);  // Hide the gate
            Debug.Log("All switches activated! Gate is now hidden.");
        }
    }
}

