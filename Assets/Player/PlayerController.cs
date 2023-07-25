using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Transform sprites;
    [SerializeField] private float maxVelocity;

    [Header("Ground/Wall check")]
    [SerializeField] private Transform[] feet;
    [SerializeField] private float footSize;
    [SerializeField] private LayerMask wallMask;

    [Header("Horizontal Movement")]
    private float horizontalAxis;
    [SerializeField] private float maxGroundSpeed;
    [SerializeField] private float groundedAccelMultiplier;

    [Header("Jetpack movement")]
    private bool jetting;
    private float angle;
    [SerializeField] private float rotateRate;
    [SerializeField] private KeyCode jetKey;
    [SerializeField] private float jetpackAccelMultiplier;
    [SerializeField] private float maxJetSpeedVertical;
    [SerializeField] private float maxJetSpeedHorizontal;
    [SerializeField] private ParticleSystem[] jetPackParticles;
    private float[] jetpackParticlesAngleOffsets;
    [SerializeField] private string thrusterSound;

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
        jetpackParticlesAngleOffsets = new float[jetPackParticles.Length];
        for(int i = 0; i < jetPackParticles.Length; i ++) {
            jetpackParticlesAngleOffsets[i] = jetPackParticles[i].shape.rotation.z;
        }
    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        jetting = Input.GetKey(jetKey) && fuel > 0;

        sprites.rotation = Quaternion.Euler(sprites.rotation.x, sprites.rotation.y, Mathf.Rad2Deg * angle);
        fuelBar.value = fuel;

        if(Input.GetKeyDown(jetKey) && fuel > 0) {
            foreach(ParticleSystem particles in jetPackParticles) {
                particles.Play();
            }
            AudioManager.Instance.Play(thrusterSound);
        }

        else if (Input.GetKeyUp(jetKey)) {
            stopEffects();
        }

        if(jetting) {
            for(int i = 0; i < jetPackParticles.Length; i ++) {
                var shape = jetPackParticles[i].shape;
                shape.rotation = new Vector3(shape.rotation.x, shape.rotation.y, angle * Mathf.Rad2Deg + jetpackParticlesAngleOffsets[i]);
            }
        }
    }

    private void FixedUpdate() {
        bool grounded = CheckGrounded();
        float xAccel = 0f;
        float yAccel = 0f;
        
        if(jetting) {
            yAccel += (maxJetSpeedVertical - rb.velocity.y) * jetpackAccelMultiplier * Mathf.Sin(angle + Mathf.PI * 0.5f);
            xAccel += (maxJetSpeedHorizontal - rb.velocity.x) * jetpackAccelMultiplier * Mathf.Cos(angle + Mathf.PI * 0.5f);
            fuel -= consumptionRate * Time.fixedDeltaTime;
            if(fuel <= 0) {
                stopEffects();
            }
        }

        if(grounded) {
            xAccel += (horizontalAxis * maxGroundSpeed - rb.velocity.x) * groundedAccelMultiplier;
            angle = 0;
        } else {
            angle -= rotateRate * horizontalAxis * Time.fixedDeltaTime;
        }
        rb.AddForce(new Vector2(xAccel, yAccel));
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);
    }

    private bool CheckGrounded() {
        foreach(Transform foot in feet) {
            if(Physics2D.RaycastAll(foot.transform.position, Vector2.down, footSize, wallMask).Length > 0)
                return true;
        } 

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<FuelCapsule>(out FuelCapsule capsule)) {
            fuel += capsule.amount;
            fuel = Mathf.Clamp(fuel, 0f, maxFuel);
            capsule.Consume();
        }
    }

    private void stopEffects() {
        foreach(ParticleSystem particles in jetPackParticles) {
            particles.Stop();
        }
        AudioManager.Instance.Stop(thrusterSound);
    }
}
