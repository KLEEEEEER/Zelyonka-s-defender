using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage = 1;
    public float range = 100f;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            switch (hit.transform.tag)
            {
                case "Enemy":
                    Enemy enemyComponent = hit.transform.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.TakeDamage(damage);
                    }
                    break;
            }

            //Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
