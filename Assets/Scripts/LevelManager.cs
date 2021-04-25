using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    private GameObject currentLevel;
    private GameObject previousLevel;
    private float score;
    private int i;

    private void Start()
    {
        currentLevel = Instantiate(levels[1], transform.position, transform.rotation);
        
    }

    private void Update()
    {
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().score;
        if (score >= 250 && i == 0)
        {
            i++;
            previousLevel = currentLevel;
            int j = Random.Range(1, levels.Length);
            currentLevel = Instantiate(levels[j], transform.position, transform.rotation);
            Debug.Log(currentLevel.name);
        }
        else if (score >= 250 * i && i == 1)
        {
            i++;
            Destroy(previousLevel);
            previousLevel = currentLevel;
            currentLevel = Instantiate(levels[0], transform.position, transform.rotation);
            Debug.Log(currentLevel.name);
        }
        else if(score >= 250 *i && i !=0)
        {
            i++;
            Destroy(previousLevel);
            previousLevel = currentLevel;
            int j = Random.Range(1, levels.Length);
            currentLevel = Instantiate(levels[j], transform.position, transform.rotation);
            Debug.Log(currentLevel.name);
        }
    }
}
