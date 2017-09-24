using UnityEngine;
using System.Collections;

public class ArriveUnit : MonoBehaviour {

    public Vector3 targetPosition;

    private SteeringBasics steeringBasics;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 accel = steeringBasics.Arrive(targetPosition);

        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
}
