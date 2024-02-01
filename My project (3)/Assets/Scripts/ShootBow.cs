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
        int layerMask = 1 << gameObject.layer; // Get the layer of the player
        layerMask = ~layerMask; // Invert the layer mask to ignore the player's layer

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range, layerMask))
        {
            Debug.Log(hit.transform.name);

            if(hit.collider != null)
            {
                Debug.Log("Collider found on " + hit.transform.name);

                Target target = hit.transform.GetComponent<Target>();
                if(target != null)
                {
                    target.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("Target component not found on " + hit.transform.name);
                }
            }
            else
            {
                Debug.Log("No collider found on " + hit.transform.name);
            }
        }
    }
    
}