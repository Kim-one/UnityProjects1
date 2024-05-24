using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startLevel();
        }

    }
    void startLevel()
    {

        SceneManager.LoadSceneAsync("Level 1");

    }
}
