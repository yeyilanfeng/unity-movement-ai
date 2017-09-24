using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBasics2))]
public class Pursue2 : MonoBehaviour
{
    // 未来预测的最大预测时间
    public float maxPrediction = 1f;

    private Rigidbody rb;

    private SteeringBasics2 steeringBasics;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        steeringBasics = GetComponent<SteeringBasics2>();
    }


    internal Vector3 GetSteering(Rigidbody target)
    {
        // 计算到目标的距离
        Vector3 displacement = target.position - transform.position;
        float distance = displacement.magnitude;

        // 当前的速度
        float speed = rb.velocity.magnitude;

        // 计算预测时间 （不能让预测时间能跑的距离 超过当前的距离）
        float prediction;
        if (speed <= distance / maxPrediction)  // (maxPrediction * speed <= distance ) 
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed; 
        }

        // 目标 ： 在目标位置基础上添加预测的部分
        Vector3 explicitTarget = target.position + target.velocity * prediction;

        return steeringBasics.Seek(explicitTarget);
    }
}
