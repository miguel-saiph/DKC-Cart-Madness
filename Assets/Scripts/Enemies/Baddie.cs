using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour {

    [SerializeField] private bool invulnerable = false;
    [SerializeField] private AudioClip deathSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cart")
        {
            //gameObject.layer = 10;
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //collision.gameObject.GetComponent<Cart>().Hurt(gameObject);

        }

    }

    public void Death()
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 150));
        gameObject.GetComponent<Animator>().SetTrigger("Hurt");
        Camera.main.GetComponent<AudioSource>().PlayOneShot(deathSound);
        Invoke("AutoDestroy", 2f);
    }

    private void AutoDestroy()
    {
        Destroy(gameObject);
    }
}
