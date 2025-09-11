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
    
}

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Variables
    /// </summary>
    public static GameManager instance;
    private int playerNumber = 5;
    private int buttonToSelectNumber = 4;
    
    // ref
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private RecMicro recordedClip;
    
    // array
    public int[][] playerButton;
    public AudioClip[][] playerAudioButton;
    public AudioClip[] playersName;
    
    // state
    public E_State state = E_State.None;
    
    // audio
    private AudioClip audioExplicationFirst;        // choisis une combinaison de 4 btn et associe leur à chacun un son
    private AudioClip audioExplicationOthers;       // écoute ce son et essaye de trouver l'objet correspondant
    private AudioClip audioAfterName;               // tu as sélectionner un obj, associe lui maintenant un son
    private AudioClip audioHaveSelectedAllObject;   // tu peux maintenant sortir de la piece
    private AudioClip audioEndOfTheGame;            // fin de la partie mouhahahah (tu gères pablo)
    
    // actual var
    private int actualPlayer = -1;
    private int buttonsSelected = -1;
    
    

    /// <summary>
    /// Functions
    /// </summary>
    private void Awake() => instance = this;

    private void Start()
    {
        // init player button array
        playerButton = new int[playerNumber][];
        for (int i = 0; i < playerNumber; i++)
        {
            playerButton[i] = new int[buttonToSelectNumber];
        }
        
        // init player audio array
        playerAudioButton = new AudioClip[playerNumber][];
        for (int i = 0; i < playerNumber; i++)
        {
            playerAudioButton[i] = new AudioClip[buttonToSelectNumber];
        }
        
        playersName = new AudioClip[playerNumber];
    }

    public IEnumerator StartAudioExplication()
    {
        state = E_State.ExplicationTuto;
        actualPlayer++;
        buttonsSelected = 0;
        
        AudioClip clip = actualPlayer == 0 ? audioExplicationFirst : audioExplicationOthers;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        
        // case other players and have to play audio first obj
        /*if (actualPlayer > 0)
        {
            audioSource.PlayOneShot(playerAudioButton[actualPlayer - 1][0]);
            yield return new WaitForSeconds(playerAudioButton[actualPlayer - 1][0].length);
        }*/
        state = E_State.WaitForName;
    }
    
    public void StartRecPlayerName()
    {
        state = E_State.SayingName;
        recordedClip.StartRecording();
    }

    public IEnumerator StopRecPlayerName()
    {
        playersName[actualPlayer] = recordedClip.StopRecording();
        
        state = E_State.WeHaveNameDoNext;
        audioSource.PlayOneShot(audioAfterName);
        yield return new WaitForSeconds(audioAfterName.length);
        
        // case other players and have to play audio first obj
        if (actualPlayer > 0)
        {
            audioSource.PlayOneShot(playerAudioButton[actualPlayer - 1][0]);
            yield return new WaitForSeconds(playerAudioButton[actualPlayer - 1][0].length);
        }
        state = E_State.WaitChooseObject;
    }
    
    public void StartRecChooseObject()
    {
        state = E_State.RecObjectSound;
        recordedClip.StartRecording();
    }

    public void StopRecChooseObject(int button)
    {
        playerAudioButton[actualPlayer][buttonsSelected] = recordedClip.StopRecording();
        playerButton[actualPlayer][buttonsSelected] = button;
        buttonsSelected++;

        if (buttonsSelected >= 4)
        {
            PlayerFinished();
        }

        else
        {
            state = E_State.WaitChooseObject;
        }
    }

    private void PlayerFinished()
    {
        if (actualPlayer >= 3)
        {
            GameFinished();
        }
        else
        {
            //ask to leave the room
        }
    }

    private void GameFinished()
    {
        
    }
}
