using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public Light[] focos;
    public float rangoActivacion = 3f;
    public Transform jugador;

    private bool lucesEncendidas = true;

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(jugador.position, transform.position);

        if (distancia <= rangoActivacion)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                lucesEncendidas = !lucesEncendidas;

                foreach (Light foco in focos)
                {
                    foco.enabled = lucesEncendidas;
                }
            }
        }
    }
}