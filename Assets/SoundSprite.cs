using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSprite : MonoBehaviour
{
    public Image image;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            image.enabled = true;
        }
        else if(PlayerPrefs.GetInt("Sound") == 0)
        {
            image.enabled = false;
        }
    }
}
