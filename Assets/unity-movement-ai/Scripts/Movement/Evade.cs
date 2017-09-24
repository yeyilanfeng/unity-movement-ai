using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Flee))]
public class Evade : MonoBehaviour
{
    /*     // 未来预测的最大预测时间 */
    public float maxPrediction = 1f;

    private Flee flee;

    // Use this for initialization
    void Start()
    {
        flee = GetComponent<Flee>();
    }

    public Vector3 GetSteering(Rigidbody target)
    {
        /*  计算到目标的距离  */
        Vector3 displacement = target.position - transform.position;
        float distance = displacement.magnitude;

        /*  获得目标现在的速度  */
        float speed = target.velocity.magnitude;

        // 计算预测时间 （不能让预测时间能跑的距离 超过当前的距离）
        float prediction;
        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
            // 目标到达角色前，将预测位置在往前一点
            prediction *= 0.9f;
        }

        // 目标 ： 在目标位置基础上添加预测的部分
        Vector3 explicitTarget = target.position + target.velocity * prediction;

        // 使用之前的逃离功能
        return flee.getSteering(explicitTarget);
    }
}
