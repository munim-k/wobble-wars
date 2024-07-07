// using System.Collections;
// using System.Collections.Generic;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Lobbies;
// using Unity.Services.Lobbies.Models;
// using UnityEngine;
//
// public class LobbyScript : MonoBehaviour
// {
//     private async void Start()
//     {
//         await UnityServices.InitializeAsync();
//         AuthenticationService.Instance.SignedIn += () =>
//         {
//             Debug.Log("Signed in");
//         };
//         await AuthenticationService.Instance.SignInAnonymouslyAsync();
//     }
//
//     private async void CreateLobby()
//     {
//         try
//         {
//             string LobbyName = "MyLobby";
//             int maxPlayers = 4;
//             Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(LobbyName, maxPlayers);
//             Debug.Log($"Lobby created: {lobby.Id}");
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.LogError($"Failed to create lobby: {e.Message}");
//         }
//     }
//     
//     private async void JoinLobby()
//     {
//         try
//         {
//             string LobbyId = "MyLobby";
//             Lobby lobby = await LobbyService.Instance.GetLobbyAsync(LobbyId);
//             await lobby.JoinAsync();
//             Debug.Log($"Joined lobby: {lobby.Id}");
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.LogError($"Failed to join lobby: {e.Message}");
//         }
//     }
//     
//     private async void LeaveLobby()
//     {
//         try
//         {
//             string LobbyId = "MyLobby";
//             Lobby lobby = await LobbyService.Instance.GetLobbyAsync(LobbyId);
//             await lobby.LeaveAsync();
//             Debug.Log($"Left lobby: {lobby.Id}");
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.LogError($"Failed to leave lobby: {e.Message}");
//         }
//     }
//     
//     private async void StartMatch()
//     {
//         try
//         {
//             string LobbyId = "MyLobby";
//             Lobby lobby = await LobbyService.Instance.GetLobbyAsync(LobbyId);
//             await lobby.StartMatchAsync();
//             Debug.Log($"Started match in lobby: {lobby.Id}");
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.LogError($"Failed to start match: {e.Message}");
//         }
//     }
//     
//     private async void CancelMatch()
//     {
//         try
//         {
//             string LobbyId = "MyLobby";
//             Lobby lobby = await LobbyService.Instance.GetLobbyAsync(LobbyId);
//             await lobby.CancelMatchAsync();
//             Debug.Log($"Cancelled match in lobby: {lobby.Id}");
//         }
//         catch(LobbyServiceException e)
//         {
//             Debug.LogError($"Failed to cancel match: {e.Message}");
//         }
//     }
//     
//     public GameObject lobbyItemPrefab; // Assign in inspector
//     public Transform lobbyListParent; // Assign in inspector
//
//     private void UpdateLobbyListUI(List<Lobby> lobbies)
//     {
//         foreach (Transform child in lobbyListParent)
//         {
//             Destroy(child.gameObject);
//         }
//
//         foreach (var lobby in lobbies)
//         {
//             var item = Instantiate(lobbyItemPrefab, lobbyListParent);
//             item.GetComponent<LobbyItemUI>().Setup(lobby);
//         }
//     }
//     private async void FetchAvailableLobbies()
//     {
//         try
//         {
//             var query = new QueryLobbiesOptions();
//             var result = await LobbyService.Instance.QueryLobbiesAsync(query);
//             UpdateLobbyListUI(result.Lobbies);
//         }
//         catch (LobbyServiceException e)
//         {
//             Debug.LogError($"Failed to fetch lobbies: {e.Message}");
//         }
//     }
//    
//     
// }
