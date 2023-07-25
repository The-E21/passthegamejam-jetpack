using UnityEngine;

public class GrappleController : MonoBehaviour
{
    [SerializeField] private Transform grappleGun;
    [SerializeField] private float gunRadius;

    [SerializeField] private GameObject grappleObject;
    private GameObject grapple;

    private Vector2 direction;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float spawnRadius;

    private void Start() {
        grapple = null;    
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
        }
        else {
            Destroy(grapple);
            grapple = null;
        }
    }
}
