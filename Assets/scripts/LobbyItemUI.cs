using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Core;
public class LobbyItemUI : MonoBehaviour
{
    public Text lobbyNameText;
    public Text playerCountText;
    public Button joinButton;

    // Setup the UI elements with lobby information
    public void Setup(Lobby lobby)
    {
        lobbyNameText.text = lobby.Name;
        playerCountText.text = $"{lobby.Players}/{lobby.MaxPlayers}";
        joinButton.onClick.AddListener(() => JoinLobby(lobby.Id));
    }

    // Method to join the lobby
    private void JoinLobby(string lobbyId)
    {
        // Implementation for joining the lobby
        Debug.Log($"Joining lobby {lobbyId}");
    }
}