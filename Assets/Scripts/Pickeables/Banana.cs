using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour {

    [SerializeField] private GameObject banana_ui;
    [SerializeField] private AudioClip sound;

    [SerializeField] private bool single = true;
    [SerializeField] private int bunchQuantity = 7;
    

    // Use this for initialization
    void Start () {

        // To get the width of an sprite
        //Debug.Log(GameObject.Find("Near_Background (0)").GetComponent<SpriteRenderer>().bounds.size.x);

    }
	
	// Update is called once per frame
	void Update () {

		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cart")
        {
            //string prefab_name = PrefabUtility.GetPrefabParent(this).name;
            
            if(!single)
            {
                float time = 0f;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                for (int i = 0; i < bunchQuantity; i++)
                {
                    StartCoroutine(SpawnBananita(time));
                    time += (float) 5 / 100;
                    
                }

                Invoke("DestroyThis", 4f);

            } else
            {
                GameObject bananita = Instantiate(banana_ui, GameObject.Find("Main Canvas").transform, false);
                bananita.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                Camera.main.GetComponent<AudioSource>().PlayOneShot(sound);
                Destroy(gameObject);
            }

            Camera.main.GetComponent<AudioSource>().PlayOneShot(sound);

        }
    }

    private IEnumerator SpawnBananita(float delay)
    {

        yield return new WaitForSeconds(delay); //Coroutine pauses for 1 second

        GameObject bananita = Instantiate(banana_ui, GameObject.Find("Main Canvas").transform, false);
        bananita.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Camera.main.GetComponent<AudioSource>().PlayOneShot(sound);

    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
