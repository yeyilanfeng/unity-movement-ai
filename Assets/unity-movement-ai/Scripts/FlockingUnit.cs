using UnityEngine;
using System.Collections;

public class FlockingUnit : MonoBehaviour
{
    // 参数
    public float cohesionWeight = 1.5f;
    public float separationWeight = 2f;
    public float velocityMatchWeight = 1f;

    // 速度
    private SteeringBasics steeringBasics;
    private Wander2 wander;
    private Cohesion cohesion;
    private Separation separation;
    private VelocityMatch velocityMatch;

    private NearSensor sensor;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander2>();
        cohesion = GetComponent<Cohesion>();
        separation = GetComponent<Separation>();
        velocityMatch = GetComponent<VelocityMatch>();

        sensor = transform.Find("Sensor").GetComponent<NearSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 accel = Vector3.zero;
        // 集群加速度
        accel += cohesion.GetSteering(sensor.targets) * cohesionWeight;
        // 分隔加速度
        accel += separation.GetSteering(sensor.targets) * separationWeight;
        // 速度匹配加速度
        accel += velocityMatch.GetSteering(sensor.targets) * velocityMatchWeight;

        // 如果没有那些 影响， 就漫游好了
        if (accel.magnitude < 0.005f)
        {
            accel = wander.GetSteering();
        }

        // 设置刚体速度  和  朝向
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
}
