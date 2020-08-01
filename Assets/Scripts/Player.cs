using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // move
    public float turnSpeed = 20f;
    public float moveSpeed = 1f;

    // health point
    public int maxHealthPoint = 3;
    public GameEnding gameEnding;
    public HealthPoint[] healthpoints;

    public Enemies enemies;

    public float shotInterval = 1f;
    public CursorUI cursorUI;

    public GameObject model;

    float m_ShotInterval;
    bool m_IsShot = false;
    bool m_IsShotInterval = false;

    bool m_IsDead = false;
    int m_HealthPoint;
    bool m_HoldWeapon = false;

    GameObject m_Weapon;

    FadeModel fadeModel;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource_step;
    AudioSource m_AudioSource_hurt;
    
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    HealthPoint[] m_HealthPoints;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource_step = GetComponents<AudioSource>()[0];
        m_AudioSource_hurt = GetComponents<AudioSource>()[1];
        m_HealthPoint = maxHealthPoint;
        m_ShotInterval = shotInterval;
    }

    private void Update()
    {
        if(!m_IsDead && m_HoldWeapon)
        {
            if(!m_IsShotInterval)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    Shot();
                }
            }
            else
            {
                if(m_ShotInterval > 0)
                {
                    m_ShotInterval -= Time.deltaTime;
                }
                else
                {
                    m_IsShotInterval = false;
                    m_ShotInterval = shotInterval;
                }
            }
        }
    }

    public void Shot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Vector3 pos;

        if(Physics.Raycast(ray, out raycastHit))
        {
            pos = raycastHit.point;
            transform.rotation = Quaternion.LookRotation(pos - transform.position);
            // adjust the face angle of player
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
        m_IsShot = true;
        m_IsShotInterval = true;
        m_Weapon.transform.GetComponent<Weapon>().Fire();
        cursorUI.Fire();
    }

    public void TurnOffLight()
    {
        if (m_Animator.GetBool("HoldWeapon"))
        {
            m_Animator.Play("Fight_SwitchOff");
        }
        else
        {
            m_Animator.Play("SwitchOff");
        }
    }

    public void TurnOnLight()
    {
        if (m_Animator.GetBool("HoldWeapon"))
        {
            m_Animator.Play("Fight_SwitchOn");
        }
        else
        {
            m_Animator.Play("SwitchOn");
        }
    }

    public void GetWeapon()
    {
        m_HoldWeapon = true;
        m_Animator.SetBool("HoldWeapon", m_HoldWeapon);
        m_Weapon = GameObject.FindGameObjectWithTag("Weapon");
        cursorUI.Aim();//绘制准星
    }

    public void OpenBox()
    {
        m_Animator.Play("BoxOpen");
        Invoke("GetWeapon", 1f);
    }

    public void BeAttacked(int attackFrom, int damage = 1)
    {
        if (m_HealthPoint < 0)
        {
            return;
        }

        // attack From enemy: 0
        // attack From trap: 1
        m_HealthPoint -= damage;

        for(int i=0; i<maxHealthPoint; i++)
        {
            if(i==m_HealthPoint)
            {
                healthpoints[i].DeleteUI();
            }
        }     

        if (m_HealthPoint <= 0)
        {
            enemies.AllEnemiesStopAttack();
            Die();
        }
        else
        {
            // play animation
            m_HoldWeapon = m_Animator.GetBool("HoldWeapon");
            m_AudioSource_hurt.Play();

            if (attackFrom == 0)
            {
                if (!m_HoldWeapon)
                    m_Animator.Play("Hit01");
                else
                    m_Animator.Play("Fight_Hit01");
            }
            else if(attackFrom == 1)
            {
                if (!m_HoldWeapon)
                    m_Animator.Play("Hit02");
                else
                    m_Animator.Play("Fight_Hit02");
            }
        }
    }

    void Die()
    {
        m_IsDead = true;
        m_HoldWeapon = GetComponent<Animator>().GetBool("HoldWeapon");
        if (!m_HoldWeapon)
        {
            m_Animator.Play("Die");
        }
        else
        {
            m_Animator.Play("Fight_Die");
        }
        Invoke("ModelFade", 2f);
        Invoke("ShowCaughtUI", 4f);
    }

    void ModelFade()
    {
        fadeModel = new FadeModel(model);
        fadeModel.HideModel();
    }

    void ShowCaughtUI()
    {
        gameEnding.CaughtPlayer();
    }

    void FixedUpdate()
    {
        if (!m_IsDead)
        {
            // move
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            m_Animator.SetBool("IsWalking", isWalking);

            if (isWalking)
            {
                if (!m_AudioSource_step.isPlaying)
                {
                    m_AudioSource_step.Play();
                }
            }
            if(m_IsShot)
            {
                m_Animator.Play("Fight_Attack");
                m_IsShot = false;
            }
            else
            {
                m_AudioSource_step.Stop();
            }

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

            m_Rotation = Quaternion.LookRotation(desiredForward);
        }
    }

    void OnAnimatorMove()
    {
        if(!m_IsShot)
        {
            m_Rigidbody.MoveRotation(m_Rotation);
        }
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude*moveSpeed);
    }
}
