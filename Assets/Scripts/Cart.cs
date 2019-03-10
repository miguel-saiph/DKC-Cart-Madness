using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cart : MonoBehaviour {

    //Variables
	public int speed = 2;
	public int jumpForce = 100;
    public float weight = 0.05f;
    [SerializeField] bool canMove = false;

    //Objects
    private Rigidbody2D rb;
	private Animator anim;
    private ParticleSystem particles;
    private GameObject donkey;
    private GameObject diddy;
    private GameObject _enemy;
    private AudioSource audio;

    //Physics
    private Transform groundCheck;    // A position marking where to check if the player is grounded.
	public float groundRadius = .2f;
	private bool grounded;   
	[SerializeField] private LayerMask whatIsGround;

    private bool isLanding = false;
    private bool isJumping = false;
    private bool killingCooldown = false;
    private bool donkeyIsInCar = true;
    private bool diddyIsInCar = true;
    private bool isPaused = false;


    // Use this for initialization
    void Start () {

		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		particles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
        donkey = GameObject.Find("Donkey");
        diddy = GameObject.Find("Diddy");
        audio = GetComponent<AudioSource>();
        groundCheck = transform.Find("GroundCheck");

        //Initial actions
        donkey.GetComponentInChildren<SpriteRenderer>().enabled = false;
        diddy.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        particles.Stop();
        grounded = true;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        grounded = false;

        if (canMove)
        {
            // Rotation adjusts
            if (transform.rotation.z > 0.4)
            {
                transform.rotation = Quaternion.Euler(0, 0, 39);
                //Debug.Log(transform.rotation.z);
            }

            if (transform.rotation.z < -0.4)
            {
                transform.rotation = Quaternion.Euler(0, 0, -39);
                //Debug.Log(transform.rotation.z);
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    grounded = true;
            }

            // Main movement
            //float extraSpeed = 1;
            //if (grounded && transform.rotation.z >= 0.2) extraSpeed = 0.8f;
            if (grounded && transform.rotation.z <= -0.2) rb.AddForce(new Vector2(100, rb.velocity.y - weight));
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y - weight);
            

            //Para que atraviese plataformas
            //Physics2D.IgnoreLayerCollision(0, 8, (!grounded));
        }
		
	}


    void Update() {
        
        // To avoid weird angle hurting jumps
        if (donkey.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            donkey.transform.localRotation = new Quaternion(0,0,0,0);
        }

        if (canMove)
        {

            if (Input.GetButtonDown("Jump") && grounded)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                //anim.SetTrigger("Jump");
                isJumping = true;
            }
            
            if (!grounded)
            {
                if (isJumping)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 35);

                } else
                {
                    transform.rotation = Quaternion.Euler(0, 0, -35);
                }
                
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                particles.Stop();
                isLanding = true;
                CancelInvoke("Land");
                StopCoroutine("Landing");

            }
            else
            {
                particles.Play();
                
                //rb.constraints = RigidbodyConstraints2D.None;

                if (isLanding)
                {
                    StartCoroutine(Landing(0.15f));
                    isLanding = false;
                    isJumping = false;
                    audio.Play();
                }
                
            }
        }

        // Death by falling into the vast empty space
        if(Camera.main.WorldToViewportPoint(transform.position).y <= -1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // X_X
        }

	}

	void OnCollisionEnter2D(Collision2D other) {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Starting sequence
        if (collision.tag == "Player")
        {
            donkey.GetComponentInChildren<SpriteRenderer>().enabled = true;
            diddy.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            canMove = true;

            Camera.main.GetComponent<CustomCamera2D>().enabled = true;
            Camera.main.GetComponent<CustomCamera2D>().Initialize();

            Destroy(collision.gameObject);

            grounded = true;
            
        }

        if (collision.tag == "Enemy" && !grounded)
        {
            
            if(GetComponent<BoxCollider2D>().IsTouching(collision) && !killingCooldown)
            {
                rb.AddForce(new Vector2(0f, jumpForce * 1.5f));
                killingCooldown = true;
                Invoke("ResetKillingCooldown", 1f);
                collision.GetComponentInParent<EnemyCart>().KillPassenger();
                //anim.SetTrigger("Jump");
                //isJumping = true;

                /*
                ContactPoint2D[] contacts = new ContactPoint2D[10];
                collision.GetContacts(contacts);
                foreach (var item in contacts)
                {
                    Debug.Log(item.otherCollider);
                }
                */
            }
            

        }


    }



    // Lerp de rotación
    private IEnumerator Landing(float time)
    {
        while (isPaused)
        {
            yield return null;
        }

        float elapsedTime = 0;
        Quaternion startingRotation = transform.rotation;
        Quaternion finalRotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);

        //Debug.Log(transform.rotation);
        while (elapsedTime < time & startingRotation != finalRotation)
        {
            transform.rotation = Quaternion.Lerp(startingRotation, finalRotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if(elapsedTime >= time)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        
    }

    private void Land()
    {
        StartCoroutine(Landing(0.15f));
    }

    private void ResetKillingCooldown()
    {
        killingCooldown = false;
    }

    public void Hurt(GameObject enemy)
    {
        isPaused = true;
        rb.bodyType = RigidbodyType2D.Static;
        _enemy = enemy;

        if (GameManager.gm.DonkeyPos == 1)
        {
            donkey.transform.parent = null;
            donkey.transform.rotation = new Quaternion(0, 0, 0, 0);
            donkey.GetComponentInChildren<Animator>().SetTrigger("Hurt");
            donkey.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            donkey.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200f));
        }
            
        else
        {
            diddy.transform.parent = null;
            diddy.transform.rotation = new Quaternion(0, 0, 0, 0);
            diddy.GetComponentInChildren<Animator>().SetTrigger("Hurt");
            diddy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            diddy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200f));
        }

        Invoke("EndHurt", 1.2f);

    }

    // Called from player's death animation
    public void EndHurt()
    {
        isPaused = false;
        if(GameManager.gm.DonkeyPos == 1)
        {
            // Diddy takes donkey's place
            donkey.GetComponentInChildren<SpriteRenderer>().enabled = false;
            
            GameManager.gm.DiddyPos = 1;
            GameManager.gm.DonkeyPos = 0;

            GameManager.gm.PositionMonkeys();

            donkey.gameObject.SetActive(false);

            // Get things back to normal
            _enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            rb.bodyType = RigidbodyType2D.Dynamic;

        } else if(GameManager.gm.DonkeyPos == 2 || GameManager.gm.DonkeyPos == 0)
        {
            // Diddy dies, everything dies
            GameManager.gm.DiddyPos = 0;
            diddy.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            GameManager.gm.EndGame();
        }
        
    }


}
