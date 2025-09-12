using System.Collections;
using UnityEngine;

public class FinDePartie : MonoBehaviour
{
    public GameObject[] bouttons = new GameObject[4];
    public GameManager gameManager;
    
    public Transform[] spawnPoints = new Transform[20];
    
    public static FinDePartie instance;
    
    private void Awake() => instance = this;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetSpawnPoint(int row, int col)
    {
        return spawnPoints[row * 5 + col];
    }

    private IEnumerator EcranDeFin()
    {
        //Initier tour et joueur no shit
        int tour = 0;
        int joueur = 0;

        AudioClip clipToSet;
        Transform espace;
        for (tour = 0; tour < 4; tour++)
        {
            for (joueur = 0; joueur < 4; joueur++)
            {
                //Je prends le clip audio avec d'abord le numéro du bouton appuyé chronologiquement (le 1er boutton du joueur x)
                //puis le joueur qui a enregistré le son (numéro défini par ordre de passage)
                clipToSet = gameManager.playerAudioButton[tour][joueur];
        
                //je définis le spawnpoint avec d'abord le "tour" qui est en fait le joueur dans mon cas cf schéma feutre marron moche
                //puis le bouton appuyé en suivant la logique de baptiste
                espace = GetSpawnPoint(tour, gameManager.playerButton[tour][joueur]);
        
                //J'instantie mon nouveau boutton a ma nouvelle loc et je lui attribue mon clip défini plus tot
                GameObject newBoutton = Instantiate(bouttons[tour], espace.position, espace.rotation);
                BouttonAudio newClip = newBoutton.GetComponent<BouttonAudio>();
        
                //ça parle pour soi
                newClip.SetSound(clipToSet);
                newClip.PlaySound();
    
            }
        }
       
        yield return null;
    }
}
