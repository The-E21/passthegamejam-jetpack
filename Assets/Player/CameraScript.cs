using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    private Vector3 velocity;
    private float zPos;
    [SerializeField] private float smoothTime;
    [SerializeField] private Vector2 offset;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        zPos = transform.position.z;
        velocity = Vector3.zero;
    }

    private void LateUpdate() {
        Vector3 targetPos = new Vector3(player.position.x + offset.x, player.position.y + offset.y, zPos);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
