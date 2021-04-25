using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    private Vector3 acceleration;
    private Vector3 speed;
    public float maxSpeed;
    public float a = 5;
    public float boost = 5;
    private GameObject player;
    private Vector3 moveToPos;
    public float raduis =1;
    private bool newPositionFound = true;
    private bool agro = false;
    public float waitTime = 2;
    private float moveTime;

    private bool isAttacking;
    private float currentHealth = 4;
    private bool isDead;

    private float timer;
    private float attackDelay = 1;
    public int agroDistance = 8;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!agro)
        {
            moveToPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            Vector3 distance = moveToPos - this.transform.position;
            
            if (distance.magnitude < agroDistance)
            {
                StartCoroutine(NewPosition());
                agro = true;
                moveToPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                float angle = Vector2.SignedAngle(Vector2.right, (moveToPos - this.transform.position)) * Mathf.PI / 180;
                this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);
            }
        }

        if(currentHealth <= 0 && !isDead )
        {
            StopAllCoroutines();
            isDead = true;
            int i = Random.Range(-1, 2);
            if (i == 0)
                i = 1;
            moveToPos = new Vector3(20 * i, 0, 0);
            moveTime = (moveToPos - this.transform.position).magnitude / maxSpeed;
            float angle = Vector2.SignedAngle(Vector2.right, (moveToPos - this.transform.position)) * Mathf.PI / 180;
            this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            acceleration = direction * a;
            speed = acceleration.normalized * Time.fixedDeltaTime * 1.5f;
            moveTime = speed.magnitude * (moveToPos - this.transform.position).magnitude;
            StartCoroutine(Died());
        }
        if((this.transform.position.x < -10 || transform.position.x > 10) && !isDead)
        {
            StopAllCoroutines();
            StartCoroutine(NewPosition());
        }
    }

    private void FixedUpdate()
    {
        moveTime -= Time.deltaTime;
        speed += acceleration * Time.fixedDeltaTime;
        speed = Vector3.ClampMagnitude(speed, maxSpeed);
        this.transform.position += speed * Time.fixedDeltaTime + (acceleration * Mathf.Pow(Time.fixedDeltaTime, 2) / 2);

        if (moveTime <= 0 && !newPositionFound)
        {
            raduis = .5f;
            newPositionFound = true;
            acceleration *= -1;
            StartCoroutine(NewPosition());
        }
        if (speed.magnitude <= .1 && speed.magnitude >= -.1)
        {
            isAttacking = false;
            acceleration = new Vector3(0, 0, 0);
            speed = new Vector3(0, 0, 0);

        }
    }


    IEnumerator NewPosition()
    {
        yield return new WaitForSeconds(waitTime);
        isAttacking = true;
        moveToPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        float angle = Vector2.SignedAngle(Vector2.right, (moveToPos - this.transform.position)) * Mathf.PI / 180;
        this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);
        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        acceleration = direction.normalized * a ;
        speed = acceleration.normalized * Time.fixedDeltaTime * 1.5f;
        moveTime = (moveToPos - this.transform.position).magnitude / maxSpeed;
        newPositionFound = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && isAttacking && Time.time - timer > attackDelay)
        {
            timer = Time.time;
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ink"))
        {
            StopAllCoroutines();
            //raduis = 3;
            isAttacking = false;
            moveToPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            float angle;
            if (Vector2.Dot(player.transform.up, (this.transform.position - player.transform.position)) > 0 && Vector2.Dot(player.transform.right, (this.transform.position - player.transform.position)) < 0)
            {
                angle = (Vector2.SignedAngle(Vector2.right, (moveToPos - this.transform.position)) * Mathf.PI / 180) + .7f;
            }
            else if (Vector2.Dot(player.transform.up, (this.transform.position - player.transform.position)) < 0 && Vector2.Dot(player.transform.right, (this.transform.position - player.transform.position)) < 0)
            {
                angle = (Vector2.SignedAngle(Vector2.right, (moveToPos - this.transform.position)) * Mathf.PI / 180) - .7f;
            }
            else
            {
                angle = Vector2.SignedAngle(Vector2.right, (moveToPos - this.transform.position)) * Mathf.PI / 180;
            }
            this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            acceleration = direction.normalized * a / Mathf.Pow(direction.magnitude, 2);
            speed = acceleration.normalized * Time.fixedDeltaTime * 1f;
            moveTime = 1;
            newPositionFound = false;
            currentHealth--;
        }

    }
    

    IEnumerator Died()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
