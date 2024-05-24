using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
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

    //player health variables 
    public int maxhealth = 50;
    private int currHealth;
    public int damage = 2;
    public UnityEvent OnPlayerDeath;
    public Slider healthBar;

    public Camera cam;
    public Camera mainCamera;
    public GameObject weapon;


    internal bool isAlive;
    public float delayBeforeNextLevel = 6f;
    public NewBehaviourScript1 weapon2;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.enabled = true;
        cam.enabled = false;
        //player_rd = GetComponent<Rigidbody>();
        animator = playerAim.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        isWalkingForward = false;
        isWalkingBackward = false;
        isAlive = true;

        //look at opponent 
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
            transform.LookAt(enemy);
        }
        //health stuff
        Enemy2 opponent = GameObject.FindWithTag("Enemy").GetComponent<Enemy2>();
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
        rb.MovePosition(transform.position + movement);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.instance.LoadScene("Menu Scene");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Health")
        {
            Destroy(collision.gameObject);
            currHealth = maxhealth;
            healthBar.value = currHealth;
        }
        else if (collision.gameObject.tag == "Clock")
        {
            Debug.Log("Collision");
            Destroy(collision.gameObject);
            //rb.transform.position = new Vector3(4.24f,-1.04f, -0.88f);
            mainCamera.enabled = false;
            cam.enabled = true;

            //cam.transform.LookAt(enemy);
            //transform.LookAt(enemy);//turn player to look at enemy
            weapon.transform.LookAt(enemy);
            weapon2.resetCharge();
        }
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
            isAlive = false;
        }
    }

    //set the animation if the player dies 
    void Die()
    {
        //die animation or knockout animation
        Debug.Log("Player Died!");
        animator.SetTrigger("KnockOut");
        cam.enabled = false;
        mainCamera.enabled = true;
        StartCoroutine(MoveToLevelWithDelay());
    }

    //play animation if player wins
    void OnOpponentDeathHandler()
    {
        animator.SetTrigger("Win");
        cam.enabled = false;
        mainCamera.enabled = true;
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



