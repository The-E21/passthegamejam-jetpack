using UnityEngine;

public class FuelCapsule : MonoBehaviour
{
    public float amount;

    public void Consume() {
        Destroy(gameObject);
    }
}
