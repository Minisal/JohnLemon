using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int maxHealPoint = 1;
    public float revivalInterval = 3f;
    public AudioClip[] dieAudios;
    public List<Transform> enemies;

    int[] m_EnemiesHealthPoint;
    List<Transform> m_DeadEnemies = null;

    private void Awake()
    {
        foreach (Transform enemy in this.transform)
        {
            enemies.Add(enemy);
        }
        m_EnemiesHealthPoint = new int[enemies.Count];
        for(int i=0; i<enemies.Count; i++)
        {
            m_EnemiesHealthPoint[i] = maxHealPoint;
        }
        m_DeadEnemies = new List<Transform>();
    }

    public void BeAttacked(string name, int damage = 1)
    {
        for(int index=0; index<enemies.Count; index++)
        {
            if (enemies[index].name == name)
            {
                m_EnemiesHealthPoint[index] -= damage;
                if (m_EnemiesHealthPoint[index] <= 0)
                {
                    Die(index);
                } 
                break;
            }
                
        }     
    }

    public void ReviveAllEnemies()
    {
        foreach(Transform deadEnemy in m_DeadEnemies)
        {
            if(deadEnemy.gameObject.activeSelf == false)
            {
                Vector3 startPoint = deadEnemy.GetComponent<WaypointPatrol>().waypoints[0].position;
                deadEnemy.transform.position = startPoint;
            }
            deadEnemy.gameObject.SetActive(true);
            deadEnemy.GetComponent<Ghost>().RevivalSilence();
        }
        m_DeadEnemies.Clear();
    }

    void Die(int index)
    {
        Transform deadEnemy = enemies[index];
        if(m_DeadEnemies.Contains(deadEnemy))
        {
            return;
        }
        Vector3 pos = deadEnemy.transform.position;

        if(deadEnemy.tag == "Ghost")
        {
            AudioSource.PlayClipAtPoint(dieAudios[0], pos);
        }
        else
        {
            AudioSource.PlayClipAtPoint(dieAudios[1], pos);
        }

        m_DeadEnemies.Add(deadEnemy);
        deadEnemy.gameObject.SetActive(false);
    }

    public void AllEnemiesStopAttack()
    {
        for(int i=0; i< enemies.Count; i++)
        {
            Ghost tempEnemy = enemies[i].GetComponent<Ghost>();
            tempEnemy.EnemyStopAttack();
            if(enemies[i].tag == "Gargoyle")
            {
                Observer tempObserver = enemies[i].GetComponentInChildren<Observer>();
                tempObserver.EnemyStopAttack();
            }
        }
    }
}
