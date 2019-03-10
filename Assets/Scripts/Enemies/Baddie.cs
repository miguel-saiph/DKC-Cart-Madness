using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour {

    [SerializeField] private bool invulnerable = false;

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
        Debug.Log("Morí");
    }
}
