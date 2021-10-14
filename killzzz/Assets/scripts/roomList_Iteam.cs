using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;



public class roomList_Iteam : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public RoomInfo info;
    public void setUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;

    }
    public void OnClick()
    {
        Launcher.Instance.joinRoom(info);
    }

}
