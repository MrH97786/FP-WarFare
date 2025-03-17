using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateSwitch : Interactable
{
    public GateSwitchManager switchManager; // Reference to the manager
    public TextMeshProUGUI promptText; // Reference to the TextMeshProUGUI component
    private bool isActivated = false;
    private bool isUsed = false;

    protected override void Interact()
    {
        if (!isActivated && !isUsed)
        {
            if (ScoreManager.instance.HasEnoughPoints(300))
            {
                ScoreManager.instance.DeductPoints(300); // reduce 300 points

                isActivated = true;
                isUsed = true; // Prevent further use
                switchManager.ActivateSwitch(); // Notify the GateSwitchManager to activate the switch
                
                promptMessage = "Activated"; 
                promptText.text = promptMessage; 
                Debug.Log("Switch activated!");
            }
            else
            {
                Debug.Log("Not enough points to activate the switch.");
            }
        }
    }
}
