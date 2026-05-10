using UnityEngine;

public class WinZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger tocado por: " + other.name);
        if (other.CompareTag("Player"))
        {
            GameManager.instance.WinGame();
        }
    }
}