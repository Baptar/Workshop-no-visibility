using UnityEngine;

public class InputManager : MonoBehaviour
{
    public void ReceiveInput(int index)
    {
        Debug.Log("receive input from button " + index);
    }
}
