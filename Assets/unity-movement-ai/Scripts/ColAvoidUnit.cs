using UnityEngine;
using System.Collections;

public class ColAvoidUnit : MonoBehaviour {
    // 编辑器设置
    public LinePath path;

    // 组件
    private SteeringBasics steeringBasics;
    private FollowPath followPath;
    private CollisionAvoidance colAvoid;

    private NearSensor colAvoidSensor;

    void Start()
    {
        path.calcDistances();

        steeringBasics = GetComponent<SteeringBasics>();
        followPath = GetComponent<FollowPath>();
        colAvoid = GetComponent<CollisionAvoidance>();

        colAvoidSensor = transform.Find("ColAvoidSensor").GetComponent<NearSensor>();
    }

    void Update()
    {
        // 调试
        path.draw();

        // 是否原路返回
        if (isAtEndOfPath())
        {
            path.reversePath();
        }

        // 躲避 加速度
        Vector3 accel = colAvoid.GetSteering(colAvoidSensor.targets);

        // 不需要躲避 就沿着路径走
        if (accel.magnitude < 0.005f)
        {
            accel = followPath.getSteering(path);
        }

        // 设置刚体速度 和 朝向
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }

    public bool isAtEndOfPath()
    {
        return Vector3.Distance(path.endNode, transform.position) < followPath.stopRadius;
    }
}
