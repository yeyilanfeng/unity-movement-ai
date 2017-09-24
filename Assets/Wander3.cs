
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander3 : MonoBehaviour
{

    public float wanderRadius = 1.2f;
    public float wanderDistance = 2f;

    // 随机的最大位移(1秒)
    public float wanderJitter = 40f;

    private Vector3 wanderTarget;

    private SteeringBasics2 steeringBasics2;

    private void Start()
    {
        // 随机一个 弧度   （角度的取值范围 0~360， 弧度范围 0~2π）
        float theta = Random.value * 2 * Mathf.PI;

        // 位置 向量（极坐标 得到）     // x = rcos（θ），     y = rsin（θ），
        wanderTarget = new Vector3(wanderRadius * Mathf.Cos(theta),
            wanderRadius * Mathf.Sin(theta), 0);

        steeringBasics2 = GetComponent<SteeringBasics2>();
    }

    internal Vector3 GetSteering()
    {
        // 得到一帧时间的  最大位移
        float jitter = wanderJitter * Time.deltaTime;

        // 向目标的位置添加一个小的随机向量（每一帧都调整新的） 
        wanderTarget += new Vector3(Random.Range(-1f, 1f) * jitter,
            Random.Range(-1f, 1f) * jitter, 0f);

        // 得到新的漫游圈
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        // 得到目标位置， 在角色前方 基础上做一个偏移。 c = a + b , 就是对角线 
        Vector3 targetPosition = transform.position + transform.right * wanderDistance + wanderTarget;

        // 为了调试
        Debug.DrawLine(transform.position, targetPosition);

        //  返回方向上的  最大加速度
        return steeringBasics2.Seek(targetPosition);
    }
}
