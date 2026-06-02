using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiPerspectiveCamera : MonoBehaviour
{
    public bool tPerson = true;

    [Header("Objetivos de cámara")]
    public Transform tpTarget;
    public Transform fpTarget;

    [Header("Visibilidad de Jugador")]
    public bool disablePlayerMesh = true;
    public GameObject playerMesh;

    private Vector2 angle = new Vector2(270 * Mathf.Deg2Rad, 0);
    private new Camera camera;
    private Vector2 nearPlaneSize;
    private Transform follow;
    private float defaultDistance;
    private float newDistance;

    [Header("Ajustes de Cámara")]
    public float maxDistace = 7f;
    public float minDistance = 2f;
    public int zoomVelocity = 300;
    public float zoomSmoth = 0.1f;
    public Vector2 sensitivity = new Vector2(1, 1);

    [Header("Tecla para cambiar perspectiva")]
    public Key switchKey = Key.Q;

    [Header("Interacción (Enlace Superliminal)")]
    public Superliminal superliminalScript; // Asigna aquí la misma cámara que tiene el script Superliminal

    // Variables para el nuevo Input System
    private Vector2 mouseDelta;
    private float scrollDelta;

    void Start()
    {
        ChangePerspective(tPerson);

        defaultDistance = (maxDistace + minDistance) / 2;
        newDistance = defaultDistance;

        Cursor.lockState = CursorLockMode.Locked; // Bloqueamos el cursor al inicio para control FP/TP
        Cursor.visible = false;

        camera = GetComponent<Camera>();

        CalculateNearPlaneSize();
    }

    void ChangePerspective(bool ThirdPerson)
    {
        if (ThirdPerson)
        {
            follow = tpTarget;
            if (disablePlayerMesh && playerMesh != null)
                playerMesh.SetActive(true);
            tPerson = true;
        }
        else
        {
            follow = fpTarget;
            if (disablePlayerMesh && playerMesh != null)
                playerMesh.SetActive(false);
            tPerson = false;
        }
    }

    private void CalculateNearPlaneSize()
    {
        float height = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.nearClipPlane;
        float width = height * camera.aspect;
        nearPlaneSize = new Vector2(width, height);
    }

    private Vector3[] GetCameraCollisionPoints(Vector3 direction)
    {
        Vector3 position = follow.position;
        Vector3 center = position + direction * (camera.nearClipPlane + 0.4f);

        Vector3 right = transform.right * nearPlaneSize.x;
        Vector3 up = transform.up * nearPlaneSize.y;

        return new Vector3[]
        {
            center - right + up,
            center + right + up,
            center - right - up,
            center + right - up
        };
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        mouseDelta = Mouse.current.delta.ReadValue();
        scrollDelta = Mouse.current.scroll.ReadValue().y;

        if (mouseDelta.x != 0)
            angle.x += mouseDelta.x * 0.0015f * sensitivity.x; // Ajustado el multiplicador para suavizar

        if (mouseDelta.y != 0)
        {
            angle.y += mouseDelta.y * 0.0015f * sensitivity.y;
            angle.y = Mathf.Clamp(angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
        }

        if (tPerson)
        {
            if (scrollDelta > 0)
                newDistance -= 0.1f * (Time.deltaTime * zoomVelocity);
            else if (scrollDelta < 0)
                newDistance += 0.1f * (Time.deltaTime * zoomVelocity);

            newDistance = Mathf.Clamp(newDistance, minDistance, maxDistace);
            defaultDistance = Mathf.Lerp(defaultDistance, newDistance, zoomSmoth);
        }
        else
        {
            defaultDistance = 0.1f;
        }

        if (Keyboard.current[switchKey].wasPressedThisFrame)
        {
            ChangePerspective(!tPerson);
        }
    }

    void LateUpdate()
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle.x) * Mathf.Cos(angle.y),
            -Mathf.Sin(angle.y),
            -Mathf.Sin(angle.x) * Mathf.Cos(angle.y)
        );

        float distance = defaultDistance;
        Vector3[] points = GetCameraCollisionPoints(direction);

        foreach (Vector3 point in points)
        {
            if (Physics.Raycast(point, direction, out RaycastHit hit, defaultDistance))
            {
                // DETECCIÓN E IGNORADO: Si el objeto golpeado por la cámara es la pelota que cargamos, lo ignoramos
                if (superliminalScript != null && superliminalScript.target != null && hit.transform == superliminalScript.target)
                {
                    continue;
                }

                distance = Mathf.Min((hit.point - follow.position).magnitude, distance);
            }
        }

        transform.position = follow.position + direction * distance;
        transform.rotation = Quaternion.LookRotation(follow.position - transform.position);
    }
}