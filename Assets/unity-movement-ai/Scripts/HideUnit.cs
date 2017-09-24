using UnityEngine;
using System.Collections;

public class HideUnit : MonoBehaviour {
    public Rigidbody target;

    private SteeringBasics steeringBasics;
    private Hide hide;
    private Spawner obstacleSpawner;

    private WallAvoidance wallAvoid;

    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        hide = GetComponent<Hide>();
        obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<Spawner>();

        wallAvoid = GetComponent<WallAvoidance>();
    }

    void Update()
    {
        // 得到加速度   躲在障碍物后面，同时不被目标看到
        Vector3 hidePosition;
        Vector3 hideAccel = hide.GetSteering(target, obstacleSpawner.objs, out hidePosition);

        // 如果撞墙要 解决
        Vector3 accel = wallAvoid.GetSteering(hidePosition - transform.position);

        // 没有撞墙 （说明如果撞墙先解决撞墙）
        if (accel.magnitude < 0.005f)
        {
            accel = hideAccel;
        }

        // 设置速度 和 朝向
        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
}
