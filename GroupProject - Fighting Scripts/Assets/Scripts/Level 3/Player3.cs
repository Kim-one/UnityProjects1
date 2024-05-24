using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player3 : MonoBehaviour
{
    Animator animator;
    public GameObject playerAim;
    private bool isWalkingForward;
    private bool isWalkingBackward;

    //public float minDist = 1f;
    public Transform enemy;
    public float speed = 1f;
    public Rigidbody rb;
    public float moveSpeed = 2f;
    public float attackDistance = 1.5f;

    //player health variables 
    public int maxhealth = 80000;
    private int currHealth;
    public int damagePoints;
    public UnityEvent OnPlayerDeath;
    public Slider healthBar;

    public float delayBeforeNextLevel = 2f;
    public Fighting kick;
    public Fighting punch;

    /*
    public Camera cam;
    public Camera mainCamera;
    public GameObject weapon;
    */
    // Start is called before the first frame update
    void Start()
    {
        animator = playerAim.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //isWalkingForward = false;
        //isWalkingBackward = false;

        //look at opponent 
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
            transform.LookAt(enemy);
        }
        //health stuff
        Monster opponent = GameObject.FindWithTag("Enemy").GetComponent<Monster>();
        opponent.OnOpponentDeath.AddListener(OnOpponentDeathHandler);
        currHealth = maxhealth;
    }

    //reduce player health 
    public void playerDamage(int damage)
    {
        currHealth -= damage;
        healthBar.value = currHealth;
        if (currHealth <= 0)
        {
            currHealth = 0;
            Die();
            OnPlayerDeath.Invoke();
        }
    }

    //set the animation if the player dies 
    void Die()
    {
        //die animation or knockout animation
        Debug.Log("Player Died!");
        animator.SetTrigger("KnockOut");
        StartCoroutine(MoveToLevelWithDelay());
        //LoseLevel();
        //SceneConroller.instance.playerLost();
    }

    //play animation if player wins
    void OnOpponentDeathHandler()
    {
        animator.SetTrigger("Win");
        //WinLevel();
        SceneController.instance.champion();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        transform.LookAt(enemy);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
        //Player Attacks
        if (Input.GetKeyDown(KeyCode.Z))// left hand punch
        {
            attack1();
        }
        else if (Input.GetKeyDown(KeyCode.X))//right hand punch 
        {
            attack2();
        }
        else if (Input.GetKeyDown(KeyCode.K))//kick opponent
        {
            attack3();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.instance.LoadScene("Menu Scene");
        }
        
    }
    /*
    void applyDownwardforce()
    {
        rb.velocity = new Vector3(0f, -5f, 0f);
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Health")
        {
            Destroy(collision.gameObject);
            currHealth = maxhealth;
            healthBar.value = currHealth;
        }
        
    }

    void AttackRange(int damagePoints)
    {
        if((enemy.transform.position - transform.position).magnitude <= attackDistance)
        {
            attackEnemy(damagePoints);
        }
    }
    
    private void attack1()
    {
        damagePoints = 5;
        punch.GetComponent<Fighting>().PunchSound();
        animator.SetTrigger("LeftPunch");
        AttackRange(damagePoints);
    }
    void attack3()
    {

        damagePoints = 6;
        kick.GetComponent<Fighting>().KickSound();
        animator.SetTrigger("Kick");
        AttackRange(damagePoints);
    }

    void attack2()
    {
        damagePoints = 3;
        punch.GetComponent<Fighting>().PunchSound();
        animator.SetTrigger("RightPunch");
        AttackRange(damagePoints);
    }

    void attackEnemy(int damagePoints)
    {
        Monster monster = enemy.GetComponent<Monster>();
        if (monster != null)
        {
            monster.MonsterDamage(damagePoints);
        }
    }
    IEnumerator MoveToNextLevelWithDelay()
    {
        // Pause for a certain amount of time
        yield return new WaitForSeconds(delayBeforeNextLevel);

        // Move to the next level
        SceneController.instance.champion();
    }

    IEnumerator MoveToLevelWithDelay()
    {
        // Pause for a certain amount of time
        yield return new WaitForSeconds(delayBeforeNextLevel);

        // Move to the next level
        SceneController.instance.playerLost();
    }
}



