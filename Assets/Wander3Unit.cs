using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander3Unit : MonoBehaviour
{
    // 实际操作 Rigidbody 组件
    private SteeringBasics2 steeringBasics2;

    // 主要是得到加速度方向
    private Wander3 wander3;

    private void Start()
    {
        steeringBasics2 = GetComponent<SteeringBasics2>();
        wander3 = GetComponent<Wander3>();
    }

    private void Update()
    {
        // 得到加速度
        Vector3 accel = wander3.GetSteering();

        // 设置刚体 和 方向
        steeringBasics2.Steer(accel);
        steeringBasics2.LookWhereYoureGoing();
    }
}
