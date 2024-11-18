using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    [Tooltip("오디오 소스")]
    private AudioSource audioSource;

    [SerializeField]
    [Tooltip("골 사운드")]
    private AudioClip _goalSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            audioSource.PlayOneShot(_goalSound);
            other.gameObject.SetActive(false);
        }
    }
}
