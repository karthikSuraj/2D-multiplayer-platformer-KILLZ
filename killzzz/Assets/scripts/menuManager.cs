using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class menuManager : MonoBehaviour
{
    public static menuManager Instance;
    [SerializeField] menu[] menus;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenMenu(string menuName)
    {

        for(int i=0; i< menus.Length;i++)
        {
            
            if(menus[i].menuName==menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                
                CloseMenu(menus[i]);
            }


        }
    }

    public void OpenMenu(menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {

            if (menus[i].open)
            {
              
                CloseMenu(menus[i]);
            }


        }
        menu.Open();
    }
    public void CloseMenu(menu menu)
    {
        menu.Close();
    }

}
