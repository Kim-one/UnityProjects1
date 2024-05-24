using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy2 : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    public Transform target; // Player's transform
    public Animator animator;
    public int maxHealth = 50;
    private int curHealth;
    public float Distance;

    public GameObject fireball;
    public Transform shootingPoint;

    private RaycastHit hitPlayer;
    public float LastTime;
    float timer;
    public UnityEvent OnOpponentDeath;

    public float detectionRange = 20f;
    public float shootingRange = 10f;
    public float shootInterval = 0.5f;
    private float lastShootTime;
    private float shootCooldownTimer = 0f;

    private Player playerScript;
    private bool playerAlive = true;
    private bool enemyAlive = true;

    List<Transform> waypoints = new List<Transform>();
    private float shootCooldown = 2f;
    public Slider healthBar;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        lastShootTime = -shootInterval;

        playerScript = target.GetComponent<Player>();

        curHealth = maxHealth;
        transform.LookAt(target);
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.OnPlayerDeath.AddListener(OnPlayerDeathHandler);

    }


    void Update()
    {
        if (playerScript != null && !playerScript.isAlive)
        {
            playerAlive = false;
            return;
        }
        // If player is dead, stop shooting
        if (!playerAlive || !enemyAlive)
            return;

        if (target != null)
        {
            // Check if the player is within detection range and shooting range
            if (Vector3.Distance(transform.transform.position, target.transform.position) <= detectionRange && Vector3.Distance(transform.transform.position, target.transform.position) <= shootingRange)
            {
                // Make the enemy look at the player
                transform.LookAt(target);

                // Shoot at the player if enough time has passed since the last shot
                if (shootCooldownTimer <= 0f)
                {
                    Shoot();
                    shootCooldownTimer = shootCooldown;
                }
                else
                {
                    shootCooldownTimer -= Time.deltaTime;
                }
            }
            else
            {
                // If player is out of range, move towards the player
                agent.SetDestination(target.transform.position);
            }
        }
    }


    void Shoot()
    {
        // Instantiate fireball and set its direction towards the player

        GameObject newFireball = Instantiate(fireball, shootingPoint.transform.position, Quaternion.identity);
        Vector3 direction = (target.transform.position - shootingPoint.transform.position).normalized;
        newFireball.GetComponent<Rigidbody>().velocity = direction * 10f;
        lastShootTime = Time.time;

    }

    public void takeDamage(int damage)
    {
        curHealth -= damage;
        healthBar.value = curHealth;
        if (curHealth <= 0)
        {
            Die();
            OnOpponentDeath.Invoke();
        }
    }

    void Die()
    {
        enemyAlive = false;
        animator.SetTrigger("KnockOut");
    }

    void OnPlayerDeathHandler()
    {
        animator.SetTrigger("Win");
        //cam.enabled = false;
        //mainCamera.enabled = true;
    }
}
