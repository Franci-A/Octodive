using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFish : MonoBehaviour
{
    public float speed;


    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

        if ((transform.position.x > 7 && speed > 0) || (transform.position.x < -7 && speed < 0))
        {
            speed *= -1;

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }
}
