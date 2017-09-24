using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterposeUnit2 : MonoBehaviour {

    public Rigidbody target1;

    public Rigidbody target2;

    private SteeringBasics2 steeringBasics;

    private void Start()
    {
        steeringBasics = GetComponent<SteeringBasics2>();
    }

    private void Update()
    {
        Vector3 accel = steeringBasics.Interpose(target1, target2);

        steeringBasics.Steer(accel);

        steeringBasics.LookWhereYoureGoing();
    }
}
