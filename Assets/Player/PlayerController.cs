using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Transform sprites;

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
    private float angle;
    [SerializeField] private float rotateRate;
    [SerializeField] private KeyCode jetKey;
    [SerializeField] private float jetpackAccelMultiplier;
    [SerializeField] private float maxJetSpeed;

    [Header("Fuel Consumption")]
    [SerializeField] private float consumptionRate;
    [SerializeField] private float maxFuel;
    private float fuel;
    [SerializeField] private Slider fuelBar;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        fuel = maxFuel;
        fuelBar.maxValue = maxFuel;
        fuelBar.value = fuel;    
    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        jetting = Input.GetKey(jetKey);

        sprites.rotation = Quaternion.Euler(sprites.rotation.x, sprites.rotation.y, Mathf.Rad2Deg * angle);
        fuelBar.value = fuel;
    }

    private void FixedUpdate() {
        bool grounded = CheckGrounded();
        float xAccel = 0f;
        float yAccel = 0f;
        
        if(jetting && fuel > 0) {
            yAccel += (maxJetSpeed - rb.velocity.y) * jetpackAccelMultiplier * Mathf.Sin(angle + Mathf.PI * 0.5f);
            xAccel += (maxJetSpeed - rb.velocity.x) * jetpackAccelMultiplier * Mathf.Cos(angle + Mathf.PI * 0.5f);
            fuel -= consumptionRate * Time.fixedDeltaTime;
        }

        if(grounded) {
            xAccel += (horizontalAxis * maxSpeed - rb.velocity.x) * groundedAccelMultiplier;
            angle = 0;
        } else {
            angle -= rotateRate * horizontalAxis * Time.fixedDeltaTime;
        }
        rb.AddForce(new Vector2(xAccel, yAccel));
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
    }

    private bool CheckGrounded() {
        foreach(Transform foot in feet) {
            if(Physics2D.RaycastAll(foot.transform.position, Vector2.down, footSize, wallMask).Length > 0)
                return true;
        } 

        return false;
    }
}
