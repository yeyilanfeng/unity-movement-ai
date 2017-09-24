using UnityEngine;
using System.Collections;

public class FollowPathUnit : MonoBehaviour {

    // 是否循环
    public bool pathLoop = false;
    // 原路返回
    public bool reversePath = false;
    // 编辑器设置路径的关键点
    public LinePath path;

    // 其他组件
    private SteeringBasics steeringBasics;
    private FollowPath followPath;


    void Start () {
        // 计算距离
        path.calcDistances();

        steeringBasics = GetComponent<SteeringBasics>();
        followPath = GetComponent<FollowPath>();
    }
	
    
	void Update () {

        path.draw();

        // 执行原路返回的逻辑
        if (reversePath && isAtEndOfPath())
        {
            path.reversePath();
        }

        // 计算线性加速度
        Vector3 accel = followPath.getSteering(path, pathLoop);

        // 设置刚体速度
        steeringBasics.Steer(accel);
        // 朝向
        steeringBasics.LookWhereYoureGoing();
    }

    // 到达最后节点
    public bool isAtEndOfPath()
    {
        return Vector3.Distance(path.endNode, transform.position) < followPath.stopRadius;
    }
}
