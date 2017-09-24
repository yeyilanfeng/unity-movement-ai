using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee2 : MonoBehaviour {

    // 跟目标保持的距离
    public float panicDist = 3.5f;

    // 是否开启快到目标时 减速
    public bool decelerateOnStop = true;

    public float maxAcceleration = 10f;

    // 跟 SteeringBasics2 中的意思类似
    public float timeToTarget = .1f;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 GetSteering(Vector3 targetPosition)
    {
        // 得到方向
        Vector3 acceleration = transform.position - targetPosition;

        // 不需要逃离了
        if (acceleration.magnitude > panicDist)
        {
            // 如果我们要减速，就放慢速度
            if (decelerateOnStop && rb.velocity.magnitude > 0.001f)
            {

                if (acceleration.magnitude > maxAcceleration)
                {
                    // 减速到0速  需要的时间
                    acceleration = -rb.velocity / timeToTarget;

                }

                return acceleration;
            }
            else
            {
                rb.velocity = Vector2.zero;
                return Vector3.zero;
            }
        }

        // 以最大速度逃离
        return GiveMaxAccel(acceleration);
    }

    private Vector3 GiveMaxAccel(Vector3 v)
    {
        // 移除z 影响
        v.z = 0;

        v.Normalize();

        v *= maxAcceleration;
        return v;
    }
}
