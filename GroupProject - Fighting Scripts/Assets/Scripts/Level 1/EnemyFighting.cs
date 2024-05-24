using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighting : MonoBehaviour
{
    //public Rigidbody rb;
    public float speed = 1f;
    public Transform target;
    public float minDist = 1f;
    public Animator animator;
    public GameObject enemy;
    private bool iswalking;
    private bool isAttacking = false;

    //list of attacks for the enemy to randomly choose from
    private List<Action> attackMethods = new List<Action>();

    // Start is called before the first frame update
    void Start()
    {
        animator = enemy.GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();

        iswalking = false;
        if(target == null)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }
        //add attacks to list 
        attackMethods.Add(attack1);
        attackMethods.Add(attack2);
        attackMethods.Add(Uppercut);
    }

    // Update is called once per frame
    void Update()
    {
        if(target  != null)
        {
            transform.position =  Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.LookAt(target);
        }
        /*
        if(!isAttacking)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > minDist)
            {
                transform.LookAt(target);
                transform.position += transform.forward * speed * Time.deltaTime;
            }
           
        }
        */
    }


    void performAttack()
    {
        int randomIndex = UnityEngine.Random.Range(0, attackMethods.Count);
        Action action = attackMethods[randomIndex];
        action();
    }
    
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            Debug.Log("Attack Player");
            performAttack();
        }
    }
    
    /*
    void ResetAttackFlag()
    {
        isAttacking = false;
    }
    */
    void walktoPlayer()
    {
        animator.SetBool("Walk", iswalking);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    void attack1()
    {
        animator.SetTrigger("Kick");
    }
    void attack2()
    {
        animator.SetTrigger("Punch");
        //StartCoroutine(ResetAttackFlag());
    }
    void Uppercut()
    {
        animator.SetTrigger("Uppercut");
        //StartCoroutine(ResetAttackFlag());
    }
    /*
    IEnumerator ResetAttackFlag()
    {
        isAttacking = true;
        yield return new WaitForSeconds(2.5f); // Adjust time based on your animation length
        isAttacking = false;
    }
    */
}
