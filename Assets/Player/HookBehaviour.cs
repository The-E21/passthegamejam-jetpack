using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HookBehaviour: MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private LayerMask collisionMask;
    private bool connected;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        connected = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(collisionMask == (collisionMask | 1 << other.gameObject.layer) && !connected) {
            FixedJoint2D joint = other.gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = rb;
            connected = true;
        }
    }
}
