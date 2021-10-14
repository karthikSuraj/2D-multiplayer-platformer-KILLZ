using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Text.RegularExpressions;

public class chatManager : MonoBehaviour
{
   
     private PhotonView PV;
    public GameObject BUbbleSpeechObject;
    public TMP_InputField ChatInputField;
    public TMP_Text UpdatedText;
    private string message;
    
    
    

    

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    [PunRPC]
    void RPC_AddNewMesage(string msg)
    {
        UpdatedText.text = msg;
    }
    [PunRPC]
    void SetActive()
    {
        
        BUbbleSpeechObject.SetActive(PV.IsMine);
        SubmitChat();
        StartCoroutine(DisableCoroutine());
    }

    public void SendChat(string msg)
    {
        string NewMessage = PhotonNetwork.NickName + ": " + msg;
        PV.RPC("RPC_AddNewMesage", RpcTarget.AllBuffered, NewMessage);
    }

    public void SubmitChat()
    {
        string blankCkeck = ChatInputField.text;
        blankCkeck = Regex.Replace(blankCkeck, @"\s", "");
        if(blankCkeck == "")
        {
            ChatInputField.ActivateInputField();
            ChatInputField.text = "";
            return;
        }

        SendChat(ChatInputField.text);
        ChatInputField.ActivateInputField();
        ChatInputField.text = "";

    }


    private void Update()
    {
        
        if (!PV.IsMine)
        {

            BUbbleSpeechObject.SetActive(false);
        }
        if (PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {

                BUbbleSpeechObject.SetActive(false);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
           
            PV.RPC("SetActive", RpcTarget.AllBuffered);
        }


    }

    IEnumerator DisableCoroutine()
    {
        
        yield return new WaitForSeconds(5);
        BUbbleSpeechObject.SetActive(false);




    }



}
