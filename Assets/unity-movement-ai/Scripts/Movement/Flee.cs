using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Flee : MonoBehaviour {
    // 跟目标保持的距离
    public float panicDist = 3.5f;
    
    // 是否开启快到目标时减速
    public bool decelerateOnStop = true;

    public float maxAcceleration = 10f;

    // 跟 SteeringBasics2中的意思类似， 
    public float timeToTarget = 0.1f;


    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    public Vector3 getSteering(Vector3 targetPosition)
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
        return giveMaxAccel(acceleration);
    }

    // 最大加速度
    private Vector3 giveMaxAccel(Vector3 v)
    {
        // 移除z影响
        v.z = 0;

        v.Normalize();

        v *= maxAcceleration;

        return v;
    }
}
