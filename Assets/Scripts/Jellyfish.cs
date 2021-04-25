using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    private bool up;
    public Sprite[] jellyFace;
    public SpriteRenderer jellyRenderer;

    private void Start()
    {
        StartCoroutine(UpOrDown());
    }
    private void Update()
    {
        if (up)
        {
            transform.position = new Vector3(this.transform.position.x, transform.position.y + Time.deltaTime *.5f, 0);
        }
        else {
            transform.position = new Vector3(this.transform.position.x, transform.position.y - Time.deltaTime *.5f, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<AudioSource>().Play();
            jellyRenderer.sprite = jellyFace[1];
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
            StartCoroutine(SwapFaces());
        }
    }

    IEnumerator SwapFaces()
    {
        yield return new WaitForSeconds(1);
        jellyRenderer.sprite = jellyFace[0];
    }

    IEnumerator UpOrDown()
    {
        yield return new WaitForSeconds(1);
        up = !up;
        StartCoroutine(UpOrDown());
    }

}
