using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameConstants gameConstants;
    private float originalX;
    
    float maxOffset;
    float enemyMoveSpeed; 
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;

    void Start()
    {
        maxOffset = gameConstants.goombaMaxOffset;
        enemyMoveSpeed = gameConstants.enemyMoveSpeed;
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * enemyMoveSpeed, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

      void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.name);
    }

    public void GameRestart()
    {
        transform.localPosition = new Vector3(originalX, transform.position.y, transform.position.z);
        moveRight = -1;
        ComputeVelocity();
    }

    // note that this is Update(), which still works but not ideal. See below.
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {
            // move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }
}