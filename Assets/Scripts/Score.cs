using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("Score")]
    public TextMeshProUGUI scoreText;
    public float score;
    public float envChange;
    private int i =1;
    private PlayerController player;
    private GameObject mainCamera;
    public Color[] colors;
    private bool changeColor;
    private float timer;

    [Header("Health")]
    public Image[] healthbar;
    private int healthbarHealth;
    public Image face;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        healthbarHealth = player.currentHealth;
    }

    private void Update()
    {
        score += player.scoreUpdate;
        scoreText.text = "Deepnest : " + (int)score;

        if (score > envChange * i)
        {
            if (i < colors.Length - 1)
            {
                i++;

                changeColor = true;
                StartCoroutine(StopChangeColor());
            }
        }

        if (changeColor)
        {
            timer += Time.deltaTime;
            mainCamera.GetComponent<Camera>().backgroundColor = Color.Lerp(colors[i - 1], colors[i], timer);
        }

        if (player.currentHealth != healthbarHealth)
        {
            healthbarHealth = player.currentHealth;
            int i = 0;
            foreach (Image image in healthbar)
            {
                if( i < healthbarHealth)
                {
                    image.enabled = true;
                }
                else
                {
                    image.enabled = false;
                }
                i++;
            }
        }

        face.sprite = player.head.sprite;
    }

    IEnumerator StopChangeColor()
    {
        yield return new WaitForSeconds(1);
        changeColor = false;
    }
}
