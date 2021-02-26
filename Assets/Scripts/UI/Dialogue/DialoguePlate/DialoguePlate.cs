using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialoguePlate : MonoBehaviour
{
    public event UnityAction DialoguePlateMouseDown;

    private void OnMouseDown()
    {
        DialoguePlateMouseDown?.Invoke();
    }
}