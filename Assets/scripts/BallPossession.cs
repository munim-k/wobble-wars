using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BallPossession : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform ballPossessionPosition;
    public NetworkedFootball football;
    public bool hasBall = false;
    private Rigidbody playerRB;
    private Rigidbody ballRB;
    private bool IsDribbling = false;
    [SerializeField] private float BallRotationSpeed = 0.4f;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        football = ball.GetComponent<NetworkedFootball>();
        ballRB= ball.GetComponent<Rigidbody>();
        playerRB = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ball && football.CanTakeBall)
        {
           // Debug.Log("Ball Possession");
            // if(gameObject != football.CurrentPlayer)
            // {
            //     LosePossession();
            // }
            GainPossession();
            football.SetCurrentPlayer(gameObject);
        }
    }
    void Update()
    {
        if(hasBall && playerRB.velocity.magnitude > 0.1f)
        {
            IsDribbling = true;
        }
        else
        {
            IsDribbling = false;
        }

        if(IsDribbling)
        {
            ball.transform.rotation = Quaternion.LookRotation(playerRB.velocity*BallRotationSpeed);
        }
    }
    public void LosePossession()
    {
        ball.transform.parent = null;
        ballRB.constraints = RigidbodyConstraints.None;
        ballRB.mass = 1.0f;
        hasBall = false;
    }
    public void GainPossession()
    {
        ball.transform.position = ballPossessionPosition.position;
        ball.transform.parent = gameObject.transform;
        ballRB.constraints = RigidbodyConstraints.FreezePosition;
        ballRB.mass = 0.0f;
        hasBall = true;
    }
}
