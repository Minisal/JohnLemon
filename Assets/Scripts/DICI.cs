using UnityEngine;

public class DICI : MonoBehaviour
{
    public Transform player;
    public AudioClip clip;

    int m_AttackFrom = 1;
    Animator m_Animator;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            // attack player
            player.GetComponent<Player>().BeAttacked(m_AttackFrom);
            m_Animator.Play("Attack");
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
}
