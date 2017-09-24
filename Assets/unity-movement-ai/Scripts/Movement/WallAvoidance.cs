using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringBasics))]
public class WallAvoidance : MonoBehaviour {

    /* 前方射线应该延伸多远 */
    public float mainWhiskerLen = 1.25f;

    /* 跟墙保持的距离 */
    public float wallAvoidDistance = 0.5f;

    // 两边射线应该延伸多远
    public float sideWhiskerLen = 0.701f;

    // 两边射线的角度
    public float sideWhiskerAngle = 45f;

    public float maxAcceleration = 40f;

    // 组件
    private Rigidbody rb;
    private SteeringBasics steeringBasics;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        steeringBasics = GetComponent<SteeringBasics>();
    }

    public Vector3 GetSteering()
    {
        return GetSteering(rb.velocity);
    }

    public Vector3 GetSteering(Vector3 facingDir)
    {
        Vector3 acceleration = Vector3.zero;

        /* 创建射线方向向量 */
        Vector3[] rayDirs = new Vector3[3];
        // 自己前进的方向
        rayDirs[0] = facingDir.normalized;

        // 返回弧度，      对边y/临边x 
        float orientation = Mathf.Atan2(rb.velocity.y, rb.velocity.x);

        // 两边的射线方向
        rayDirs[1] = orientationToVector(orientation + sideWhiskerAngle * Mathf.Deg2Rad);
        rayDirs[2] = orientationToVector(orientation - sideWhiskerAngle * Mathf.Deg2Rad);

        RaycastHit hit;

        /* 如果没有碰撞，什么也不做 */
        if (!findObstacle(rayDirs, out hit))
        {
            return acceleration;
        }

        /* 从墙上创建一个目标来 seek （这个方向和射线方向相反）*/
        Vector3 targetPostition = hit.point + hit.normal * wallAvoidDistance;

        /* 如果速度和碰撞法线平行，则将目标向左或向右移动一点    (如果不矫正就会一直 垂直撞一个地方)*/
        Vector3 cross = Vector3.Cross(rb.velocity, hit.normal);   // 叉乘 判断两个向量是否平行
        // 点乘“·”计算得到的结果是一个标量；                  平行向量 normalized的点乘 是 -1 或者 1，   垂直是0
        // 叉乘“×”得到的结果是一个垂直于原向量构成平面的向量。  平行向量的叉乘是零向量
        if (cross.magnitude < 0.005f)
        {
            targetPostition = targetPostition + new Vector3(-hit.normal.y, hit.normal.x, hit.normal.z);
        }

        // 返回最大加速度
        return steeringBasics.seek(targetPostition, maxAcceleration);
    }

    /* 将弧度  作为一个  单位向量返回   （极坐标公式）*/
    private Vector3 orientationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
    }

    /// <summary>
    /// 多个射线， 检测是否发生碰撞
    /// </summary>
    /// <param name="rayDirs"></param>
    /// <param name="firstHit"></param>
    /// <returns></returns>
    private bool findObstacle(Vector3[] rayDirs, out RaycastHit firstHit)
    {
        firstHit = new RaycastHit();
        bool foundObs = false;

        for (int i = 0; i < rayDirs.Length; i++)
        {
            float rayDist = (i == 0) ? mainWhiskerLen : sideWhiskerLen;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirs[i], out hit, rayDist))
            {
                foundObs = true;
                firstHit = hit;
                break;
            }

            // 调试
            Debug.DrawLine(transform.position, transform.position + rayDirs[i] * rayDist);
        }

        return foundObs;
    }
}
