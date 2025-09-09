using System.IO.Ports;
using UnityEngine;

public class ArduinoInput : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    SerialPort sp = new SerialPort("COM5", 9600); // adapte COM selon ton PC
    string input;

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 50;
    }

    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                input = sp.ReadLine();
                Debug.Log("Arduino: " + input);

                // Exemple : 01000011 → traiter les boutons
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '1')
                        inputManager.ReceiveInput(i);
                        //Debug.Log("Bouton " + i + " pressé !");
                }
            }
            catch (System.Exception) { }
        }
    }

    void OnApplicationQuit()
    {
        if (sp.IsOpen) sp.Close();
    }
}