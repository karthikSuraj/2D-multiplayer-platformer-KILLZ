using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class roomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject redBoder, GreenBorder;
    public static roomManager instance;
    GameObject obj;

    public int player = 0;
  
    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public override void OnEnable()
    {
       
        base.OnEnable();
      
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
        
        base.OnDisable();
    }
    public void onSceneLoaded(Scene scene , LoadSceneMode loadSceneMode)
    {
        PhotonNetwork.Destroy(obj);
        if (scene.buildIndex==1 )
        {
           obj= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "playerManager"),Vector3.zero,Quaternion.identity);
            
        }

        if (scene.buildIndex == 2)
        {
            obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "playerManager"), Vector3.zero, Quaternion.identity);

        }


    }
    // Start is called before the first frame update
    void Start()
    {
        redBoder.SetActive(false);
        GreenBorder.SetActive(false);
    }

    // Update is called once per frame
  

    public void OnClickRed()
    {
        player = 2;
        redBoder.SetActive(true);
        GreenBorder.SetActive(false);

      

    }

    public void OnClickGreen()
    {
        player = 1;
        redBoder.SetActive(false);
        GreenBorder.SetActive(true);


    }



}
