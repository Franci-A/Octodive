using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    private GameObject currentLevel;
    private GameObject previousLevel;
    private float score;
    private float levelChangeValue;
    private int i;
    private bool canSpawn = false;

    private void Start()
    {
        currentLevel = Instantiate(levels[1], transform.position, transform.rotation);
        levelChangeValue = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().envChange;
        i = 1;
    }

    private void Update()
    {
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().score;
        if(!canSpawn && score >= levelChangeValue * i)
        {
            canSpawn = true;
        }

        if (canSpawn && score >= levelChangeValue && i == 1)
        {
            canSpawn = false ;
            i++;
            previousLevel = currentLevel;
            int j = Random.Range(1, levels.Length);
            currentLevel = Instantiate(levels[j], transform.position, transform.rotation);
            Debug.Log(currentLevel.name);
        }
        else if (canSpawn && score >= levelChangeValue * i && i == 7)
        {
            canSpawn = false;
            i++;
            Destroy(previousLevel);
            previousLevel = currentLevel;
            currentLevel = Instantiate(levels[0], transform.position, transform.rotation);
            Debug.Log(currentLevel.name);
        }
        else if(canSpawn && score >=levelChangeValue * i && i !=0 && i !=7)
        {
            canSpawn = false;
            i++;
            Destroy(previousLevel);
            previousLevel = currentLevel;
            int j = Random.Range(1, levels.Length);
            currentLevel = Instantiate(levels[j], transform.position, transform.rotation);
            Debug.Log(currentLevel.name);
        }
    }
}
