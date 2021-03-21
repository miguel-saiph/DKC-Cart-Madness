using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCart : Cart
{
    private GameObject donkey;
    private GameObject diddy;
    [SerializeField] private AudioClip donkeyHit;
    [SerializeField] private AudioClip diddyHit;
    private bool donkeyIsInCar = true;
    private bool diddyIsInCar = true;
    private bool killingCooldown = false;

    new void Start() {

        base.Start();
        donkey = GameObject.Find("Donkey");
        diddy = GameObject.Find("Diddy");

        // Initial actions
        donkey.GetComponentInChildren<SpriteRenderer>().enabled = canMove;
        diddy.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = canMove;
    }

    // Update is called once per frame
    void Update()
    {
        // Input
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
                    audioSource.Play();
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

    private void OnTriggerEnter2D(Collider2D collision) {
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
                if (collision.GetComponentInParent<EnemyCart>())
                    collision.GetComponentInParent<EnemyCart>().KillPassenger();
                else
                    collision.GetComponent<Baddie>().Death();
            }
            
        }

        if (collision.tag == "Enemy" && grounded)
        {
            collision.gameObject.layer = 10;
            Hurt(collision.gameObject);
        }
        if (collision.tag == "Bad")
        {
            collision.gameObject.layer = 10;
            Hurt(collision.gameObject);
        }
    }

    public void Hurt(GameObject enemy)
    {
        isPaused = true;
        rb.bodyType = RigidbodyType2D.Static;
        _enemy = enemy;

        if (GameManager.gm.DonkeyPos == 1)
        {
            audioSource.PlayOneShot(donkeyHit);
            donkey.transform.parent = null;
            donkey.transform.rotation = new Quaternion(0, 0, 0, 0);
            donkey.GetComponentInChildren<Animator>().SetTrigger("Hurt");
            donkey.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            donkey.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200f));
        }
            
        else
        {
            audioSource.PlayOneShot(diddyHit);
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
            if (_enemy.GetType() == typeof(EnemyCart))
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
    private void ResetKillingCooldown()
    {
        killingCooldown = false;
    }
}
