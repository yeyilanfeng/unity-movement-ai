using UnityEngine;
using System.Collections;

public class WallAvoidanceUnit: MonoBehaviour
{
    // 编辑器编辑 路径
    public LinePath path;

    // 组件
    private SteeringBasics steeringBasics;
    private WallAvoidance wallAvoidance;
    private FollowPath followPath;

    
    void Start()
    {
        path.calcDistances();

        steeringBasics = GetComponent<SteeringBasics>();
        wallAvoidance = GetComponent<WallAvoidance>();
        followPath = GetComponent<FollowPath>();
    }

    // Update is called once per frame
    void Update()
    {
        // 到达终点了， 原路返回
        if (isAtEndOfPath())
        {
            path.reversePath();
        }

        // 得到加速度 （要躲避墙）
        Vector3 accel = wallAvoidance.GetSteering();

        // 沿着路径走了 （约定没有碰撞时 加速度为0了）
        if (accel.magnitude < 0.005f)
        {
            accel = followPath.getSteering(path);
        }

        // 设置刚体 和 朝向
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();

        // 调试
        path.draw();
    }

    public bool isAtEndOfPath()
    {
        return Vector3.Distance(path.endNode, transform.position) < followPath.stopRadius;
    }
}
