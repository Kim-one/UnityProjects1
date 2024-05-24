using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NewBehaviourScript2 : MonoBehaviour
{
    public Light dl;
    public GameObject fire;
    public Camera cam;
    public int charge;
    private int attackDamage = 10;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        dl.enabled = false;
        fire.SetActive(false);
        charge = 50;
        mainCamera = Camera.main;
        mainCamera.enabled = true;
        //cam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H) && charge > -1)
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

                //drain the opponent's health here
                //'hit' is the gameobject
                if (hit.collider.gameObject.name == "Enemy")
                {
                    Debug.Log("Enemy Hit");
                    Enemy2 enemy = hit.collider.GetComponent<Enemy2>();
                    if (enemy != null)
                    {
                        //Debug.Log("Enemy health component found.");
                        enemy.takeDamage(attackDamage);
                    }
                    fire.transform.position = hit.transform.position;
                    fire.SetActive(true);
                }
            }
            else
            {
                fire.SetActive(false);
            }
            charge -= 1;
            if (charge < 0)
            {
                cam.enabled = false;
                mainCamera.enabled = true;
                //charge = 250;
            }
            else
            {
                dl.enabled = false;
                fire.SetActive(false);
            }
        }
    }

    void ActivateWeapon()
    {

        
    }
}
