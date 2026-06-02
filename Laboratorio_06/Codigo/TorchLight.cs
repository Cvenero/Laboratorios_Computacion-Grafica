using UnityEngine;
using UnityEngine.InputSystem;

public class TorchLight : MonoBehaviour
{
    public Light luzLinterna;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            luzLinterna.enabled = !luzLinterna.enabled;
        }
    }
}