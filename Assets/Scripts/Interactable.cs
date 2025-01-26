using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;

    //BaseInteract function to be called from the player script
    public void BaseInteract()
    {
        Interact();
    }
    
    protected virtual void Interact()
    {
        //function to be overridden 
    }
}
