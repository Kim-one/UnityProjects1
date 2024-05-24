using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLoseUI : MonoBehaviour
{
    private int level;
    public GameObject player;
    public GameObject enemy1;
    //public GameObject enemy2;
    //public GameObject enemy3;
    public GameObject canv;
    public GameObject t1;
    public GameObject b1;
    public GameObject b2;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        Button btn1 = b1.GetComponent<Button>();
        btn1.onClick.AddListener(NextLevel);
        Button btn2 = b1.GetComponent<Button>();
        btn2.onClick.AddListener(Retry);

    }

    // Update is called once per frame
    void Update()
    {
        if (level == 1)
        {
            Animator anim = enemy1.GetComponent<Animator>();
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("KnockOut"))
            {
                canv.SetActive(true);
            }
            Animator anim2 = player.GetComponent<Animator>();
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("KnockOut"))
            {
                Lose();
            }
        }
        /*
        else if (level == 2) {
            Animator anim = enemy2.GetComponent<Animator>();
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Knockout"))
            {
                Win();
            }
            Animator anim2 = player.GetComponent<Animator>();
            if (anim2.GetCurrentAnimatorStateInfo(0).IsName("Knockout"))
            {
                Lose();

            }
        }
        else
        {
            Animator anim = enemy3.GetComponent<Animator>();
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                Win();
            }
            Animator anim2 = player.GetComponent<Animator>();
            if (anim2.GetCurrentAnimatorStateInfo(0).IsName("Knockout"))
            {
                Lose();
            }
        }
        */
    }

    void Lose()
    {
        b2.SetActive(true);
        b1.SetActive(false);
        canv.SetActive(true);
        TMP_Text text1 = t1.GetComponent<TMP_Text>();
        text1.text = "GAME OVER";
    }
    void Win()
    {
        b2.SetActive(false);
        b1.SetActive(true);
        canv.SetActive(true);
        TMP_Text text1 = t1.GetComponent<TMP_Text>();
        text1.text = "YOU WIN!";
    }
    void NextLevel()
    {
        level += 1;
        //you can put changing the active enemy here if you want
        if (level == 2)
        {
            enemy1.SetActive(false);
            //enemy2.SetActive(true);
            //you can add whatever else you need for level 2
        }
        else
        {
            //enemy2.SetActive(false);
            //enemy3.SetActive(true);
            //you can add whatever else you need for level 3

        }
    }
    void Retry()
    {
        level = 1;
        // return to level 1
        if (level == 2)
        {
            //enemy2.SetActive(false);
        }
        else if (level == 3)
        {
            //enemy3.SetActive(false);
        }
        else
        {
            //failed level 1; reset enemy1 components here
        }
        enemy1.SetActive(true);
    }
}
