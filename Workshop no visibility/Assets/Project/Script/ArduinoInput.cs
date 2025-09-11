using System.IO.Ports;
using UnityEngine;
using System.Collections.Generic;

public class ArduinoInput : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    SerialPort sp = new SerialPort("COM5", 9600); // adapte COM selon ton PC
    string input;

    // dictionnaire pour mémoriser le dernier temps d'appui par bouton
    private Dictionary<int, float> lastPressTime = new Dictionary<int, float>();
    private float doubleClickThreshold = 0.3f; // 300 ms max entre deux clics

    void Start()
    {
        try
        {
            sp.Open();
            sp.ReadTimeout = 50;
            Debug.Log("Port série ouvert : " + sp.PortName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Impossible d’ouvrir le port série : " + e.Message);
        }
    }

    void Update()
    {
        if (sp != null && sp.IsOpen)
        {
            try
            {
                input = sp.ReadLine().Trim(); 
                Debug.Log("Arduino: " + input);

                if (input.StartsWith("BUTTON_"))
                {
                    string[] parts = input.Split('_');
                    if (parts.Length == 3)
                    {
                        int buttonIndex;
                        if (int.TryParse(parts[1], out buttonIndex))
                        {
                            string action = parts[2]; 

                            if (action == "PRESSED")
                            {
                                DetectClick(buttonIndex);
                            }
                            else if (action == "RELEASED")
                            {
                                Debug.Log("Bouton " + buttonIndex + " relâché !");
                            }
                        }
                    }
                }
            }
            catch (System.TimeoutException) { }
        }
    }

    void DetectClick(int buttonIndex)
    {
        float currentTime = Time.time;

        if (lastPressTime.ContainsKey(buttonIndex))
        {
            float timeSinceLastPress = currentTime - lastPressTime[buttonIndex];

            if (timeSinceLastPress <= doubleClickThreshold)
            {
                Debug.Log("Bouton " + buttonIndex + " DOUBLE CLIQUE !");
                inputManager.ReceiveDoubleClick(buttonIndex);
            }
            else
            {
                Debug.Log("Bouton " + buttonIndex + " SIMPLE CLIQUE !");
                inputManager.ReceiveInput(buttonIndex);
            }
        }
        else
        {
            Debug.Log("Bouton " + buttonIndex + " SIMPLE CLIQUE !");
            inputManager.ReceiveInput(buttonIndex);
        }

        // Mettre à jour le temps du dernier clic
        lastPressTime[buttonIndex] = currentTime;
    }

    void OnApplicationQuit()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
            Debug.Log("Port série fermé.");
        }
    }
}
