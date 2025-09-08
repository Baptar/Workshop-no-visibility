using System.Collections;
using UnityEngine;

public enum E_State
{
    None,
    AudioAskName,
    SayingName,
    ExplicationTuto,
    ChooseObject,
    RecObjectSound
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
    private AudioClip audioAskName;                 // bienvenue dis ton nom
    private AudioClip audioExplicationFirst;        // choisis une combinaison de 4 btn et associe leur à chacun un son
    private AudioClip audioExplicationOthers;       // écoute ce son et essaye de trouver l'objet correspondant
    private AudioClip audioChooseObject;            // tu as sélectionner un obj, associe lui maintenant un son
    private AudioClip audioSelectAllObject;         // tu peux maintenant sortir de la piece
    //private AudioClip audioEndOfTheGame;          // fin de la partie mouhahahah (tu gères pablo)
    
    // actual var
    private int actualPlayer = -1;
    
    

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


    public IEnumerator StartAudioAskName()
    {
        state = E_State.AudioAskName;
        actualPlayer++;
        audioSource.PlayOneShot(audioAskName);
        yield return new WaitForSeconds(audioAskName.length);
        StartRecPlayerName(actualPlayer);
    }

    private void StartRecPlayerName(int player)
    {
        state = E_State.SayingName;
        recordedClip.StartRecording();
    }

    public IEnumerator StopRecPlayerName()
    {
        state = E_State.ExplicationTuto;
        AudioClip clip = actualPlayer == 0 ? audioExplicationFirst : audioExplicationOthers;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        
        StartChoosingObject();
    }

    private void StartChoosingObject()
    {
        state = E_State.ChooseObject;

        if (actualPlayer > 0)
        {
            //audioSource.PlayOneShot();
        }
    }

    private void EndScrren()
    {
        
    }
}
