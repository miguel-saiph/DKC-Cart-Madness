using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCart : MonoBehaviour
{

    //Variables
    public int speed = 2;
    public int jumpForce = 100;
    public float weight = 0.05f;
    [SerializeField] bool canMove = false;

    //Objects
    private Rigidbody2D rb;
    private Animator anim;
    private ParticleSystem particles;
    private GameObject enemy;
    private AudioSource audio;
    [SerializeField] private AudioClip hitSound;

    //Physics
    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    public float groundRadius = .2f;
    private bool grounded;
    [SerializeField] private LayerMask whatIsGround;

    private bool isLanding = false;
    private bool isJumping = false;

    private Vector2 collisionPosition;


    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        particles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
        enemy = transform.Find("Kremlin").gameObject;
        audio = GetComponent<AudioSource>();
        groundCheck = transform.Find("GroundCheck");

        //Initial actions
        particles.Stop();
        grounded = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        grounded = false;

        if (canMove)
        {

            if(rb.bodyType == RigidbodyType2D.Dynamic)
                rb.velocity = new Vector2(speed * Time.deltaTime * transform.localScale.x, rb.velocity.y - weight);

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

            //Para que atraviese plataformas
            //Physics2D.IgnoreLayerCollision(0, 0, (grounded));
        }

    }


    void Update()
    {
        

        if (canMove)
        {

            if (!grounded)
            {
                if (isJumping)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 35 * transform.localScale.x);

                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, -35 * transform.localScale.x);
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
                }

            }
        }

        // Death by falling into the vast empty space
        if (Camera.main.WorldToViewportPoint(transform.position).y <= -1f)
        {
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cart")
        {
            gameObject.layer = 10;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //collision.gameObject.GetComponent<Cart>().Hurt(gameObject);
            
        }
        
    }

    private void LateUpdate()
    {
        // Adjust to hurted animation
        if (enemy && enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("kremlin_hurt"))
        {
            enemy.transform.position = new Vector2(collisionPosition.x, enemy.transform.position.y);
        }
    }



    // Rotation Lerp
    private IEnumerator Landing(float time)
    {

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

        if (elapsedTime >= time)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }

    }

    private void Land()
    {
        StartCoroutine(Landing(0.15f));
    }

    public void KillPassenger()
    {
        audio.PlayOneShot(hitSound);
        enemy.GetComponent<Animator>().SetTrigger("Hurt");
        collisionPosition = transform.position;
        enemy.transform.parent = null;
    }


}

