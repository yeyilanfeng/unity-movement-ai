using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringBasics))]
[RequireComponent(typeof(Evade))]
public class Hide : MonoBehaviour {
    // 边界距离 （距离障碍物  边的距离）
    public float distanceFromBoundary = 0.6f;

    // 组件
    private SteeringBasics steeringBasics;
    private Evade evade;

    // Use this for initialization
    void Start () {
        steeringBasics = GetComponent<SteeringBasics>();
        evade = GetComponent<Evade>();
	}

    public Vector3 GetSteering(Rigidbody target, ICollection<Rigidbody> obstacles, out Vector3 bestHidingSpot)
    {
        // 临时值
        float distToClostest = Mathf.Infinity;
        bestHidingSpot = Vector3.zero;

        // 找到最近的隐藏点 ， 遍历所有障碍
        foreach(Rigidbody r in obstacles)
        {
            // 这个障碍  可以作为隐蔽的位置
            Vector3 hidingSpot = GetHidingPosition(r, target);

            // 离我多远
            float dist = Vector3.Distance(hidingSpot, transform.position);

            // 最近就保存
            if(dist < distToClostest)
            {
                distToClostest = dist;
                bestHidingSpot = hidingSpot;
            }
        }

        //如果没有发现隐藏点，就只躲避敌人    (比如我和敌人都处于所有障碍的一侧)
        if (distToClostest == Mathf.Infinity)
        {
            return evade.GetSteering(target);
        }

        Debug.DrawLine(transform.position, bestHidingSpot);

        // 返回加速度
        return steeringBasics.Arrive(bestHidingSpot);
    }

    // 获取隐藏位置
    private Vector3 GetHidingPosition(Rigidbody obstacle, Rigidbody target)
    {
        // 这个障碍物 附近
        float distAway = SteeringBasics.GetBoundingRadius(obstacle.transform) + distanceFromBoundary;
        // 目标看障碍物的方向（就要躲在这个方向上）
        Vector3 dir = obstacle.position - target.position;
        dir.Normalize();

        // 最终隐藏位置
        return obstacle.position + dir * distAway;
    }
}
