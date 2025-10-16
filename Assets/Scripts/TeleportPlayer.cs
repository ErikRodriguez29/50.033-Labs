using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TeleportPlayer : MonoBehaviour
{
    public Vector3 playerDestination;
    public float cameraXDestination;
    public float cameraYDestination;
    public Transform newEndLimit;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Teleport!");
            other.transform.position = playerDestination;
            Camera.main.transform.position  = new Vector3(cameraXDestination, cameraYDestination, Camera.main.transform.position.z);
            
            var cameraController = Camera.main.GetComponent<CameraController>();

            cameraController.endLimit = newEndLimit;
            cameraController.RecalculateBounds();
        }
    }
}