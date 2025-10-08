using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BlockHit : MonoBehaviour
{
    // for animation
    public Sprite emptyBlockSprite;
    private bool animating = false;

    // for coin
    public GameObject coin;
    public bool hasCoin;
    public AudioClip coinSound;

    // for audio
    public AudioClip hitSound;
    public AudioSource audioSource;

    public GameManager gameManager;

    private SpriteRenderer coinSpriteRenderer;
    private Color color;

    Vector3 boxSize = new Vector3(0.25f, 0.25f, 0.0f);

    private GameObject block;

    private bool hadCoin;
    
    Sprite previousBlockSprite;


    void Start()
    {
        block = this.gameObject;
        hadCoin = hasCoin;
        previousBlockSprite = block.GetComponent<SpriteRenderer>().sprite;
        // if(coin.GetComponent<SpriteRenderer>() != null)
        if(hasCoin)
        {
            coinSpriteRenderer = coin.GetComponent<SpriteRenderer>();
            coinSpriteRenderer.enabled = false;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D[] contacts = collision.contacts;
            if(Vector3.Dot(contacts[contacts.Length / 2].normal, Vector3.up) > 0.8f)
            {
                block.GetComponent<SpriteRenderer>().sprite = emptyBlockSprite;
                if (hitSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
                StartCoroutine(AnimateBlock());
                if(hasCoin)
                {
                    audioSource.PlayOneShot(coinSound);
                    StartCoroutine(AnimateCoin());
                }
            }
        }
    }

    private IEnumerator AnimateBlock()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 raisedPosition = restingPosition + Vector3.up / 2f;

        yield return Move(block, restingPosition, raisedPosition);
        yield return Move(block, raisedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator AnimateCoin()
    {
        coinSpriteRenderer.enabled = true;
        gameManager.IncreaseScore(1);
        
        Vector3 restingPosition = transform.localPosition;
        Vector3 raisedPosition = restingPosition + Vector3.up * 2f;

        yield return Move(coin, restingPosition, raisedPosition);
        yield return Move(coin, raisedPosition, restingPosition);

        coinSpriteRenderer.enabled = false;
        hasCoin = false;
    }

    private IEnumerator Move(GameObject obj, Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;
        float duration = 0.125f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            obj.transform.localPosition = Vector3.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.localPosition = end;
    }

    public void gameRestart()
    {
        hasCoin = hadCoin;
        block.GetComponent<SpriteRenderer>().sprite = previousBlockSprite;
        if(hasCoin)
        {
            coinSpriteRenderer.enabled = false;
        }
    }

    // void Update()
    // {
    //     if(!animating)
    //     {
    //         color.a = 0f;
    //     }
    //     else
    //     {
    //         color.a = 1f;
    //     }
    // }
}