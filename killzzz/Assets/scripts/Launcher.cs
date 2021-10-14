using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField userNameNameInputField;
    [SerializeField] TMP_Text usernameError;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text  roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject startGameButton;


     void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("connecting.... to master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        menuManager.Instance.OpenMenu("username");
       
       
        
    }
    public void startGameUsername()
    {
        if(userNameNameInputField.text.Length==0)
        {
            usernameError.text = "Please enter username! " ;
            menuManager.Instance.OpenMenu("errorUserName");
        }
        else
        {
            PhotonNetwork.NickName = userNameNameInputField.text;
            menuManager.Instance.OpenMenu("title");
        }
        
    }

    public void OnClickOKButtonUserNameError()
    {
        menuManager.Instance.OpenMenu("username");
    }

    public void CreateRoom()
    {
        if(roomNameInputField.text.Length==0)
        {

            errorText.text = "Please enter room name! ";
            menuManager.Instance.OpenMenu("error");

           
        }
        else
        {
            PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 2 }, null);
            menuManager.Instance.OpenMenu("loading");
        }
        
    }
    public override void OnJoinedRoom()
    {
        
        Debug.Log("Joined room");
        menuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList_Iteam>().setup(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
      startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public void startGame()
    {

        menuManager.Instance.OpenMenu("selectPlayer");
    }
   

    public void startGameButtons()
    {

        PhotonNetwork.LoadLevel(1);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "ROOM Creation Failed " + message;
        menuManager.Instance.OpenMenu("error");
    }
    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        menuManager.Instance.OpenMenu("title");
    }

    public override void OnLeftRoom()
    {
        menuManager.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        { 
            if (roomList[i].RemovedFromList)
            { continue; }
              
        
            Instantiate(roomListPrefab, roomListContent).GetComponent<roomList_Iteam>().setUp(roomList[i]);
        }
    }
    public void joinRoom(RoomInfo info)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinRoom(info.Name);
        menuManager.Instance.OpenMenu("loading");
        

       

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList_Iteam>().setup(newPlayer);
    }
}
