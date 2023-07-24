using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Ground/Wall check")]
    [SerializeField] private Transform[] feet;
    [SerializeField] private float footSize;
    [SerializeField] private LayerMask wallMask;

    [Header("Horizontal Movement")]
    private float horizontalAxis;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float groundedAccelMultiplier;

    [Header("Jetpack movement")]
    private bool jetting;
    [SerializeField] private KeyCode jetKey;
    [SerializeField] private float jetpackAccelMultiplier;
    [SerializeField] private float maxJetSpeed;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        jetting = Input.GetKey(jetKey);
    }

    private void FixedUpdate() {
        bool grounded = CheckGrounded();
        float xAccel = 0f;
        float yAccel = 0f;
        if(grounded) {
            xAccel = (horizontalAxis * maxSpeed - rb.velocity.x) * groundedAccelMultiplier;
        }

        if(jetting) {
            yAccel = (maxSpeed - rb.velocity.y) * jetpackAccelMultiplier;
        }
        rb.AddForce(new Vector2(xAccel, yAccel));
    }

    private bool CheckGrounded() {
        foreach(Transform foot in feet) {
            if(Physics2D.RaycastAll(foot.transform.position, Vector2.down, footSize, wallMask).Length > 0)
                return true;
        } 

        return false;
    }
}
