using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidanceUnit2 : MonoBehaviour
{

    // 编辑器下设置  路径 
    public LinePath path;

    // 组件
    private SteeringBasics2 steeringBasics2;
    private WallAvoidance2 wallAvoidance2;
    private FollowPath followPath;

    private void Start()
    {
        path.calcDistances();

        steeringBasics2 = GetComponent<SteeringBasics2>();
        wallAvoidance2 = GetComponent<WallAvoidance2>();
        followPath = GetComponent<FollowPath>();
    }

    private void Update()
    {
        // 到达终点了， 原路返回
        if (IsAtEndOfPath())
        {
            path.reversePath();
        }

        // 得到加速度 （要躲避墙）
        Vector3 accel = wallAvoidance2.GetSteering();

        // 沿着路径走了 （约定没有碰撞时， 加速度为0 了）
        if (accel.magnitude < 0.005f)
        {
            accel = followPath.getSteering(path);
        }

        // 设置刚体 和 朝向
        steeringBasics2.Steer(accel);
        steeringBasics2.LookWhereYoureGoing();

        // 调试
        path.draw();
    }

    private bool IsAtEndOfPath()
    {
        return Vector3.Distance(path.endNode, transform.position) < followPath.stopRadius;
    }

}
