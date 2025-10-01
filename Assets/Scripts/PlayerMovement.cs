using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // Physics parameters
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;
    // public float bounceForce = 5;

    // Sprite and Rigidbody parameters
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private Rigidbody2D marioBody;

    // other variables
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;

    // Game over screen controller variables
    public CanvasGroup gameOverScreen;
    public TextMeshProUGUI finalScoreText;

    // for animation
    public Animator marioAnimator;

    //for audio
    public AudioSource marioAudio;

    //for death
    public AudioClip marioDeath;
    public float deathImpulse = 15;

    // state
    [System.NonSerialized]
    public bool alive = true;

    public Transform gameCamera;
    

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();

        gameOverScreen.alpha = 0;
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    public void RestartButtonCallback(int input)
    {
        // Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-3.8f, -2.5f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        jumpOverGoomba.score = 0;

        // hide game over screen
        gameOverScreen.alpha = 0;
        finalScoreText.text = "";

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset velocity
        marioBody.linearVelocity = Vector2.zero;

        // reset audio
        marioAudio.Stop();
        marioAudio.Play();

        // reset camera position
        gameCamera.position = new Vector3(0, 2, -10);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        // if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Enemies") || col.gameObject.CompareTag("Obstacles") && !onGroundState)
        // {
        //     onGroundState = true;
        //     // update animator state
        //     marioAnimator.SetBool("onGround", onGroundState);
        // }
        

        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }

        // // Only bounce blocks when hit from below
        // if (col.gameObject.CompareTag("Block")) 
        // {
        //     col.gameObject.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // make mario die if collided with a goomba or other enemy
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            marioAnimator.Play("mario_die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    void gameOverScene()
    {
        gameOverScreen.alpha = 1;
        finalScoreText.text = "Final Score: " + jumpOverGoomba.score.ToString();
        scoreText.text = "";

        Time.timeScale = 0.0f;
    }


    void playJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if(alive)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
            }

            // stop
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
                // stop
                marioBody.linearVelocity = Vector2.zero;
            }

            // other instructions
            if (Input.GetKeyDown("space") && onGroundState)
            {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
        
    }
}