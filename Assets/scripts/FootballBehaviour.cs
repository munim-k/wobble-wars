using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedFootball : NetworkBehaviour
{
    public float KickPower = 1f;
    [SerializeField] private BallPossession ballPossession;
    [SerializeField] private float upForce = 1f;
    public GameObject CurrentPlayer;
    public Button KickButton;
    public bool CanTakeBall = true;

    private Rigidbody rb;
    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        KickButton = GameObject.FindGameObjectWithTag("KickButton").GetComponentInChildren<Button>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ballPossession = GameObject.FindGameObjectWithTag("Player").GetComponent<BallPossession>();
        if (KickButton != null)
            Debug.Log("KickButton found");
        KickButton.onClick.AddListener(OnKickButton);
    }

    public void SetCurrentPlayer(GameObject player)
    {
        CurrentPlayer = player;
    }

    public void OnKickButton()
    {
        Debug.Log("Kick Button Pressed");
        Debug.Log(ballPossession.hasBall);
        if (!IsOwner || !ballPossession.hasBall)
        {
            return;
        }

        if (CurrentPlayer != null)
        {
            Rigidbody playerRb = CurrentPlayer.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 forceDirection = transform.position - CurrentPlayer.transform.position;
                forceDirection = forceDirection.normalized + Vector3.up * upForce;
                float playerVelocityMagnitude = playerRb.velocity.magnitude;
                ApplyForceServerRpc(forceDirection, playerVelocityMagnitude);
            }
        }
    }

    void StartKickAnimation()
    {
        playerController.BallKickAnimation();
    }

    [ServerRpc]
    void ApplyForceServerRpc(Vector3 direction, float playerVelocity)
    {
        ApplyForceClientRpc(direction, playerVelocity);
    }

    [ClientRpc]
    void ApplyForceClientRpc(Vector3 direction, float playerVelocity)
    {
        ballPossession.LosePossession();
        CanTakeBall = false;
        Invoke("CanTakeBallswitch", 1.0f);
        StartCoroutine(AddForceToBall(direction, playerVelocity));
        StartKickAnimation();
    }
    IEnumerator AddForceToBall(Vector3 direction, float playerVelocity)
    {
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(direction * (KickPower + playerVelocity), ForceMode.Impulse);
    }
    void CanTakeBallswitch()
    {
        CanTakeBall = true;
    }
}
