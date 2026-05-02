using UnityEngine;
public class ParallaxEffect : MonoBehaviour
{
    public GameObject cam;
    public float parallaxFactor; // Velocidad de movimiento
    private float startPos;

    void Start()
    {
        startPos = transform.position.x;
    }

    void Update()
    {
        // Movimiento basado en la posición de la cámara
        float distance = (cam.transform.position.x * parallaxFactor);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}