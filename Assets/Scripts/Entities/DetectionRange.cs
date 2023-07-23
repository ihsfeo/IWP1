using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{

    public List<GameObject> DetectedObjects = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged" &&
            collision.gameObject.tag != "Destructible")
            DetectedObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged" &&
            collision.gameObject.tag != "Destructible")
            DetectedObjects.Remove(collision.gameObject);
    }
}
