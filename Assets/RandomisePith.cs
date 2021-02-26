using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomisePith : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.pitch = Random.Range(0.5f, 1.5f);
    }
}