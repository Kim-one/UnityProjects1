using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NewBehaviourScript1 : MonoBehaviour
{
    public Light dl;
    public GameObject fire;
    public Camera cam;
    public int charge;
    private int initialCharge = 100;
    private int attackDamage = 10;
    public Camera camera1;

    // Start is called before the first frame update
    void Start()
    {
        dl.enabled = false;
        fire.SetActive(false);
        charge = initialCharge;
        camera1 = Camera.main;
        camera1.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //int layermask = ~LayerMask.GetMask("Weapon");

        if (Input.GetKey(KeyCode.H)&&charge>-1)
        {
            //using the weapon
            dl.enabled = true;
            //cast a ray to see what's in front
            Vector3 forward = cam.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, forward, out hit, 1000f))
            {
                //fire.transform.position = hit.transform.position;
                //fire.SetActive(true);
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                //drain the opponent's health here
                //'hit' is the gameobject
                Enemy2 enemy = hit.collider.GetComponent<Enemy2>();
                if(enemy != null)
                {
                    //Debug.Log("Enemy health component found.");
                    enemy.takeDamage(attackDamage);
                }
                fire.transform.position = hit.point;
                fire.SetActive(true);
            }
            else
            {
                fire.SetActive(false);
            }
            charge -= 1;
            if(charge < 0)
            {
                cam.enabled = false;
                camera1.enabled = true;
            }
        }
        else
        {
            dl.enabled = false;
            fire.SetActive(false);
        }
    }

    public void resetCharge()
    {
        charge = initialCharge;
    }
}
