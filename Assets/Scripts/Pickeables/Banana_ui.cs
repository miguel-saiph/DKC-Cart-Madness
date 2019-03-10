using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana_ui : MonoBehaviour {

    private Vector2 _target;

    public float timeTakenDuringLerp = 1f;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    
    private float _timeStartedLerping;

    // Use this for initialization
    void Start () {

        GameObject icon = GameObject.Find("Banana_icon");

        icon.GetComponent<Banana_icon>().Show();
        _target = icon.transform.Find("Corner").transform.position;

        _timeStartedLerping = Time.time;
        _startPosition = transform.position;
        _endPosition = _target;
        
	}
	
	// Update is called once per frame
	void Update () {

        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

        transform.position = Vector2.Lerp(_startPosition, _endPosition, percentageComplete);

        if (percentageComplete >= 1.0f)
        {
            Destroy(gameObject);
        }

    }
}
