using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class networkManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject networkmanagerUIGameObject;
   [SerializeField] private Button svrButton;
   [SerializeField] private Button hostButton;
   [SerializeField] private Button clientButton;


   private void Awake()
   {
       svrButton.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartServer();
           networkmanagerUIGameObject.SetActive(false);
       });
       hostButton.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartHost();
           networkmanagerUIGameObject.SetActive(false);
       });
        clientButton.onClick.AddListener(() =>
        {
              NetworkManager.Singleton.StartClient();
              networkmanagerUIGameObject.SetActive(false);
        });
    }       
   
}
