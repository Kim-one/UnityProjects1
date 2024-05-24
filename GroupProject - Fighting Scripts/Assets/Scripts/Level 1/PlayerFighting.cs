using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerFighting : MonoBehaviour
{
    public Animator animator;
    //public GameObject playerAim;
    private bool isWalkingForward;
    private bool isWalkingBackward;

    //public float minDist = 1f;
    public Transform enemy;
    public float speed = 1f;
    public Rigidbody rb;
    public float moveSpeed = 2f;

    //player health variables 
    public int maxhealth = 50;
    public int currHealth;
    public int damagePoints;
    public UnityEvent OnPlayerDeath;
    public Slider healthBar;

    //private CharacterController characterController;
    private float attackRange = 1.5f;
    public float delayBeforeNextLevel = 10f;
    public Fighting kick;
    public Fighting punch;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //isWalkingForward = false;
        //isWalkingBackward = false;
        
        //look at opponent 
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
            transform.LookAt(enemy);
        }
        //health stuff
        NewBehaviourScript opponent = GameObject.FindWithTag("Enemy").GetComponent<NewBehaviourScript>();
        opponent.OnOpponentDeath.AddListener(OnOpponentDeathHandler);
        currHealth = maxhealth;
    }
    // Update is called once per frame
    void Update()
    {
        // Movement controls
        transform.LookAt(enemy);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;
        //animator.SetFloat("MoveBackwards", Mathf.Abs(vertical) + Mathf.Abs(horizontal));
        rb.MovePosition(transform.position + movement);

        //player attacks
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
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.instance.LoadScene("Menu Scene");
        }
        //end of player attacks
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Health")
        {
            Destroy(collision.gameObject);
            currHealth = maxhealth;
            healthBar.value = currHealth;
        } 
    }
    private void attack1()
    {
        damagePoints = 1;
        animator.SetTrigger("LeftPunch");
        inRange(damagePoints);
        punch.GetComponent<Fighting>().PunchSound();
    }
    void attack2()
    {
        damagePoints = 10;
        animator.SetTrigger("RightPunch");
        inRange(damagePoints);
        punch.GetComponent<Fighting>().PunchSound();
    }
    void attack3()
    {
        damagePoints = 20;
        animator.SetTrigger("Kick");
        inRange(damagePoints);
        punch.GetComponent<Fighting>().KickSound();
    }

    void attackEnemy(int damagePoints)
    {
        NewBehaviourScript enemy1 = enemy.GetComponent<NewBehaviourScript>();
        if(enemy1 != null)
        {
            enemy1.CPUDamage(damagePoints);
        }
    }

    void inRange(int damage)
    {
        if((enemy.transform.position - transform.position).magnitude <= attackRange)
        {
            attackEnemy(damage);
        }
    }
    
    void moveForward()
    {
        isWalkingForward = true;
        animator.SetBool("MoveForward", isWalkingForward);
    }
    void movebackwards()
    {
        isWalkingBackward=true;
        animator.SetBool("MoveBackwards", isWalkingBackward);
        //transform.Translate(Vector3.back * speed * backwardSpeedMultiplier * Time.deltaTime);
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
        //SceneConroller.instance.playerLost();
        //levelManager.GameOver();
        //LoseLevel();
    }

    //play animation if player wins
    void OnOpponentDeathHandler()
    {
        animator.SetTrigger("Win");
        //go to next level
        StartCoroutine(MoveToNextLevelWithDelay());
        
    }

    IEnumerator MoveToNextLevelWithDelay()
    {
        // Pause for a certain amount of time
        yield return new WaitForSeconds(delayBeforeNextLevel);

        // Move to the next level
        SceneController.instance.NextLevel();
    }

    IEnumerator MoveToLevelWithDelay()
    {
        // Pause for a certain amount of time
        yield return new WaitForSeconds(delayBeforeNextLevel);

        // Move to the next level
        SceneController.instance.playerLost();
    }
}
