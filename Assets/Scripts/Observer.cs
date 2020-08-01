using UnityEngine;

public class Observer : MonoBehaviour
{
	public Transform playerTrans;
	public GameEnding gameEnding;
    public Player player;

    public float cantAttackTime = 3f; // skill cool down time after revival
    public float attackInterval = 1f;

    int m_AttackFrom = 0;
    bool m_IsPlayerInRange;
    bool m_CanAttack = true;
    bool m_IsAttackInterval = false;
    float m_CantAttackTime;
    float m_AttackInterval;
    AudioSource m_AudioSource;

    private void Start()
    {
        m_CantAttackTime = cantAttackTime;
        m_AttackInterval = attackInterval;
        m_AudioSource = transform.parent.GetComponent<AudioSource>();
        
    }

    void OnTriggerEnter (Collider other)
    {
    	if(other.transform == playerTrans)
        {
            m_IsPlayerInRange = true;            
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.transform == playerTrans)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if (m_CanAttack)
        {
            if (m_IsAttackInterval)
            {
                if (m_AttackInterval > 0)
                {
                    m_AttackInterval -= Time.deltaTime;
                }
                else
                {
                    m_IsAttackInterval = false;
                    m_AttackInterval = attackInterval;
                }
            }
            else
            {
                Attack();
            }
        }
        else
        {
            if (player.GetComponent<Collider>().transform == player)
            {
                m_IsPlayerInRange = false;
            }

            if (m_CantAttackTime > 0)
            {
                m_CantAttackTime -= Time.deltaTime;
            }
            else
            {
                m_CanAttack = true;
                m_CantAttackTime = cantAttackTime;
            }
        }
    }

    public void RevivalSilence()
    {
        Debug.Log("revival silence");
        m_CanAttack = false;
        m_CantAttackTime = cantAttackTime;
    }

    private void Attack()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = playerTrans.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == playerTrans)
                {
                    player.BeAttacked(m_AttackFrom);
                    m_AudioSource.Play();
                    m_IsAttackInterval = true;
                }
            }
        }
    }

    public void EnemyStopAttack()
    {
        m_CanAttack = false;
    }
}
