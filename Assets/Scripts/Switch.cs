using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Light switchLight;
    public float LightOnTime = 10f;
    public float LightOffTime = 15f;
    public Player player;
    public Enemies enemies;
    public float EnemiesRevivalTime = 3f;

    bool m_IsLightOn = false;
    float m_LightOnTime;
    float m_LightOffTime;
    float m_EnemyRevivalTime;

    Animator m_Animator;
    AudioSource m_OnAudio;
    AudioSource m_OffAudio;
    List<Transform> m_enemies;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_OnAudio = GetComponents<AudioSource>()[0];
        m_OffAudio = GetComponents<AudioSource>()[1];
        m_LightOnTime = LightOnTime;
        m_LightOffTime = LightOffTime;
        m_EnemyRevivalTime = EnemiesRevivalTime;
    }

    private void OnTriggerStay(Collider other)
    {
        OperateSwitch(other);
    }

    void OperateSwitch(Collider other)
    {
        // input space to operate switch
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(m_IsLightOn)
            {
                OffLight();
                m_OffAudio.Play();
                player.TurnOffLight();
            }
            else
            {
                OnLight();
                m_OnAudio.Play();
                player.TurnOnLight();
            }
        }
    }

    void OffLight()
    {
        m_IsLightOn = false;
        m_Animator.Play("SwitchOff");
        switchLight.intensity = 0;

        m_LightOnTime = LightOnTime;
        m_LightOffTime = LightOffTime;
        m_EnemyRevivalTime = EnemiesRevivalTime;
    }

    void OnLight()
    {
        m_IsLightOn = true;
        m_Animator.Play("SwitchOn");
        switchLight.intensity = 100;

        KillEnemiesInRange();

        m_LightOnTime = LightOnTime;
        m_LightOffTime = LightOffTime;
        m_EnemyRevivalTime = EnemiesRevivalTime;
    }

    void KillEnemiesInRange()
    {
        m_enemies = enemies.enemies;
        for (int i = 0; i < m_enemies.Count; i++)
        {
            float distance = (m_enemies[i].position - switchLight.transform.position).sqrMagnitude;
            if (distance <= switchLight.range * switchLight.range)
            {
                enemies.BeAttacked(m_enemies[i].name);
            }
        }
    }

    private void Update()
    {
        if(m_IsLightOn)
        {
            if(m_LightOnTime>0)
            {
                m_LightOnTime -= Time.deltaTime;
                KillEnemiesInRange();
            }
            else
            {
                OffLight();
            }
        }
        else
        {
            if(m_LightOffTime>0)
            {
                m_LightOffTime -= Time.deltaTime;
            }
            else
            {
                OnLight();
            }
        }
        if(!m_IsLightOn)
        {
            if(m_EnemyRevivalTime>0)
            {
                m_EnemyRevivalTime -= Time.deltaTime;
            }
            else
            {
                enemies.ReviveAllEnemies();
                m_EnemyRevivalTime = EnemiesRevivalTime;
            }
        }
    }
}
