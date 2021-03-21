using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RollingStart : MonoBehaviour {

    [SerializeField] private int impulseX;
    [SerializeField] private int impulseY;
    private Rigidbody2D rb;


    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(impulseX, impulseY));

    }
}
