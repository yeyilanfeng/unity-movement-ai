using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringBasics))]
public class OffsetPursuit : MonoBehaviour {
    /*未来预测的最大预测时间*/
    public float maxPrediction = 1f;

    private Rigidbody rb;
    private SteeringBasics steeringBasics;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        steeringBasics = GetComponent<SteeringBasics>();
    }

    public Vector3 getSteering(Rigidbody target, Vector3 offset)
    {
        Vector3 targetPos;
        return getSteering(target, offset, out targetPos);
    }

    public Vector3 getSteering(Rigidbody target, Vector3 offset, out Vector3 targetPos)
    {
        // 得到世界坐标的偏移位置
        Vector3 worldOffsetPos = target.position + target.transform.TransformDirection(offset);

        Debug.DrawLine(transform.position, worldOffsetPos);

        /* 计算距离到偏移点 */
        Vector3 displacement = worldOffsetPos - transform.position;
        float distance = displacement.magnitude;

 
        float speed = rb.velocity.magnitude;

        /* 预测的距离不要超过当前距离 */
        float prediction;
        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        /* 目标位置 */
        targetPos = worldOffsetPos + target.velocity * prediction;

        return steeringBasics.Arrive(targetPos);
    }
}
