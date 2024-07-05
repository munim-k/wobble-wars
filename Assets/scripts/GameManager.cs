using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject footballPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GameObject football = Instantiate(footballPrefab, Vector3.zero, Quaternion.identity);
            football.GetComponent<NetworkObject>().Spawn();
        }
    }
}
