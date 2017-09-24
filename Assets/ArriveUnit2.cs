using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveUnit2 : MonoBehaviour {

    public Vector3 targetPosition;

    private SteeringBasics2 steeringBasics;



    private void Start()
    {
        steeringBasics = GetComponent<SteeringBasics2>();
    }

    private void Update()
    {
        //   计算线性加速度
        Vector3 accel = steeringBasics.Arrive(targetPosition);

        //  得到速度
        steeringBasics.Steer(accel);
        // 使当前的游戏对象的朝向   ，他要去的地方
        steeringBasics.LookWhereYoureGoing();
    }
}
