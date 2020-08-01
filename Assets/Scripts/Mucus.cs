using UnityEngine;

public class Mucus : MonoBehaviour
{
    public Player player;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player.transform)
        {
            player.moveSpeed *= 0.5f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player.transform)
        {
            player.moveSpeed *= 2f;
        }
    }
}
