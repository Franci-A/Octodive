using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkAttack : MonoBehaviour
{

    [SerializeField] private ParticleSystem inkParticles;
    [SerializeField] private PolygonCollider2D inkCollider;
    [SerializeField] private float maxInk;
    [SerializeField] private float inkMultiplier;
    [SerializeField] private Slider inkSlider;
    private float currentInk;
    private PlayerController controller;
    private bool isAttacking;

    private float delay;
    public AudioClip squart;


    void Start()
    {
        controller = GetComponent<PlayerController>();

        currentInk = maxInk;
    }

    // Update is called once per frame
    void Update()
    {
        // ink attack----------------------------
        if (Input.GetMouseButtonDown(1) && currentInk > 0)
        {
            GetComponent<AudioSource>().clip = squart;
            GetComponent<AudioSource>().Play();
            isAttacking = true;
            inkCollider.enabled = true;
            controller.head.sprite = controller.faces[1];
            inkParticles.Play();
        }
        else if ((Input.GetMouseButtonUp(1) || currentInk <= 0) && isAttacking)
        {
            isAttacking = false;
            inkCollider.enabled = false;
            controller.head.sprite = controller.faces[0];
            inkParticles.Stop();
            delay = Time.time;
        }
        else if (Time.time - delay > 1 && !isAttacking)
        {
            currentInk += Time.deltaTime * inkMultiplier;
            if(currentInk > maxInk)
            {
                currentInk = maxInk;
            }
            
        }
        if(isAttacking)
        {
            currentInk -= Time.deltaTime * inkMultiplier;
        }

        inkSlider.value = currentInk;
    }
}
