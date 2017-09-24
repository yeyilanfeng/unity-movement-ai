using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class SteeringBasics2 : MonoBehaviour {

    public float maxVelocity = 3.5f;
    /* The maximum acceleration */
    public float maxAcceleration = 10f;
    /* The radius from the target that means we are close enough and have arrived */
    public float targetRadius = 0.005f;

    /* The radius from the target where we start to slow down  */
    public float slowRadius = 1f;
    /* The time in which we want to achieve the targetSpeed */
    public float timeToTarget = 0.1f;

    public float turnSpeed = 20f;

    private Rigidbody rb;

    public bool smoothing = true;
    public int numSamplesForSmoothing = 5;
    private Queue<Vector2> velocitySamples = new Queue<Vector2>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 Interpose(Rigidbody target1, Rigidbody target2)
    {
        Vector3 midPoint = (target1.position + target2.position) / 2;

        // 到达 他俩中间需要的时间
        float timeToReachMidPoint = Vector3.Distance(midPoint, transform.position)/ maxVelocity;

        // 预测 当我到达后他们的位置
        Vector3 futureTarget1Pos = target1.position + target1.velocity * timeToReachMidPoint;
        Vector3 futureTarget2Pos = target2.position + target2.velocity * timeToReachMidPoint;

        midPoint = (futureTarget1Pos + futureTarget2Pos) / 2;

        return Arrive(midPoint);
    }

    internal Vector3 Arrive(Vector3 targetPosition)
    {
        /* 得到 正确方向*/
        Vector3 targetVelocity = targetPosition - transform.position;

        // 不让z轴变化
        targetVelocity.z = 0;

        /* 得到到目标的距离 */
        float dist = targetVelocity.magnitude;

        /* 如果我们在停止的范围内， 那么停止（目标不是一个点，有范围的）*/
        if (dist < targetRadius)
        {
            rb.velocity = Vector2.zero;
            return Vector2.zero;
        }

        /* 计算速度, 如果接近目标就减速， 否则就全速前进， 到目标就是0了 */
        float targetSpeed;
        if (dist > slowRadius)
        {
            targetSpeed = maxVelocity;
        }
        else
        {
            targetSpeed = maxVelocity * (dist / slowRadius);
        }

        /* 实际的速度 = 方向 * 大小  */
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        // 计算我们想要的线性加速度（下一帧的速度 - 这一帧的速度）
        Vector3 acceleration = targetVelocity - new Vector3(rb.velocity.x, rb.velocity.y, 0);

        // 到达目标速度需要的时间。 默认是一秒，很长。   时间越短，加速度就要越大才行
        acceleration *= 1 / timeToTarget;
        Debug.Log(acceleration);


        /* 不要超过最大加速度 */
        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration.Normalize();
            acceleration *= maxAcceleration;
        }

        return acceleration;
    }

    internal Vector3 Seek(Vector3 targetPosition)
    {
        return Seek(targetPosition, maxAcceleration);
    }

    private Vector3 Seek(Vector3 targetPosition, float maxSeekAccel)
    {
        // 得到方向
        Vector3 accelaration = targetPosition - transform.position;

        // z 不变
        accelaration.z = 0;

        accelaration.Normalize();
        accelaration *= maxSeekAccel;

        return accelaration;
    }

    internal void Steer(Vector3 linearAcceleration)
    {
        rb.velocity += linearAcceleration * Time.deltaTime;

        // 不能超过最大速度
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    internal void LookWhereYoureGoing()
    {
        Vector2 direction = rb.velocity;

        if (smoothing)
        {
            if (velocitySamples.Count == numSamplesForSmoothing)
            {
                velocitySamples.Dequeue();
            }

            velocitySamples.Enqueue(rb.velocity);

            direction = Vector2.zero;

            foreach (Vector2 v in velocitySamples)
            {
                direction += v;
            }

            direction /= velocitySamples.Count;
        }

        LookAtDirection(direction);
    }

    public void LookAtDirection(Vector2 direction)
    {
        direction.Normalize();
        // If we have a non-zero direction then look towards that direciton otherwise do nothing
        if (direction.sqrMagnitude > 0.001f)
        {
            float toRotation = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, toRotation, Time.deltaTime * turnSpeed);
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
