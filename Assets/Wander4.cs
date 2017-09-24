
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander4 : MonoBehaviour
{
    // 偏移量
    public float wanderOffset = 1.5f;
    // 半径
    public float wanderRadius = 4;
    // 比率
    public float wanderRate = .4f;

    private float wanderOrientation = 0;

    private SteeringBasics2 steeringBasics2;

    private void Start()
    {
        steeringBasics2 = GetComponent<SteeringBasics2>();
    }

    internal Vector3 GetSteering()
    {
        // 当前角色的方向（弧度）
        float characterOrientation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        // 随机一个漫游方向(弧度）
        wanderOrientation += RandomBinomial() * wanderRate;

        // 目标方向结合（弧度）， 就是在角色方向上的一个偏移
        float targetOrientation = wanderOrientation + characterOrientation;

        // 角色的前方（偏移量）
        Vector3 targetPosition = transform.position + (OrientationToVector(characterOrientation) * wanderOffset);

        // 得到目标位置 向量 c = a + b 就是对角线的方向
        targetPosition = targetPosition + (OrientationToVector(targetOrientation) * wanderRadius);

        // 调试
        Debug.DrawLine(transform.position, targetPosition);

        // 得到 最大加速度
        return steeringBasics2.Seek(targetPosition);
    }

    /*返回一个随机数介于-1和1。 值为零的可能性更大。*/
    float RandomBinomial()
    {
        return Random.value - Random.value;
    }

    /* 返回方向的单位向量 */
    Vector3 OrientationToVector(float orientation)
    {
        // 位置 向量（极坐标 得到）     // x = rcos（θ），     y = rsin（θ），
        return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
    }
}
