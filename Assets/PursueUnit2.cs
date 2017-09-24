using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueUnit2 : MonoBehaviour {
    // 得到目标位置 和 当前速度
    public Rigidbody target;

    // 组件
    private Pursue2 pursue;

    private SteeringBasics2 steeringBasics;

    private void Start()
    {
        steeringBasics = GetComponent<SteeringBasics2>();
        pursue = GetComponent<Pursue2>();
    }

    private void Update()
    {
        // 得到线性加速度
        Vector3 accel = pursue.GetSteering(target);

        // 设置刚体速度
        steeringBasics.Steer(accel);

        // 朝向
        steeringBasics.LookWhereYoureGoing();
    }
}
