using System.Collections;
using UnityEngine;

public enum E_State
{
    None,
    ExplicationTuto,// first audio, explain everything and ask for name
    WaitForName,
    SayingName,
    WeHaveNameDoNext, // after rec name -> continue 
    WaitChooseObject,
    RecObjectSound,
    YouCanLeave,
    WaitEndScreen,
    EndScreen
}

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Variables
    /// </summary>
    public static GameManager instance;
    private int playerNumber = 4;
    private int buttonToSelectNumber = 4;
    
    // ref
    public AudioSource audioSource;
    [SerializeField] private RecMicro recordedClip;
    
    [System.Serializable]
    public class AudioClipRow
    {
        public AudioClip[] clips;
    }
    
    [System.Serializable]
    public class ButtonIntRow
    {
        public int[] buttons;
    }
    
    
    // array
    public ButtonIntRow[] playerButton;
    public AudioClipRow[] playerAudioButton;
    public AudioClip[] playersName;
    
    // state
    public E_State state = E_State.None;
    
    // audio
    [SerializeField] private AudioClip audioExplicationFirst;        // choisis une combinaison de 4 btn et associe leur à chacun un son
    [SerializeField] private AudioClip audioExplicationOthers;       // écoute ce son et essaye de trouver l'objet correspondant
    [SerializeField] private AudioClip audioAfterName;               // tu as sélectionner un obj, associe lui maintenant un son
    [SerializeField] private AudioClip audioHaveSelectedAllObject;   // tu peux maintenant sortir de la piece
    
    [SerializeField] private AudioClip audioStartRecButton;            // fin de la partie mouhahahah (tu gères pablo)
    [SerializeField] private AudioClip audioStopRecButton;            // fin de la partie mouhahahah (tu gères pablo)
    
    
    // DEBUG
    // actual var
    public int actualPlayer = -1;
    public int buttonsSelected = -1;
    
    

    /// <summary>
    /// Functions
    /// </summary>
    private void Awake() => instance = this;

    private void Start()
    {
        // init player button array
        playerButton = new ButtonIntRow[playerNumber];
        for (int i = 0; i < playerNumber; i++)
        {
            playerButton[i] = new ButtonIntRow();
            playerButton[i].buttons = new int[buttonToSelectNumber];
        }
        
        // init player audio array
        playerAudioButton = new AudioClipRow[playerNumber];
        for (int i = 0; i < playerNumber; i++)
        {
            playerAudioButton[i] = new AudioClipRow();
            playerAudioButton[i].clips = new AudioClip[buttonToSelectNumber];
        }
        
        playersName = new AudioClip[playerNumber];
    }

    public IEnumerator StartAudioExplication()
    {
        state = E_State.ExplicationTuto;
        actualPlayer++;
        buttonsSelected = 0;

        InputManager.instance.ResetUsableButtons();
        
        AudioClip clip = actualPlayer == 0 ? audioExplicationFirst : audioExplicationOthers;
        if (clip) audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);

        state = E_State.WaitForName;
    }
    
    public IEnumerator StartRecPlayerName()
    {
        if (audioStartRecButton) audioSource.PlayOneShot(audioStartRecButton);
        yield return new WaitForSeconds(audioStartRecButton.length);
        state = E_State.SayingName;
        recordedClip.StartRecording();
    }

    public IEnumerator StopRecPlayerName()
    {
        playersName[actualPlayer] = recordedClip.StopRecording();
        
        state = E_State.WeHaveNameDoNext;
        if (audioAfterName) audioSource.PlayOneShot(audioAfterName);
        yield return new WaitForSeconds(audioAfterName.length + 0.5f);
        
        // case other players and have to play audio first obj
        if (actualPlayer > 0)
        {
            audioSource.PlayOneShot(playerAudioButton[actualPlayer - 1].clips[0]);
            state = E_State.WaitChooseObject;
            Debug.Log("HIIIIIIIIIIIIIII");
        }
        else state = E_State.WaitChooseObject;
    }
    
    public IEnumerator StartRecChooseObject()
    {
        if (audioStartRecButton) audioSource.PlayOneShot(audioStartRecButton);
        yield return new WaitForSeconds(audioStartRecButton.length - 0.5f);
        state = E_State.RecObjectSound;
        recordedClip.StartRecording();
    }

    public void StopRecChooseObject(int button)
    {
        if (audioStopRecButton) audioSource.PlayOneShot(audioStopRecButton);
        
        playerAudioButton[actualPlayer].clips[buttonsSelected] = recordedClip.StopRecording();
        playerButton[actualPlayer].buttons[buttonsSelected] = button;
        Debug.Log("le joueur " + actualPlayer + " a associé le bouton " + playerButton[actualPlayer].buttons[buttonsSelected]);
        InputManager.instance.SetUsableButton(button, false);
        
        buttonsSelected++;

        if (buttonsSelected >= 4)
        {
            StartCoroutine(PlayerFinished());
        }

        else
        {
            state = E_State.WaitChooseObject;
        }
    }

    private IEnumerator PlayerFinished()
    {
        state = E_State.YouCanLeave;
        if (audioHaveSelectedAllObject)
        {
            audioSource.PlayOneShot(audioHaveSelectedAllObject);
            yield return new WaitForSeconds(audioHaveSelectedAllObject.length);
            state = actualPlayer >= 3 ? E_State.WaitEndScreen : E_State.None;
        }
    }

    public void GameFinished()
    {
        state = E_State.EndScreen;
        //FinDePartie.instance.
    }
}
