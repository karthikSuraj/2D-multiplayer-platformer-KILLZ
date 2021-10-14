using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollower : MonoBehaviour
{
    public GameObject player;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        distance = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + distance;
     
    }
}
