using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cameraFollower : MonoBehaviour
{
    public GameObject player;
    PhotonView PV;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        distance = transform.position - player.transform.position;
        if(PV.IsMine)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + distance;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
