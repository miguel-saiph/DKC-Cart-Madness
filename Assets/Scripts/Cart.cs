using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cart : MonoBehaviour {

    //Variables
	public int speed = 2;
	public int jumpForce = 100;
    public float weight = 0.05f;
    [SerializeField] protected bool canMove = false;

    //Objects
    protected Rigidbody2D rb;
	private Animator anim;
    protected ParticleSystem particles;
    protected GameObject _enemy;
    protected AudioSource audioSource;

    //Physics
    private Transform groundCheck;    // A position marking where to check if the player is grounded.
	public float groundRadius = .2f;
	protected bool grounded;   
	[SerializeField] private LayerMask whatIsGround;

    protected bool isLanding = false;
    protected bool isJumping = false;
    protected bool isPaused = false;


    // Use this for initialization
    protected void Start () {

		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		particles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        groundCheck = transform.Find("GroundCheck");

        //Initial actions
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
            if (rb.bodyType == RigidbodyType2D.Dynamic) rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y - weight);
            

            //Para que atraviese plataformas
            //Physics2D.IgnoreLayerCollision(0, 8, (!grounded));
        }
		
	}


    void Update() {

       

	}

	void OnCollisionEnter2D(Collision2D other) {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        
    }



    // Rotation lerp
    protected IEnumerator Landing(float time)
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

    protected void Land()
    {
        StartCoroutine(Landing(0.15f));
    }

}
