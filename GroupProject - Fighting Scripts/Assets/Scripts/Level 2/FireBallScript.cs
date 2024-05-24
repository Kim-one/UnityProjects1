using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{
    public Player playerHealth;
    public int attackDamage = 100;
    private int lifespan = 2;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<Player>();
        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player hit.");
            Destroy(gameObject);
            if (playerHealth != null)
            {
                playerHealth.playerDamage(attackDamage);
                //collision.gameObject.GetComponent<Player>().playerDamage(attackDamage);

            }
            else
            {
                Debug.Log("Player not found.");
            }
        }
    }
    
}
