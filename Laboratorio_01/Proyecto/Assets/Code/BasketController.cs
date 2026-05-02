using UnityEngine;
using UnityEngine.InputSystem;

public class BasketController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public Transform ball;
    public Transform arms;
    public Transform posOverHead;
    public Transform posDribble;
    public Transform target;
    public float shootForce = 10f;

    private Rigidbody rb;
    private bool inHands = true;
    private float timeSinceShot = 0f;
    private float pickupDelay = 0.8f;

    void Start()
    {
        rb = ball.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        // MOVIMIENTO
        Vector3 direction = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) direction.z += 1;
        if (Keyboard.current.sKey.isPressed) direction.z -= 1;
        if (Keyboard.current.aKey.isPressed) direction.x -= 1;
        if (Keyboard.current.dKey.isPressed) direction.x += 1;

        if (direction != Vector3.zero)
        {
            transform.position += direction * MoveSpeed * Time.deltaTime;
            transform.LookAt(transform.position + direction);
        }

        // BALÓN EN MANOS
        if (inHands)
        {
            rb.isKinematic = true;
            if (Keyboard.current.spaceKey.isPressed)
            {
                ball.position = posOverHead.position;
                arms.localEulerAngles = Vector3.right * 180;
                transform.LookAt(target.position);
            }
            else
            {
                ball.position = posDribble.position +
                    Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                arms.localEulerAngles = Vector3.zero;
            }

            if (Keyboard.current.fKey.wasPressedThisFrame)
                Shoot();
        }
        else
        {
            arms.localEulerAngles = Vector3.zero;
        }

        // RECUPERAR BALÓN
        if (!inHands)
        {
            timeSinceShot += Time.deltaTime;

            if (timeSinceShot > pickupDelay)
            {
                float distance = Vector3.Distance(transform.position, ball.position);
                bool ballSlowed = rb.linearVelocity.magnitude < 3f;

                if (distance < 2.5f && ballSlowed)
                    PickBall();
            }
        }
    }

    void PickBall()
    {
        inHands = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        ball.position = posDribble.position;
    }

    void Shoot()
    {
        inHands = false;
        timeSinceShot = 0f;
        rb.isKinematic = false;
        Vector3 shootDirection = transform.forward + Vector3.up * 1f;
        rb.linearVelocity = shootDirection * shootForce;
    }
}




/*using UnityEngine;
using UnityEngine.InputSystem;

public class BasketController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public Transform ball;
    public Transform arms;
    public Transform posOverHead;
    public Transform posDribble;
    public Transform target;
    public float shootForce = 10f;
    
    private Rigidbody rb;
    private bool inHands = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento del personaje
        Vector3 direction = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            direction.z += 1;

        if (Keyboard.current.sKey.isPressed)
            direction.z -= 1;

        if (Keyboard.current.aKey.isPressed)
            direction.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            direction.x += 1;

        if (direction != Vector3.zero)
        {
            transform.position += direction * MoveSpeed * Time.deltaTime;
            transform.LookAt(transform.position + direction);
        }

        //Posesion del Balon
        if (inHands)
        {
            
            //Sostener sobre la cabeza
            if (Keyboard.current.spaceKey.isPressed)
            {
                ball.position = posOverHead.position;
                arms.localEulerAngles = Vector3.right * 180;
                transform.LookAt(target.parent.position);
            }

            //Driblear - botar el balon
            else
            {
                ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                arms.localEulerAngles = Vector3.right * 0;
            }
        }
        else 
        {
            // Cuando no se tiene la pelota
            arms.localEulerAngles = Vector3.right * 0;
        }
        if (!inHands)
        {
            float distance = Vector3.Distance(arms.position, ball.position);

            if (distance < 2f && ball.position.y < arms.position.y + 1f)
            {
                PickBall();
            }
        }
        if (Keyboard.current.fKey.wasPressedThisFrame && inHands)
        {
            Shoot();
        }
    }
    void PickBall()
    {
        inHands = true;

        // detener completamente la pelota
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // desactivar física
        rb.isKinematic = true;

        // colocarla inmediatamente en posición de dribleo
        ball.position = posDribble.position;

        // brazos en posición normal
        arms.localEulerAngles = Vector3.right * 0;
    }

    void Shoot()
    {
        inHands = false;

        // Activar física
        rb.isKinematic = false;

        // Direccion hacia adelante del personaje
        Vector3 shootDirection = transform.forward + Vector3.up * 1.0f;

        rb.linearVelocity = shootDirection * shootForce;
    }

}
*/