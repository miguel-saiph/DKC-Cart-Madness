using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necky : Baddie {

    [SerializeField] private GameObject rockyPrefab;

    [SerializeField] private float impulseX;
    [SerializeField] private float impulseY;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Cart").transform;
    }

	private void InstanceRock()
    {
        if (player.position.x <= transform.position.x) impulseX *= -1;
        else impulseX = Mathf.Abs(impulseX);

        GameObject rocky = Instantiate(rockyPrefab, transform.position, Quaternion.identity, transform);
        Rigidbody2D rb = rocky.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(impulseX, impulseY));

    }
}
