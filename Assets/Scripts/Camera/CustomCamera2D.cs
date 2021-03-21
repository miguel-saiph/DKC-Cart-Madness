using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera2D : MonoBehaviour {

	public Transform target;

	public float xOffset = 0f;
	public float yOffset = 0f;

	public float xLimit = 9;
	public float yLimit = 6;

	public bool isXLocked = false;
	public bool isYLocked = false;

	private float cameraSize;
    private bool init = false;

    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    public float timeTakenDuringLerp = 1f;

    //Whether we are currently interpolating or not
    private bool _isLerping;

    //The start and finish positions for the interpolation
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;

    void OnEnable () {

		cameraSize = Camera.main.orthographicSize;
        
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPosition = transform.position;


    }
	
	// Update is called once per frame
	void Update () {

        if (init)
        {
            _endPosition = new Vector3(target.position.x + xOffset, 0, transform.position.z);
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                init = false;
            }

        } else
        {
            float xTarget = target.position.x + xOffset;
            float yTarget = target.position.y + yOffset;

            //Locking in the limits
            if (Mathf.Abs(xTarget) >= xLimit - cameraSize * 2)
            {
                isXLocked = true;
            }
            else
            {
                isXLocked = false;
            }

            if (Mathf.Abs(yTarget) >= yLimit - 3)
            {
                isYLocked = true;
            }
            else
            {
                isYLocked = false;
            }

            float xNew = transform.position.x;
            if (!isXLocked)
            {
                xNew = xTarget;
            }

            float yNew = transform.position.y;
            if (!isYLocked)
            {
                yNew = yTarget;
            }

            transform.position = new Vector3(xNew, yNew, transform.position.z);
        }
        
	}

    public void Initialize()
    {
        init = true;
    }

    //How to lerp
    //http://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly
}
