using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour
{

    public float scrollSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
    }
}
