using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class networkManagerUI : MonoBehaviour
{
   [SerializeField] private Button svrButton;
   [SerializeField] private Button hostButton;
   [SerializeField] private Button clientButton;


   private void Awake()
   {
       svrButton.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartServer();
       });
       hostButton.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartHost();
       });
         clientButton.onClick.AddListener(() =>
         {
              NetworkManager.Singleton.StartClient();
         });
    }       
   
}
