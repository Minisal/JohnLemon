using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public Player player;
    public GameObject fireEffect;
    public LineRenderer laserLine;
    public float lineEffectDuration = 0.5f;
    public float shotInterval = 1.0f;
    public float weaponRange = 100.0f;
    public Enemies enemies;
    public CursorUI cursorUI;

    public Vector3 lineRejectVector3 = new Vector3(0f, 0.3f, 0f);

    public void Fire()
    {
        Vector3 pos = muzzle.position + player.transform.forward / 2 + Vector3.up / 2;
        RaycastHit hit;
        laserLine.SetPosition(0, muzzle.position+lineRejectVector3);
        if (Physics.Raycast(pos, player.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point+lineRejectVector3);
            if (hit.collider.transform.tag == "Ghost" || hit.collider.transform.tag == "Gargoyle")
            {
                enemies.BeAttacked(hit.collider.transform.name);
                fireEffect.transform.SetPositionAndRotation(hit.collider.transform.position, fireEffect.transform.rotation);
                fireEffect.SetActive(true);
            }
        }
        else
        {
            laserLine.SetPosition(1, muzzle.position + (player.transform.forward * weaponRange)+ lineRejectVector3);
        }

        GetComponent<AudioSource>().Play();
        StartCoroutine(ShowLineEffect());
        Invoke("Reload", 0.5f);
    }

    private IEnumerator ShowLineEffect()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(lineEffectDuration);
        laserLine.enabled = false;
    }

    private void Reload()
    {
        GetComponents<AudioSource>()[1].Play();
    }
}
