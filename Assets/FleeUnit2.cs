using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeUnit2 : MonoBehaviour {

    public Transform target;

    public SteeringBasics2 steeringBasics;

    public Flee2 flee;

    private void Start()
    {
        steeringBasics = GetComponent<SteeringBasics2>();

        flee = GetComponent<Flee2>();
    }

    private void Update()
    {
        // 得到线性加速度
        Vector3 accel = flee.GetSteering(target.position);
        // 设置刚体速度
        steeringBasics.Steer(accel);
        // 设置朝向
        steeringBasics.LookWhereYoureGoing();
    }
}
