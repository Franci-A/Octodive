using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float a = 5;
    public bool isCharging;
    private Vector3 acceleration;
    private Vector3 speed;
    private float boost;
    private float boostTimer;
    [SerializeField] private float maxBoost;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float boostSpeed;
    private float lastBoostUsed;

    [Header("Attack")]
    [SerializeField] private ParticleSystem bubbleParticles;
    public Sprite[] faces;
    public SpriteRenderer head;


    [Header("Health")]
    public int currentHealth;
    [SerializeField] private int maxHealth;
    private float gameTimer;
    public float scoreUpdate;
    private Animator myAnimator;
    private float invincibilityTime;

    [Header("StarFish Boost")]
    public float starfishTime;
    public int currentStarfishNum;
    public int starfishNeeded;
    public Image star;
    private bool starfishDash;

    [Header("Sound")]
    public AudioClip[] soundEffects;
    private AudioSource audioSource;

    private void Start()
    {
        star.fillAmount = currentStarfishNum * .2f;
        myAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
        invincibilityTime = 1.5f;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        invincibilityTime -= Time.deltaTime;

        Movement();

        if (starfishDash)
        {
            Vector3 worldPosCursor = new Vector3(0, 10,0);
            float angle = Vector2.SignedAngle(Vector2.right, (worldPosCursor - this.transform.position)) * Mathf.PI / 180;
            this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);

            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            acceleration = direction.normalized * a / Mathf.Pow(direction.magnitude, 2);

            audioSource.clip = soundEffects[2];
            audioSource.Play();
            speed -= acceleration * Time.fixedDeltaTime;
            speed = Vector3.ClampMagnitude(speed, maxSpeed);
        }

        //clamped doerders--------------------------
        if (Camera.main.WorldToScreenPoint(this.transform.position).x < -10 )
        {
            this.transform.position = new Vector3((Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0,0))).x -.2f, this.transform.position.y, this.transform.position.z);
        }
        else if(Camera.main.WorldToScreenPoint(this.transform.position).x > Screen.width +10)
        {
            this.transform.position = new Vector3((Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0))).x + .2f, this.transform.position.y, this.transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            if ((speed.magnitude <= .2 && speed.magnitude >= -.2))
            {
                if (head.sprite == faces[2])
                    head.sprite = faces[0];
                acceleration = new Vector3(0, 0, 0);
                speed = new Vector3(0, 0, 0);
            }

            speed -= acceleration * Time.fixedDeltaTime;
            speed = Vector3.ClampMagnitude(speed, maxSpeed);
            this.transform.position += speed * Time.fixedDeltaTime + (acceleration * Mathf.Pow(Time.fixedDeltaTime, 2) / 2);

            if (this.transform.position.y < 0)
            {
                scoreUpdate = (speed.y * Time.fixedDeltaTime + (acceleration.y * Mathf.Pow(Time.fixedDeltaTime, 2) / 2)) * -1;
                this.transform.position = new Vector3(this.transform.position.x, 0, 0);
            }
            else
            {
                scoreUpdate = 0;
            }

            if (this.transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y)
            {
                this.transform.position = new Vector3(this.transform.position.x, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y, 0);
            }
        }
    }


    private void Movement()
    {
        if (Time.timeScale != 0)
        {
            Vector3 worldPosCursor = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            float angle = Vector2.SignedAngle(Vector2.right, (worldPosCursor - this.transform.position)) * Mathf.PI / 180;
            this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);


            if (Input.GetMouseButtonDown(0) && gameTimer - lastBoostUsed > .2f)
            {
                isCharging = true;
            }


            if (isCharging && boostTimer < maxBoost)
            {
                boostTimer += Time.deltaTime;
                myAnimator.SetTrigger("Charge");
            }
            if (Input.GetMouseButtonUp(0))
            {
                lastBoostUsed = gameTimer;
                myAnimator.SetTrigger("Boost");
                head.sprite = faces[2];
                audioSource.clip = soundEffects[4];
                audioSource.Play();
                Instantiate(bubbleParticles, this.transform.position, this.transform.rotation);
                boost = boostTimer * boostSpeed;
                boostTimer = 0;
                Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
                acceleration = direction.normalized * a / Mathf.Pow(direction.magnitude, 2);
                speed = new Vector3(direction.x * boost, direction.y * boost, 0);
                boost = 0;
                isCharging = false;
                myAnimator.ResetTrigger("Charge");
            }
        }
    }

    public void TakeDamage()
    {
        if (invincibilityTime <= 0 && !starfishDash)
        {
            invincibilityTime = 1.5f;
            head.sprite = faces[3];
            currentHealth--;
            audioSource.clip = soundEffects[3];
            audioSource.Play();
            if (currentHealth <= 0)
            {
                speed = new Vector3(0,0,0);
                acceleration = new Vector3(0,0,0);
                int i = (int)GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().score;
                
                GameObject.Find("GameManager").GetComponent<GameManager>().GameOver(i);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Starfish"))
        {
            Destroy(collision.gameObject);
            audioSource.clip = soundEffects[1];
            audioSource.Play();
            currentStarfishNum++;
            star.fillAmount = currentStarfishNum * .2f;
            if (currentStarfishNum >= starfishNeeded)
            {
                head.sprite = faces[4];
                starfishDash = true;
                currentStarfishNum -= starfishNeeded;
                star.fillAmount = currentStarfishNum * .2f;
                Debug.Log(currentStarfishNum);
                boost = maxBoost;
                
                StartCoroutine(StarfishDashTime());
            }
           
        }else if (collision.CompareTag("Health") && currentHealth < maxHealth)
        {
            Destroy(collision.gameObject);
            audioSource.clip = soundEffects[1];
            audioSource.Play();
            currentHealth++;
        }
    }

    IEnumerator StarfishDashTime()
    {
        yield return new WaitForSeconds(starfishTime);
        starfishDash = false;
        head.sprite = faces[0];
        acceleration = new Vector3(0, 0, 0);
        speed = new Vector3(0, 0, 0);
    }
}
