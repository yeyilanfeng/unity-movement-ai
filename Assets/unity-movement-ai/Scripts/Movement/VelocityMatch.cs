using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringBasics))]
public class VelocityMatch : MonoBehaviour
{
    // 视野范围
    public float facingCosine = 90;

    public float timeToTarget = 0.1f;
    public float maxAcceleration = 4f;

    // 视野的余弦值
    private float facingCosineVal;


    private Rigidbody rb;
    private SteeringBasics steeringBasics;

    // Use this for initialization
    void Start()
    {
        facingCosineVal = Mathf.Cos(facingCosine * Mathf.Deg2Rad);

        rb = GetComponent<Rigidbody>();
        steeringBasics = GetComponent<SteeringBasics>();
    }

    public Vector3 GetSteering(ICollection<Rigidbody> targets)
    {
        Vector3 accel = Vector3.zero;
        int count = 0;

        // 得到我视野内 所有人的 加速度平均值
        foreach (Rigidbody r in targets)
        {
            if (steeringBasics.isFacing(r.position, facingCosineVal))
            {
                /* 计算我们想要匹配这个目标的加速度 */
                Vector3 a = r.velocity - rb.velocity;
                /*
                 Rather than accelerate the character to the correct speed in 1 second, 
                 accelerate so we reach the desired speed in timeToTarget seconds 
                 (if we were to actually accelerate for the full timeToTarget seconds).
                */
                // 而不是在1秒内加速字符的正确速度，这样我们就能在目标秒内达到所期望的速度(如果我们要在目标秒内加速的话)。
                a = a / timeToTarget;

                accel += a;

                count++;
            }
        }

        if (count > 0)
        {
            accel = accel / count;

            /* 不要超值 */
            if (accel.magnitude > maxAcceleration)
            {
                accel = accel.normalized * maxAcceleration;
            }
        }

        return accel;
    }
}
