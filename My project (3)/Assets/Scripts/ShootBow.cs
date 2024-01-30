using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBow : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera playerCamera;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Raycast to see if we hit anything
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);


            Target target = hit.transform.GetComponent<Target>();
            hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

        }
    }

    
}