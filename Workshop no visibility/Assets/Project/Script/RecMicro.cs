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

    public void StopRecording()
    {
        Debug.Log("Stop Recording");
        Microphone.End(null);
        isRecording = false;
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

    public void OnButtonClicked()
    {
        Debug.Log("Actual state is : " + GameManager.instance.state);
        
        switch (GameManager.instance.state)
        {
            case E_State.None:
                Debug.Log("Start asking name");
                StartCoroutine(GameManager.instance.StartAudioAskName());
                break;
            case E_State.AudioAskName:
                Debug.Log("can't click --> do nothing");
                // ne rien faire
                break;
            case E_State.SayingName:
                Debug.Log("stop rec name and start explication");
                StartCoroutine(GameManager.instance.StopRecPlayerName());
                break;
        }
        
        /*if (!isRecording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }*/
    }
}
