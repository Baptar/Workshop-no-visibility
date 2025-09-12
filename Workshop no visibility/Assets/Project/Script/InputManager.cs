using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    
    [SerializeField] private AudioClip bouttonAudio;
    [SerializeField] private AudioClip alreadyUsedButton;
    
    // DEBUG
    public bool[] canUseButton = new bool[5];

    private void Awake() => instance = this;

    private void Start()
    {
        ResetUsableButtons();
    }
    
    public void ResetUsableButtons()
    {
        for (int i = 0; i < canUseButton.Length; i++)
        {
            canUseButton[i] = true;
        }
    }

    public void SetUsableButton(int indexButton, bool value)
    {
        canUseButton[indexButton] = value;
    }
    
    public void ReceiveInput(int index)
    {
        if (!canUseButton[index])
        {
            GameManager.instance.audioSource.PlayOneShot(alreadyUsedButton);
            Debug.Log("alreadyUsedButton");
            return;
        }
        
        
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
                gameManager.StopRecChooseObject(index + 1);
                break;
            case E_State.WaitEndScreen:
                gameManager.GameFinished();
                break;
        }
    }

    public void ReceiveDoubleClick(int index)
    {
        if (!canUseButton[index])
        {
            GameManager.instance.audioSource.PlayOneShot(alreadyUsedButton);
            return;
        }
        
        Debug.Log("receive double click from button " + index);
        GameManager.instance.audioSource.PlayOneShot(bouttonAudio);
        
        GameManager gameManager = GameManager.instance;
        switch (gameManager.state)
        {
            case E_State.WaitForName:
                StartCoroutine(gameManager.StartRecPlayerName());
                break;
            case E_State.WaitChooseObject:
                StartCoroutine(gameManager.StartRecChooseObject());
                break;
        }
    }
}
