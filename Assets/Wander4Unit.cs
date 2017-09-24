using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander4Unit : MonoBehaviour {

    //  实际操作 Rigidbody 组件
    private SteeringBasics2 steeringBasics2;

    // 主要是得到加速度方向
    private Wander4 wander4;

    private void Start()
    {
        steeringBasics2 = GetComponent<SteeringBasics2>();
        wander4 = GetComponent<Wander4>();
    }

    private void Update()
    {
        // 得到加速度
        Vector3 accel = wander4.GetSteering();

        // 设置刚体 和 方向
        steeringBasics2.Steer(accel);
        steeringBasics2.LookWhereYoureGoing();
    }
}
