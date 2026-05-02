using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform player;
        
    void Update()
    {
        if(FindObjectOfType<Player>() == null)
        {
            return;
        }
        transform.position = new Vector3(player.position.x,transform.position.y,transform.position.z);
    }
}
