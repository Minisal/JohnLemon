using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Player player;
    public GameEnding gameEnding;

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
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == player.transform)
        {
            m_IsPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player.transform)
        {
            m_IsPlayerInRange = false;
        }
    }

    private void Update()
    {
        if(m_CanAttack)
        {
            if(!m_IsAttackInterval)
            {
                Attack();
            }
            else
            {
                if(m_AttackInterval > 0)
                {
                    m_AttackInterval -= Time.deltaTime;
                }
                else
                {
                    m_IsAttackInterval = false;
                    m_AttackInterval = attackInterval;
                }
            }
        }
        else
        {
            if(m_CantAttackTime > 0)
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

    private void Attack()
    {
        if(m_IsPlayerInRange)
        {
            Vector3 direction = player.transform.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            if(Physics.Raycast(ray, out raycastHit))
            {
                if(raycastHit.collider.transform == player.transform)
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

    public void RevivalSilence()
    {
        m_CanAttack = false;
        m_CantAttackTime = cantAttackTime;
    }
}
