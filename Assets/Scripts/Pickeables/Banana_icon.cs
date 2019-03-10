using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Banana_icon : MonoBehaviour {

    [SerializeField] private float timeVisible = 1.5f;
    private GameObject bananas_text;
    private int counter;

	// Use this for initialization
	void Start () {

        bananas_text = transform.Find("Bananas_text").gameObject;
        GetComponent<CanvasRenderer>().SetAlpha(0);
        bananas_text.GetComponent<CanvasRenderer>().SetAlpha(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show()
    {
        counter++;
        bananas_text.GetComponent<Text>().text = counter.ToString();

        GetComponent<CanvasRenderer>().SetAlpha(1);
        bananas_text.GetComponent<CanvasRenderer>().SetAlpha(1);

        CancelInvoke();
        Invoke("Hide", timeVisible);
    }

    private void Hide()
    {
        GetComponent<CanvasRenderer>().SetAlpha(0);
        bananas_text.GetComponent<CanvasRenderer>().SetAlpha(0);
    }
}
