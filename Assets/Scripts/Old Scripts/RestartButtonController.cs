using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// later on, teach interface
// public class RestartButtonController : MonoBehaviour, IInteractiveButton
public class RestartButtonController : MonoBehaviour
{
    public UnityEvent gameRestart;

    public void ButtonClick()
    {
        gameRestart.Invoke();
    }
}