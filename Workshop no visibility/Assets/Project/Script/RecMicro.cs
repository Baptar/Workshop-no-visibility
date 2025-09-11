using UnityEngine;

public class RecMicro : MonoBehaviour
{
    private bool isRecording;
    private AudioClip recordedClip;
    [SerializeField] private AudioSource audioSource;

    public void StartRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.Log("No Microphone devices found!");
            return;
        }

        Debug.Log("Start Recording");
        isRecording = true;

        string device = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 3599;

        recordedClip = Microphone.Start(device, false, lengthSec, sampleRate);
    }

    public AudioClip StopRecording()
    {
        Debug.Log("Stop Recording");
        Microphone.End(null);
        isRecording = false;
        
        return recordedClip;
    }

    public void ListenAudio()
    {
        if (recordedClip == null)
        {
            Debug.Log("no audio to listen");
            return;
        }

        Debug.Log("Listen Recording");
        audioSource.PlayOneShot(recordedClip);
    }
    
    public AudioClip GetRecordedClip() => recordedClip;
}
