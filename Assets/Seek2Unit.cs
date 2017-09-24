using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek2Unit : MonoBehaviour
{

    public Transform target;

    private SteeringBasics2 steeringBasics2;

    private void Start()
    {
        steeringBasics2 = GetComponent<SteeringBasics2>();
    }

    private void Update()
    {
        var accel = steeringBasics2.Seek(target.position);

        steeringBasics2.Steer(accel);
        steeringBasics2.LookWhereYoureGoing();
    }
}
