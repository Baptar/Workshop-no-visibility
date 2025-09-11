using UnityEngine;

public class BouttonAudio : MonoBehaviour
{

    //public AudioClip extrait;
    private AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void SetSound(AudioClip clip)
    {
        audioSource.clip = clip;
    }
}
