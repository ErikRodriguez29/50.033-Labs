using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlockController : MonoBehaviour
{
    public Animator coinAnimator; // Reference to the coin's animator
    public Animator brickAnimator; // Reference to the brick's animator
    public GameObject block;
    public int maxCoins = 1; // How many coins this block contains
    private int coinsSpawned = 0;
    private bool isEmpty = false;
    private Sprite previousBlockSprite;
    private int prevMaxCoins;

    public AudioSource CoinSound;

    void Start()
    {
        previousBlockSprite = block.GetComponent<SpriteRenderer>().sprite; 
    }

    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isEmpty)
        {
            // Spawn coin and increase score
            brickAnimator.SetTrigger("bounce");
            SpawnCoin();
            
            // Increment coins spawned
            coinsSpawned++;
            
            // Check if block is now empty
            if (coinsSpawned >= maxCoins)
            {
                isEmpty = true;
                // Show disabled/empty sprite
                brickAnimator.SetTrigger("empty");
            }
        }
    }

    private void SpawnCoin()
    {
        // Trigger coin spawn animation
        coinAnimator.SetTrigger("bounce");
        
        // Increase score through GameManager
        GameManager.instance.IncreaseScore(1);
        
        // Optional: Play coin sound here
        CoinSound.Play();
    }

    // Called by animator when block becomes empty
    public void Disable()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void GameRestart()
    {
        isEmpty = false;
        block.GetComponent<SpriteRenderer>().sprite = previousBlockSprite;
        coinsSpawned = 0;
    }
}