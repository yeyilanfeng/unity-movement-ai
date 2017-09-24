using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionAvoidance : MonoBehaviour
{
    public float maxAcceleration = 15f;
    // 角色半径
    private float characterRadius;

    private Rigidbody rb;


    void Start()
    {
        characterRadius = SteeringBasics.GetBoundingRadius(transform);

        rb = GetComponent<Rigidbody>();
    }

    public Vector3 GetSteering(ICollection<Rigidbody> targets)
    {
        Vector3 acceleration = Vector3.zero;

        /* 1. 找出这个角色将会与之碰撞的第一个目标 */

        /* 第一次碰撞时间 */
        float shortestTime = float.PositiveInfinity; // 正无穷大   临时值

       /* The first target that will collide and other data that
        * we will need and can avoid recalculating */

       // 重置数据  ，并且可以避免重新计算
       Rigidbody firstTarget = null;
        //float firstMinSeparation = 0, firstDistance = 0;
        float firstMinSeparation = 0, 
            firstDistance = 0, 
            firstRadius = 0;
        Vector3 firstRelativePos = Vector3.zero, 
            firstRelativeVel = Vector3.zero;

        foreach (Rigidbody r in targets)
        {
            /* 计算碰撞时间 */
            // 相差位置
            Vector3 relativePos = transform.position - r.position;
            // 相差速度
            Vector3 relativeVel = rb.velocity - r.velocity;
            // 标量
            float distance = relativePos.magnitude;
            float relativeSpeed = relativeVel.magnitude;

            // 说明朝着相反的方向运动 并且速度一样
            if (relativeSpeed == 0)
            {
                continue;
            }

            // 
            float timeToCollision = -1 * Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

            /* 检查它们是否会碰撞 */
            Vector3 separation = relativePos + relativeVel * timeToCollision;
            float minSeparation = separation.magnitude;

            float targetRadius = SteeringBasics.GetBoundingRadius(r.transform);

            // 两者分离了
            if (minSeparation > characterRadius + targetRadius)
            //if (minSeparation > 2 * agentRadius)
            {
                continue;
            }

            /* 检查它是否是最短， 是的话就纪录最短的 */
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = r;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
                firstRadius = targetRadius;
            }
        }

        /* 2. 计算加速度 */

        /* 如果没有目标，就退出 */
        if (firstTarget == null)
        {
            return acceleration;
        }

        /* If we are going to collide with no separation or if we are already colliding then 
		 * Steer based on current position */
        // 如果我们要在没有分离的情况下发生碰撞，或者如果我们已经碰撞了，然后根据当前位置进行碰撞
        if (firstMinSeparation <= 0 || firstDistance < characterRadius + firstRadius)
        //if (firstMinSeparation <= 0 || firstDistance < 2 * agentRadius)
        {
            acceleration = transform.position - firstTarget.position;
        }
        /* 计算未来的相对位置 */
        else
        {
            acceleration = firstRelativePos + firstRelativeVel * shortestTime;
        }

        /* 远离目标 */
        acceleration.Normalize();
        acceleration *= maxAcceleration;

        return acceleration;
    }
}
