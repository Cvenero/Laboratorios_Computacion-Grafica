using UnityEngine;
using UnityEngine.InputSystem;

public class Superliminal : MonoBehaviour
{
    [Header("Referencias Externas")]
    public MultiPerspectiveCamera cameraScript;

    [Header("Components")]
    public Transform target;

    [Header("Parameters")]
    public LayerMask targetMask;
    public LayerMask ignoreTargetMask;

    private float originalDistance;
    private Vector3 originalScale;
    private float currentDistance;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleInput();

        if (target != null)
        {
            // Cambia el tamaño y posicion en tiempo real para mantener la ilusion
            ResizeAndPositionTarget();
        }
    }

    void HandleInput()
    {
        if (cameraScript == null || cameraScript.tPerson)
        {
            if (target != null) DropTarget();
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (target == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
                {
                    target = hit.transform;

                    if (target.GetComponent<Rigidbody>() != null)
                    {
                        target.GetComponent<Rigidbody>().isKinematic = true;
                    }

                    // Guardamos los datos iniciales al momento de levantar el objeto
                    originalDistance = Vector3.Distance(transform.position, target.position);
                    originalScale = target.localScale;
                }
            }
            else
            {
                DropTarget();
            }
        }
    }

    void ResizeAndPositionTarget()
    {
        RaycastHit hit;
        Vector3 targetDirection = transform.forward;

        // Lanzamos un raycast continuo hacia el fondo para saber a que distancia está la pared/suelo
        if (Physics.Raycast(transform.position, targetDirection, out hit, 100f, ignoreTargetMask))
        {
            currentDistance = Vector3.Distance(transform.position, hit.point);
        }
        else
        {
            currentDistance = 15f; // Distancia por defecto si se mira al cielo vacío
        }

        // Limite mínimo de distancia para evitar que el objeto colapse en nuestro frente
        if (currentDistance < 1.5f) currentDistance = 1.5f;
        
        // El factor de escala se calcula a cada fotograma. Si el fondo esta lejos, la pelota
        // se hace gigante en la mano. Al ojo humano le parecera que no ha cambiado de tamaño.
        float scaleFactor = currentDistance / originalDistance;

        // Límites de seguridad para que las físicas no se rompan
        if (scaleFactor > 12f) scaleFactor = 12f;
        if (scaleFactor < 0.1f) scaleFactor = 0.1f;

        // Aplicamos la escala en tiempo real
        target.localScale = originalScale * scaleFactor;
        
        // Tomamos el punto de impacto en la pared/suelo (hit.point) y le sumamos el radio del objeto
        // multiplicado por 'hit.normal' (la dirección perpendicular a la superficie).
        // Esto empuja la pelota hacia afuera del suelo o muro, evitando que se entierre.
        float objectRadius = target.GetComponent<Collider>() != null ? target.GetComponent<Collider>().bounds.extents.x : target.localScale.x * 0.5f;

        if (hit.collider != null)
        {
            // Si hay un impacto válido, se apoya perfectamente en la superficie usando la normal
            target.position = hit.point + (hit.normal * objectRadius);
        }
        else
        {
            // Si apuntamos al vacio, flota en el aire frente a nosotros
            target.position = transform.position + (targetDirection * currentDistance);
        }
    }

    void DropTarget()
    {
        // Al soltar, como el objeto ya tiene el tamaño y posición calculados en tiempo real,
        // solo le devolvemos la gravedad de inmediato sin saltos visuales bruscos.
        if (target.GetComponent<Rigidbody>() != null)
        {
            target.GetComponent<Rigidbody>().isKinematic = false;
        }

        target = null;
    }
}
