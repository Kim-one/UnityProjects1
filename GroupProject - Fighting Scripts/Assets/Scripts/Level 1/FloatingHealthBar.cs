using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Transform player;
    public RectTransform healthBar;

    public RectTransform enemyHealthBar;
    public Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);

            // Adjust position to be above the player's head
            Vector3 offset = new Vector3(0, 120, 0); 
            healthBar.position = screenPos + offset;
        }
        if(enemy != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(enemy.position);
            Vector3 offset = new Vector3(10, 120, 0);
            enemyHealthBar.position = pos + offset;
        }
        
    }
}
