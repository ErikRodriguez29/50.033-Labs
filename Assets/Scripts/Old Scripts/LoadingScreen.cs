// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class LoadingScreen : MonoBehaviour
// {
//     public CanvasGroup c;

//     public IntVariable gameScore;

//     void Start()
//     {
//         Debug.Log("Begin");
//         StartCoroutine(Fade());
//     }

//     IEnumerator Fade()
//     {
//         for(float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f)
//         {
//             c.alpha = alpha;
//             yield return new WaitForSecondsRealTime(0.1f);
//         }

//         gameScore.Value = 0;
//         SceneManager.LoadSceneAsync("World-1-1", LoadSceneMode.Single);
//     }

//     public void ReturnToMain()
//     {
//         SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
//     }
// }