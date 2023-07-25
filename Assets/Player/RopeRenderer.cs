using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    private LineRenderer lr;

    //Spent a long time researching this for it to not work lol

    [HideInInspector] public float ropeLength;
    /*[SerializeField] private int segmentNum;
    private float segmentLength;
    private RopeSegment[] segments;
    */
    [HideInInspector] public Transform startObject;
    [HideInInspector] public Transform endObject;
    /*
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        segments = new RopeSegment[segmentNum];
        segmentLength = ropeLength / segmentNum;
    }

    private void FixedUpdate() {
        SimulateRope();
        DrawRope();
    }

    private void SimulateRope() {
        // Simulation
        for(int i = 0; i < segmentNum; i ++) {
            Vector2 velocity = segments[i].pos - segments[i].posOld;
            segments[i].posOld = segments[i].pos;
            segments[i].pos += Physics2D.gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        }

        //Constraints
        for(int i = 0; i < 15; i ++){
            ApplyConstraints();
        }
    }

    private void ApplyConstraints() {
        segments[0].pos = startObject.position;
        segments[segmentNum - 1].pos = endObject.position;

        for(int i = 0; i < segmentNum - 1; i ++) {
            float dist = (segments[i].pos - segments[i + 1].pos).magnitude;
            float error = dist - segmentLength;
            Vector2 changeDir = (segments[i].pos - segments[i + 1].pos).normalized;
            Vector2 changeAmount = changeDir * error * 0.5f;
            if(i != 0)
                segments[i].pos += changeAmount;
            
            if(i != segmentNum - 2)
                segments[i + 1].pos -= changeAmount;
        }
    }

    private void DrawRope() {
        Vector3[] positions = new Vector3[segmentNum];
        for(int i = 0; i < segmentNum; i ++) {
            positions[i] = segments[i].pos;
        }
        lr.SetPositions(positions);
    }

    private struct RopeSegment {
        public Vector2 pos;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos){
            this.pos = pos;
            posOld = pos;
        }   
    }*/

    private void Update() {
        lr.SetPositions(new Vector3[] {startObject.position, endObject.position});
    }

    private void Start() {
        lr = GetComponent<LineRenderer>();
    }
}
