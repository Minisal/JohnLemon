using UnityEngine;

public class WoodBox : MonoBehaviour
{
    public Transform rightHand;
    public Player player;
    public Transform playerTrans;

    Animator m_Animator;
    AudioSource m_OpenAudio;
    BoxCollider m_boxCollider;

    private void Start()
    {
        m_OpenAudio = GetComponent<AudioSource>();
        m_boxCollider = GetComponent<BoxCollider>();
        m_Animator = GetComponent<Animator>();
    }

    void OpenWoodBoX(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_OpenAudio.Play();
            m_Animator.SetBool("IsOpen", true);
            Destroy(m_boxCollider);

            Transform weapon = transform.Find("Healmatic500");
            weapon.parent = rightHand.parent;
            weapon.tag = "Weapon";
            weapon.localPosition = new Vector3(-0.2f, 0f, 0f);
            weapon.localRotation = Quaternion.Euler(0, 90, 90);

            player.OpenBox();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform == playerTrans)
        {
            OpenWoodBoX(other);
        }
    }
}
