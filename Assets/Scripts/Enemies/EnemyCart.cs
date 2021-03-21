using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCart : Cart
{
    private Animator anim;
    private GameObject enemy;
    [SerializeField] private AudioClip hitSound;

    private Vector2 collisionPosition;

    new void Start() {
        base.Start();
        enemy = transform.Find("Kremlin").gameObject;
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

    public void KillPassenger()
    {
        audioSource.PlayOneShot(hitSound);
        enemy.GetComponent<Animator>().SetTrigger("Hurt");
        collisionPosition = transform.position;
        enemy.transform.parent = null;
    }


}

