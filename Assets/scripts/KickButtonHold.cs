using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class KickButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PlayerController Player;
    [SerializeField] private NetworkedFootball Ball;
    private BallPossession ballPossession;

    private Coroutine holdCoroutine;
   
    public UnityEvent onPressedOverSeconds;

    
    void Awake()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<NetworkedFootball>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ballPossession = GameObject.FindGameObjectWithTag("Player").GetComponent<BallPossession>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       Ball.OnKickButton();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Ball.EndHoldCoroutine();
        Ball.OnKickButtonhelper(Ball.seconds);
        Ball.seconds = 0.0f;
        Player.UpdateMicrobar(Ball.seconds);
    }

    
}
