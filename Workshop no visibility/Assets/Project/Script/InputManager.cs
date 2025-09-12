using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool needDoubleClick = false;
    private float timeBetweenDoubleClick = 1.0f;
    private float actualTime = 0.0f;

    private int oldButtonClicked = -1;
    
    [SerializeField] private AudioClip bouttonAudio;
    
    private void Update()
    {
        
    }
    
    public void ReceiveInput(int index)
    {
        Debug.Log("receive input from button " + index);
        GameManager.instance.audioSource.PlayOneShot(bouttonAudio);
        
        GameManager gameManager = GameManager.instance;
        switch (gameManager.state)
        {
            case E_State.None:
                StartCoroutine(GameManager.instance.StartAudioExplication());
                break;
            case E_State.SayingName:
                StartCoroutine(gameManager.StopRecPlayerName());
                break;
            case E_State.RecObjectSound:
                gameManager.StopRecChooseObject(index);
                break;
        }
    }

    public void ReceiveDoubleClick(int index)
    {
        Debug.Log("receive double click from button " + index);
        GameManager.instance.audioSource.PlayOneShot(bouttonAudio);
        
        GameManager gameManager = GameManager.instance;
        switch (gameManager.state)
        {
            case E_State.WaitForName:
                gameManager.StartRecPlayerName();
                break;
            case E_State.WaitChooseObject:
                gameManager.StartRecChooseObject();
                break;
        }
    }
}
