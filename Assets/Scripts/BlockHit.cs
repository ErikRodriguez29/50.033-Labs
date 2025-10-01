using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    // for animation
    public Sprite emptyBlockSprite;
    private bool animating = false;

    public GameObject coin;
    public bool hasCoin;

    // for audio
    public AudioClip hitSound = null;
    public AudioSource audioSource;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D[] contacts = collision.contacts;
            if(Vector3.Dot(contacts[contacts.Length / 2].normal, Vector3.up) > 0.8f)
            {
                GetComponent<SpriteRenderer>().sprite = emptyBlockSprite;
                if (hitSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
                if(hasCoin)
                {
                    Instantiate(coin, transform.position, Quaternion.identity);
                    hasCoin = false;
                }
                StartCoroutine(Animate());
            }
        }
    }

    private IEnumerator Animate()
    {
        animating = true;
        
        Vector3 restingPosition = transform.localPosition;
        Vector3 raisedPosition = restingPosition + Vector3.up / 2f;

        yield return Move(restingPosition, raisedPosition);
        yield return Move(raisedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;
        float duration = 0.125f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localPosition = Vector3.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = end;
    }
}