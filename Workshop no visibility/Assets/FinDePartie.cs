using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class FinDePartie : MonoBehaviour
{
    public GameObject[] bouttons = new GameObject[16];
    public GameManager gameManager;
    
    public Transform[] spawnPoints = new Transform[20];

    public GameObject[] fleche = new GameObject[4];
    private LineRenderer[] lineRenderer =  new LineRenderer[4];
    
    private List<Transform> spawnedButton = new List<Transform>();
    private AudioSource audioSource;

    public AudioClip aChoisi;
    public AudioClip bonneReponse;
    public AudioClip mauvaiseReponse;
    public AudioClip ilARecord;
    
    public AudioClip[] textures = new AudioClip[5];
    
    
    public static FinDePartie instance;
    
    private void Awake() => instance = this;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            lineRenderer[i] = fleche[i].GetComponent<LineRenderer>();
        }
        
        audioSource = GetComponent<AudioSource>();
    }

    public Transform GetSpawnPoint(int row, int col)
    {
        return spawnPoints[row * 5 + col];
    }
    
    public GameObject GetButtonType(int row, int col)
    {
        return bouttons[row * 4 + col];
    }

    public void StartEcranDeFin()
    {
        StartCoroutine(EcranDeFin());
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
            spawnedButton.Clear();
            lineRenderer[tour].positionCount = 0;
            for (joueur = 0; joueur < 4; joueur++)
            {
                //Je prends le clip audio avec d'abord le numéro du bouton appuyé chronologiquement (le 1er boutton du joueur x)
                //puis le joueur qui a enregistré le son (numéro défini par ordre de passage)
                clipToSet = gameManager.playerAudioButton[tour].clips[joueur];
        
                //je définis le spawnpoint avec d'abord le "tour" qui est en fait le joueur dans mon cas cf schéma feutre marron moche
                //puis le bouton appuyé en suivant la logique de baptiste

                espace = GetSpawnPoint(tour, gameManager.playerButton[tour].buttons[joueur]);

        
                //J'instantie mon nouveau boutton a ma nouvelle loc et je lui attribue mon clip défini plus tot
                GameObject newBoutton = Instantiate(GetButtonType(tour,joueur), espace.position, espace.rotation);
                BouttonAudio newClip = newBoutton.GetComponent<BouttonAudio>();
                
                audioSource.clip = gameManager.playersName[joueur];
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length + 0.2f);

                audioSource.clip = aChoisi;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length + 0.2f);
                
                audioSource.clip = textures[gameManager.playerButton[tour].buttons[joueur]];
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length + 0.2f);

                if (joueur != 0)
                {
                    if (gameManager.playerButton[tour].buttons[joueur] == gameManager.playerButton[tour].buttons[joueur - 1])
                    {
                        audioSource.clip = bonneReponse;
                        audioSource.Play();
                        yield return new WaitForSeconds(audioSource.clip.length + 0.2f);
                    }
                    else
                    {
                        audioSource.clip = mauvaiseReponse;
                        audioSource.Play();
                        yield return new WaitForSeconds(audioSource.clip.length + 0.2f);
                    }
                }
                
                audioSource.clip = ilARecord;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length + 0.2f);
        
                //ça parle pour soi
                newClip.SetSound(clipToSet);
                newClip.PlaySound();
                yield return new WaitForSeconds(clipToSet.length);
                
                spawnedButton.Add(newBoutton.transform);
                lineRenderer[tour].positionCount = spawnedButton.Count;
                lineRenderer[tour].SetPosition(spawnedButton.Count - 1, newBoutton.transform.position);

                yield return new WaitForSeconds(0.5f);

            }
        }
       
        yield return null;
    }
}
