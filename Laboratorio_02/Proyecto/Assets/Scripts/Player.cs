using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float speed = 10f;
    public float jumpHeight = 7f;    
    public bool isGround = true;
    public Text appleText;
    public int currentApples = 0;
    public GameObject panel;
    private bool facingRight = true;

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        appleText.text = currentApples.ToString();

        // Calculamos el movimiento horizontal
        float moveInput = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);

        // Movimiento físico
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Pasamos el moveInput a la función flip
        Flip(moveInput);

        // Salto
        if (kb.spaceKey.wasPressedThisFrame && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
            animator.SetBool("Jump", true);
            isGround = false;
        }

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (moveInput < 0.1f)
        {
            animator.SetFloat("Run", 0f);
        }

    }
        
    void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight)
        {
            facingRight = true;
            transform.eulerAngles = Vector3.zero; // Mirar a la derecha (0,0,0)
        }
        else if (horizontal < 0 && facingRight)
        {
            facingRight = false;
            transform.eulerAngles = new Vector3(0, 180, 0); // Mirar a la izquierda
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Asegurate de que el suelo tenga el Tag "Ground" en el Inspector
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }

        if(other.collider.tag == "Saw" || other.collider.tag == "Spike")
        {
            Die();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Apple")
        {
            currentApples++;
            Destroy(other.gameObject);
        }
        if (other.tag == "Cup")
        {
            Debug.Log("Ganaste!");
            SceneManager.LoadScene("Menu");
        }
    }

    void Die()
    {
        Debug.Log("Jugador murio");
        Destroy(this.gameObject);
        panel.active = true;
    }
}