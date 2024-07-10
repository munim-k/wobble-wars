using System.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkedFootball : NetworkBehaviour
{
    public float KickPower = 5f;
    [SerializeField] private BallPossession ballPossession;
    [SerializeField] private float upForce = 1.003f;
    public GameObject CurrentPlayer;
    public bool CanTakeBall = true;
    private Rigidbody rb;
    private PlayerController playerController;
    public Coroutine holdCoroutine;
    private PlayerController Player;
    private Button kickButton;

    public float seconds = 0.0f;
    public UnityEvent onPressedOverSeconds;

    [SerializeField] private float[] KickPowerTime = { 0.0f, 0.5f, 1.0f, 1.5f };
    [SerializeField] private float[] KickPowerValue = { 5.0f, 6.0f, 7.0f, 9.0f };
    [SerializeField] private float[] UpForceValue = { 1.003f, 1.003f, 1.002f, 1.001f };
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ballPossession = GameObject.FindGameObjectWithTag("Player").GetComponent<BallPossession>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void SetCurrentPlayer(GameObject player)
    {
        CurrentPlayer = player;
    }
    public void OnKickButton()
    {
        if(!IsOwner || !ballPossession.hasBall)
        {
            return;
        }
        holdCoroutine = StartCoroutine(Hold());
    }
    IEnumerator Hold()
    {
        while (true)
        {
            seconds += Time.deltaTime;
            Player.UpdateMicrobar(seconds);
            yield return null;
        }
    }
    public void EndHoldCoroutine()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
        }
    }
    public void OnKickButtonhelper(float HoldTime)
    {
        if (!IsOwner || !ballPossession.hasBall)
        {
            return;
        }
        if(HoldTime > KickPowerTime[3])
        {
            KickPower = KickPowerValue[3];
            upForce = UpForceValue[3];
        }
        else if(HoldTime > KickPowerTime[2])
        {
            KickPower = KickPowerValue[2];
            upForce = UpForceValue[2];
        }
        else if(HoldTime > KickPowerTime[1])
        {
            KickPower = KickPowerValue[1];
            upForce = UpForceValue[1];
        }
        else
        {
            KickPower = KickPowerValue[0];
            upForce = UpForceValue[0];
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
