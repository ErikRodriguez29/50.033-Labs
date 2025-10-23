using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    private Transform player; // Mario's Transform
    public Transform endLimit; // Gameobject that indicates end of map
    private float offset; // initial x-offset between camera and Mario
    private float startX; // smallest x-coordinate of the Camera
    private float endX; // largest x-coordinate of the Camera
    private float viewportHalfWidth;
    private Vector3 startPosition;

    // Start is called before the frist frame update
    void Start()
    {
        startPosition = Camera.main.transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // get coordinate of the bottomleft of the viewport
        // z doesn't matter since the camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);

        offset = this.transform.position.x - player.position.x;
        startX = this.transform.position.x;
        endX = endLimit.transform.position.x - viewportHalfWidth;
    }

    public void RecalculateBounds()
    {
        offset = this.transform.position.x - player.position.x;
        startX = this.transform.position.x;
        endX = endLimit.transform.position.x - viewportHalfWidth;
    }
    
    // Update is called once per frame
    void Update()
    {
        float desiredX = player.position.x + offset;
        //check if desiredX is within startX and endX
        if (desiredX > startX && desiredX < endX)
        {
            this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        }
    }

    

    public void GameRestart()
    {
        // reset camera position
        transform.position = startPosition;
    }
}