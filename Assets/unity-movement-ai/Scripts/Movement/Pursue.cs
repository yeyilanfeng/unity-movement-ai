using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringBasics))]
public class Pursue : MonoBehaviour
{
    /*未来预测的最大预测时间*/
    public float maxPrediction = 1f;

    // 组件引用
    private Rigidbody rb;
    private SteeringBasics steeringBasics;

	void Start () {
        rb = GetComponent<Rigidbody>();
        steeringBasics = GetComponent<SteeringBasics>();
	}
	
	public Vector3 getSteering (Rigidbody target) {
        /* 计算到目标的距离 */
        Vector3 displacement = target.position - transform.position;
        float distance = displacement.magnitude;

        /* 当前的速度 */
        float speed = rb.velocity.magnitude;

        /* 计算预测时间 */
        float prediction;
        if (speed <= distance / maxPrediction)   // 加速  
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;      // 保持当前速度  
        }

        /* 显式目标： 根据我们 thsink 的目标将目标放在一起*/
        Vector3 explicitTarget = target.position + target.velocity*prediction;
        Debug.Log("target.position= " + target.position + "  : prediction =" + target.velocity * prediction);

        return steeringBasics.seek(explicitTarget);
    }
}
