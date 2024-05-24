using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public int countdowntime;
    public Text countdowndisplay;

    private void Start()
    {
        StartCoroutine(CountDownToStart());
    }

    IEnumerator CountDownToStart()
    {
        while(countdowntime > 0)
        {
            countdowndisplay.text = countdowntime.ToString();

            yield return new WaitForSeconds(1f);

            countdowntime--;
        }

        countdowndisplay.text = "FIGHT!";

        SceneManager.LoadSceneAsync("Level 1");

        yield return new WaitForSeconds(1f);
        countdowndisplay.gameObject.SetActive(false);
    }
}
