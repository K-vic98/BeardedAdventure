using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _canvasGroup.alpha = 1;
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        _canvasGroup.alpha = 0;
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}