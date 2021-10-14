using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

using UnityEngine.SceneManagement;
public class playerManager : MonoBehaviour
{
    


    int player ;
    PhotonView PV;
    GameObject controller;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        
    }
    void Start()

    {
        player = roomManager.instance.player;
        if (PV.IsMine)

        {

            CreateController();
            

        }
    }

    void CreateController()
    {
        if (player == 2)
        {
            Green();
        }
        if (player == 1)
        {
            Red();
        }

    }

   

    public void Red()
    {
        player = 0;

        controller= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PLAYER1prefab"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
        
       
    }

    public void Green()
    {
        player = 0;

        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PLAYER2prefab"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });


    }

    public void OnClickLeaveRoomButton()
    {
        SceneManager.LoadScene(0);
        PhotonNetwork.Disconnect();
        
    }
    public void die()
    {
        PhotonNetwork.Destroy(controller);
        player = roomManager.instance.player;
        CreateController();
    }

   


}
