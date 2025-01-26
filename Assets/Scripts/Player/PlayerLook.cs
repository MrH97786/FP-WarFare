using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera camera;
    private float rotationX = 0f;

    public float sensitivityX = 29f;
    public float sensitivityY = 29f;
    
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //calculating camera rotation so player can look up and down
        rotationX -= (mouseY * Time.deltaTime) * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, -75f, 75f);
        camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // applying to camera transform
        
        //calculating camera rotation so player can look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * sensitivityX);
    }

}
