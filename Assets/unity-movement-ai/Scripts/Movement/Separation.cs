using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Separation : MonoBehaviour {

    /* 分隔 加速度 */
    public float sepMaxAcceleration = 25;

    /* 
     这应该是分离目标和自身之间可能的最大分离距离。 所以它应该是:分离传感器半径+最大目标半径（separation sensor radius + max target radius ）
         */
    public float maxSepDist = 1f;

    private float boundingRadius;

    // Use this for initialization
    void Start()
    {
        boundingRadius = SteeringBasics.GetBoundingRadius(transform);
    }

    public Vector3 GetSteering(ICollection<Rigidbody> targets)
    {
        Vector3 acceleration = Vector3.zero;

        // 只要存在发生碰撞的就要想办法分离
        foreach (Rigidbody r in targets)
        {
            /* 远离的方向 */
            Vector3 direction = transform.position - r.position;
            float dist = direction.magnitude;

            if (dist < maxSepDist)
            {
                float targetRadius = SteeringBasics.GetBoundingRadius(r.transform);

                /* 计算分离强度(可改为使用平方反比，而不是线性) */
                var strength = sepMaxAcceleration * (maxSepDist - dist) / (maxSepDist - boundingRadius - targetRadius);

                /* 将分离加速度增加到现有的 Steer 上 （因为可能不是跟一个发生碰撞） */
                direction.Normalize();
                acceleration += direction * strength;
            }
        }

        return acceleration;
    }
}
