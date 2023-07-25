using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrappleController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Transform grappleGun;
    [SerializeField] private float gunRadius;

    [SerializeField] private GameObject grappleObject;
    private GameObject grapple;
    [SerializeField] private GameObject ropeObject;
    private GameObject ropeActiveObject;

    private Vector2 direction;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float spawnRadius;
    [SerializeField] private string shootSound;

    [SerializeField] private float ropeLength;

    private void Start() {
        grapple = null;
        rb = GetComponent<Rigidbody2D>();    
    }

    private void Update() {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction = direction.normalized;
        grappleGun.rotation = Quaternion.Euler(grappleGun.rotation.x, grappleGun.rotation.y, Vector2.SignedAngle(Vector2.right, direction));
        grappleGun.localPosition = direction * gunRadius;
        if(Input.GetMouseButtonDown(0)) {
            Shoot();
        }       
    }

    private void Shoot() {
        if(grapple == null) {
            grapple = Instantiate(grappleObject);
            grapple.transform.position = (Vector2)transform.position + direction * spawnRadius;
            grapple.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            DistanceJoint2D rope = grapple.AddComponent<DistanceJoint2D>();
            rope.maxDistanceOnly = true;
            rope.autoConfigureDistance = false;
            rope.distance = ropeLength;
            rope.connectedBody = rb;
            AudioManager.Instance.Play(shootSound);
            ropeActiveObject = Instantiate(ropeObject);
            RopeRenderer rr = ropeActiveObject.GetComponent<RopeRenderer>();
            rr.startObject = transform;
            rr.endObject = grapple.transform;
            rr.ropeLength = ropeLength;
        }
        else {
            Destroy(grapple);
            grapple = null;
            Destroy(ropeActiveObject);
            ropeActiveObject = null;
        }
    }
}
