using UnityEngine;
using System.Collections;

public class PursueUnit : MonoBehaviour {

    public Rigidbody target;

    private SteeringBasics steeringBasics;
    private Pursue pursue;

    // Use this for initialization
    void Start () {
        steeringBasics = GetComponent<SteeringBasics>();
        pursue = GetComponent<Pursue>();
	}
	
	// Update is called once per frame
	void Update () {
        // 得到 线性加速度
        Vector3 accel = pursue.getSteering(target);

        // 设置刚体速度
        steeringBasics.Steer(accel);

        // 朝向
        steeringBasics.LookWhereYoureGoing();
	}
}
