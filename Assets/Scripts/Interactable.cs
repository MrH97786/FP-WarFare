using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;
    [SerializeField] public string promptMessage;

    //BaseInteract function to be called from the player script
    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
    }
    
    protected virtual void Interact()
    {
        //function to be overridden 
    }
}
