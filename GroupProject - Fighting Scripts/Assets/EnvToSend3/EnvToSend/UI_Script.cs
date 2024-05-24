using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_Script : MonoBehaviour
{
    public GameObject canv;
    public GameObject b1;
    public GameObject b15;
    public GameObject b2;
    public GameObject b4;
    public GameObject title;
    public GameObject title2;
    public GameObject c1;
    public GameObject c2;

    // Start is called before the first frame update
    void Start()
    {

        Button btn1 = b1.GetComponent<Button>();
        btn1.onClick.AddListener(Controls);

        Button btn15 = b15.GetComponent<Button>();
        btn15.onClick.AddListener(BackToMain);

        Button btn2 = b2.GetComponent<Button>();
        btn2.onClick.AddListener(Quit);

        Button btn4 = b4.GetComponent<Button>();
        btn4.onClick.AddListener(Play);

        //GetComponent<Rec3Script>().enabled = false;

        b2.SetActive(true);
        b1.SetActive(true);
        b15.SetActive(false);
        c1.SetActive(false);
        c2.SetActive(false);
        TMP_Text text1 = title.GetComponent<TMP_Text>();
        text1.text = "Main Menu";
        canv.SetActive(true);

    }

    void Controls()
    {

        //show the second menu
        b2.SetActive(false);
        b1.SetActive(false);
        b15.SetActive(true);
        b4.SetActive(false);
        c1.SetActive(true);
        c2.SetActive(true);
        TMP_Text text1 = title.GetComponent<TMP_Text>();
        text1.text = "Controls";
          
    }
    void BackToMain()
    {
        //return to the first menu
        b2.SetActive(true);
        b1.SetActive(true);
        b15.SetActive(false);
        b4.SetActive(true);
        c1.SetActive(false);
        c2.SetActive(false);
        TMP_Text text1 = title.GetComponent<TMP_Text>();
        text1.text = "Main Menu";
    }


    void Quit()
    {
        //exit
        Debug.Log("Quit!");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canv.SetActive(true);
            b2.SetActive(true);
            b1.SetActive(true);
            b15.SetActive(false);
            b4.SetActive(true);
            c1.SetActive(false);
            c2.SetActive(false);
            //GetComponent<Rec3Script>().enabled = false;
            TMP_Text text1 = title.GetComponent<TMP_Text>();
            text1.text = "Main Menu";
        }

    }
    void Play()
    {
        SceneManager.LoadSceneAsync("Level 1");
        //canv.SetActive(false);
        //GetComponent<Rec3Script>().enabled = true;
    }
}
