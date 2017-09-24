using UnityEngine;
using System.Collections;

public class OffsetPursuitUnit : MonoBehaviour {

    public Rigidbody target;
    public Vector3 offset;
    public float groupLookDist = 1.5f;

    // 组件
    private SteeringBasics steeringBasics;
    private OffsetPursuit offsetPursuit;
    private Separation separation;

    private NearSensor sensor;

    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        offsetPursuit = GetComponent<OffsetPursuit>();
        separation = GetComponent<Separation>();

        sensor = transform.Find("SeparationSensor").GetComponent<NearSensor>();
    }

    void LateUpdate()
    {
        Vector3 targetPos;
        // 偏移追随加速度  和  分隔加速度
        Vector3 offsetAccel = offsetPursuit.getSteering(target, offset, out targetPos);
        Vector3 sepAccel = separation.GetSteering(sensor.targets);

        // 速度会  受到两个方面的影响
        steeringBasics.Steer(offsetAccel + sepAccel);

        /* 如果我们还在前往，那就要朝向我们要去的地方，其他的方向和我们的形成目标是一样的 */
        if (Vector3.Distance(transform.position, targetPos) > groupLookDist)
        {
            steeringBasics.LookWhereYoureGoing();
        } else
        {
            steeringBasics.lookAtDirection(target.rotation);
        }
    }
}
