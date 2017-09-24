using UnityEngine;
using System.Collections;

public class WanderAvoidUnit : MonoBehaviour {

    private SteeringBasics steeringBasics;
    private Wander2 wander;
    private CollisionAvoidance colAvoid;

    // 用于纪录当前都有谁与我发生碰撞
    private NearSensor colAvoidSensor;

    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander2>();
        colAvoid = GetComponent<CollisionAvoidance>();

        colAvoidSensor = transform.Find("ColAvoidSensor").GetComponent<NearSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        // 加速度（有碰撞 就避免碰撞）
        Vector3 accel = colAvoid.GetSteering(colAvoidSensor.targets);

        // 没有任何碰撞的时候就  漫游
        if (accel.magnitude < 0.005f)
        {
            accel = wander.GetSteering();
        }

        // 速度
        steeringBasics.Steer(accel);
        // 朝向
        steeringBasics.LookWhereYoureGoing();
    }
}