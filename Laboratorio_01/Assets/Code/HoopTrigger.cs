using UnityEngine;
using UnityEngine.SceneManagement;

public class HoopTrigger : MonoBehaviour
{
    public ScoreManager scoreManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && rb.linearVelocity.y < 0)
            {
                scoreManager.AddPoints(1);
            }
        }
    }
}