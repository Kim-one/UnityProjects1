using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public Transform target;
    public NavMeshAgent agent;

    //health variables
    public int maxHealth = 600;
    public int currentHealth;
    public Slider MonsterHealthBar;

    //attack and chase variables
    private float attackDistance = 1.5f;
    private float chaseDistance = 4f;
    private float attackCoolDown = 0.5f;
    private float lastAttackTime;
    private bool isAttacking = false;
    public UnityEvent OnOpponentDeath;
    private Transform[] healthObject;

    public Monster1 monster1;

    public enum AIState
    {
        Idle,
        Attack,
        Chase
    }
    public AIState currentState;
    private bool seekingHealth = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = AIState.Idle;
        lastAttackTime = -attackCoolDown;
        Player3 player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player3>();
        player.OnPlayerDeath.AddListener(OnPlayerDeathHandler);
        currentHealth = maxHealth;

        GameObject[] healthObjects = GameObject.FindGameObjectsWithTag("Health");

        healthObject = new Transform[healthObjects.Length];
        for(int i = 0; i < healthObjects.Length; i++)
        {
            healthObject[i] = healthObjects[i].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        if(!seekingHealth && currentHealth < 500 && healthObject.Length > 0)
        {
            Transform randomHealthObject = healthObject[Random.Range(0, healthObject.Length)];
            agent.transform.LookAt(randomHealthObject);
            agent.SetDestination(randomHealthObject.position);
            seekingHealth = true;
        }
        else
        {
            if (target != null)
            {
                switch (currentState)
                {
                    case AIState.Idle:
                        if ((agent.transform.position - target.transform.position).magnitude > chaseDistance)
                        {
                            //agent.SetDestination(target.transform.position);
                            currentState = AIState.Chase;
                        }
                        break;
                    case AIState.Chase:
                        animator.SetBool("Run", true);
                        agent.SetDestination(target.transform.position);

                        if ((target.transform.position - agent.transform.position).magnitude <= attackDistance)
                        {
                            animator.SetBool("Run", false);
                            currentState = AIState.Attack;
                        }
                        break;
                    case AIState.Attack:
                        if ((target.transform.position - agent.transform.position).magnitude > attackDistance)
                        {
                            currentState = AIState.Idle;
                        }
                        else
                        {
                            if (Time.time - lastAttackTime > attackCoolDown && !isAttacking)
                            {
                                chooseAttack();
                            }
                        }
                        break;
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Health")
        {
            currentHealth = maxHealth;
            Destroy(other.gameObject);
            seekingHealth = false;
        }
    }
    
    void Chase()
    {
        Debug.Log("Run");
        animator.SetTrigger("Running");
        agent.SetDestination(new Vector3(2.3f, 3.5f, 3.5f));
    }

    void chooseAttack()
    {
        int attacknum = Random.Range(0, 4);

        switch(attacknum)
        {
            case 0:
                Debug.Log("Attack1");
                AttackAction1();
                break;
            case 1:
                Debug.Log("Attack2");
                AttackAction2();
                break;
            case 2:
                Debug.Log("Attack3");
                AttackAction3();
                break;
        }
    }

    void AttackAction1()
    {
        int attackDamage = 1;
        animator.SetBool("Attack1", true);
        Player3 playerHealth = target.GetComponent<Player3>();
        if (playerHealth != null)
        {
            playerHealth.playerDamage(attackDamage);
            //Debug.Log("Attack 1 damage points: " + attackDamage);
        }
    }

    void AttackAction2()
    {
        int attackDamage = 2;
        animator.SetBool("Attack2", true);
        Player3 playerHealth = target.GetComponent<Player3>();
        if (playerHealth != null)
        {
            playerHealth.playerDamage(attackDamage);
            //Debug.Log("Attack 2 damage points: " + attackDamage);
        }
    }

    void AttackAction3()
    {
        int attackDamage = 3;
        animator.SetBool("Attack3", true);
        Player3 playerHealth = target.GetComponent<Player3>();
        if (playerHealth != null)
        {
            playerHealth.playerDamage(attackDamage);
            //Debug.Log("Attack 3 damage points: " + attackDamage);
        }
    }

    public void MonsterDamage(int damagePoints)
    {
        currentHealth -= damagePoints;
        MonsterHealthBar.value = currentHealth;
        if(currentHealth <= 0)
        {
            Die();
            OnOpponentDeath.Invoke();
        }
    }

    void Die()
    {
        //animator.SetBool("Die", true);
        animator.SetTrigger("Die");
    }

    void OnPlayerDeathHandler()
    {
        animator.SetTrigger("Howl");
        monster1.GetComponent<Monster1>().roar();
    }
}
