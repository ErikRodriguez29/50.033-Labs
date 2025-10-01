using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    // for audio
    public AudioClip coinSound;
    public AudioSource audioSource;
    
    private void Start()
    {
        StartCoroutine(Animate());
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    private IEnumerator Animate()
    {
        Vector3 restingPosition = transform.localPosition;
        Vector3 raisedPosition = restingPosition + Vector3.up * 2f;

        yield return Move(restingPosition, raisedPosition);
        yield return Move(raisedPosition, restingPosition);

        Destroy(gameObject);
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
