using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;
    private PlayerController player;
    public float minPos;
    public float maxPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        foreach (GameObject background in backgrounds)
        {
            if (background != null && Time.timeScale != 0)
            {
                background.transform.position += new Vector3(0, player.scoreUpdate * .35f, 0);
                if (background.transform.position.y > maxPos)
                {
                    background.transform.position = new Vector3(0, minPos, 0);
                }
            }
        }

    }
}
