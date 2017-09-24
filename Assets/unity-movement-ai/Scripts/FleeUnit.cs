using UnityEngine;
using System.Collections;

public class FleeUnit : MonoBehaviour
{

    public Transform target;

    private SteeringBasics steeringBasics;
    private Flee flee;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        flee = GetComponent<Flee>();
    }

    void Update()
    {
        // 得到线性加速度
        Vector3 accel = flee.getSteering(target.position);
        // 设置刚体速度
        steeringBasics.Steer(accel);
        // 设置朝向
        steeringBasics.LookWhereYoureGoing();
    }
}
