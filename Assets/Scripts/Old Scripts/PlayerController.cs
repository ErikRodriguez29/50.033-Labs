using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // Physics parameters
    public GameConstants gameConstants;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;
    private bool onGroundState = true;

    // Sprite and Rigidbody parameters
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private Rigidbody2D marioBody;

    // other variables
    public GameObject enemies;

    // public Transform gameCamera;

    // for animation
    public Animator marioAnimator;

    //for audio
    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;

    


    // state
    [System.NonSerialized]
    public bool alive = true;


    public MarioActions marioActions;

    // public MarioStateController marioStateController;
    // public InputAction jumpHoldAction;

    void Start()
    {
        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);

    }

    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "World-1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(-3.73f, -2.5f, 0);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    private bool moving = false;
    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }



    // reset logic



    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-3.8f, -2.5f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        // reset audio
        marioAudio.Stop();
        marioAudio.Play();
        // reset velocity
        marioBody.linearVelocity = Vector2.zero;
    }


    // collision logic


    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // public GameEvent<Object> onPlayerDeath;
    public UnityEvent onGameOver;
    public void Die()
    {
        if (!alive) return;
        alive = false;
        // play death sound
        marioDeathAudio.Play();
        // stop other audio
        marioAudio.Stop();
        // play death animation
        marioAnimator.SetTrigger("onDeath");
        // apply death impulse
        PlayDeathImpulse();
        // notify other systems
        // onPlayerDeath.Raise(this);
        // invoke game over event
        StartCoroutine(WaitForInvokeGameOver());
    }

    IEnumerator WaitForInvokeGameOver()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        onGameOver.Invoke();
    }

    public GameEvent<Object> onDamagePlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onDamagePlayer.Raise(this);
        }
    }

    public void DamageMario()
    {
        // GameOverAnimationStart(); // last time Mario dies right away

        // pass this to StateController to see if Mario should start game over
        // since both state StateController and MarioStateController are on the same gameobject, it's ok to cross-refer between scripts
        // GetComponent<MarioStateController>().SetPowerup(PowerupType.Damage);
        MarioStateController stateController = GetComponent<MarioStateController>();
        if (stateController.currentPowerupType == PowerupType.Default)
        {
            Die();
        }
        else
        {
            stateController.SetPowerup(PowerupType.Damage);
        }
    }

    public void requestPowerUpEffect()
    {

    }

    // player actions



    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    private bool jumpedState = false;
    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    
    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }



    // animations & sound



    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");
        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    void playJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }
}