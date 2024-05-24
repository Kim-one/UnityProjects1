using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    //public Rigidbody rb;
    public Transform player;
    public float moveSpeed = 3f;
    //public string[] attackTriggers;
    public enum AIState
    {
        Idle,
        Attack,
        Chase
    }

    //health variables
    public int attackDamage;
    public int curHealth;
    public int maxOpponentHealth = 1500;
    public UnityEvent OnOpponentDeath;
    public Slider oppHealthBar;

    public Transform healthSphere;

    public NavMeshAgent agent;
    public AIState currState;
    private float lastAttackTime;
    private float attackCoolDown = 1.5f;
    private bool isAttacking = false;
    private float chaseRange = 1.6f;
    private float attackRange = 0.5f;
    //public PlayerFighting isPlayerAttacking;

    public CharacterController characterController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCoolDown;

        //healthSphere = GameObject.FindGameObjectWithTag("Health").transform;
        // player object
        PlayerFighting player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFighting>();

        // player's death event
        player1.OnPlayerDeath.AddListener(OnPlayerDeathHandler);

        //set opponent health at start ot max health
        curHealth = maxOpponentHealth;
        currState = AIState.Idle;
    }

    void Update()
    {
        transform.LookAt(player);
        // Move towards the player
        if (curHealth <= 0)
        {
            agent.enabled = false; 
            characterController.enabled = true; 
            return; 
        }
        if (curHealth < 450 && healthSphere != null)
        {
            agent.enabled = true;
            characterController.enabled = false;
            transform.LookAt(healthSphere);
            agent.SetDestination(healthSphere.transform.position);
        }
        else
        {
            if (player != null)
            {
                agent.enabled = true;
                characterController.enabled = false;
                switch (currState)
                {
                    case AIState.Idle:
                        if ((agent.transform.position - player.transform.position).magnitude > chaseRange)
                        {
                            currState = AIState.Chase;
                        }
                        break;
                    case AIState.Chase:
                        agent.SetDestination(player.transform.position);
                        if ((agent.transform.position - player.transform.position).magnitude <= attackRange)
                        {
                            //agent.SetDestination(transform.position);
                            currState = AIState.Attack;
                        }
                        break;
                    case AIState.Attack:
                        agent.SetDestination(transform.position);
                        if ((agent.transform.position - player.transform.position).magnitude > attackRange)
                        {
                            currState = AIState.Idle;
                        }
                        else
                        {
                           //agent.SetDestination(transform.position);

                            if (Time.time - lastAttackTime > attackCoolDown && !isAttacking)
                            {
                                chooseAttack();
                                lastAttackTime = Time.time;
                            }
                        }
                        break;
                }
            }
        }
    }

    void Attack1()
    {
        attackDamage = 1;
        //animator.SetTrigger("Kick");
        animator.SetBool("Kick", true);
        PlayerFighting playerFighting = player.GetComponent<PlayerFighting>();
        if (playerFighting != null)
        {
            playerFighting.playerDamage(attackDamage);
        }
    }

    void Attack2()
    {
        attackDamage = 2;
        
        animator.SetBool("RightPunch", true);
        PlayerFighting playerFighting = player.GetComponent<PlayerFighting>();
        if (playerFighting != null)
        {
            playerFighting.playerDamage(attackDamage);
        }
    }

    void Attack3()
    {
        attackDamage = 3;
        animator.SetBool("LeftPunch", true);
        PlayerFighting playerFighting = player.GetComponent<PlayerFighting>();
        if (playerFighting != null)
        {
            playerFighting.playerDamage(attackDamage);
        }
    }

    void chooseAttack()
    {
        int index = Random.Range(0, 3);

        switch (index)
        {
            case 0:
                Debug.Log("Attack 1");
                Attack1();
                break;
            case 1:
                Debug.Log("Attack 2");
                Attack2();
                break;
            case 2:
                Debug.Log("Attack 3");
                Attack3();
                break;
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Health")
        {
            Destroy(collision.gameObject);
            curHealth = maxOpponentHealth;
            oppHealthBar.value = curHealth;
        }
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Health")
        {
            curHealth = maxOpponentHealth;
            Destroy(other.gameObject);
        }
    }
    
    //play opponent win animation
    private void OnPlayerDeathHandler()
    {
        animator.SetTrigger("Win");
    }

    // reduce opponent health
    public void CPUDamage(int damage)
    {
        curHealth -= damage;
        oppHealthBar.value = curHealth;
        if (curHealth <= 0)
        {
            curHealth = 0;
            Die();
            OnOpponentDeath.Invoke();
        }
    }
    // play enemy death animation
    void Die()
    {
        agent.enabled = false;
        characterController.enabled = true;
        // knockout animation
        //rb.velocity = Vector3.zero;
        characterController.SimpleMove(Vector3.zero);
        Debug.Log("Opponent Died!");
        animator.SetTrigger("KnockOut");
    }
}
