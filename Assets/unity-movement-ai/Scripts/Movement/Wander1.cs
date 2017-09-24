using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringBasics))]
public class Wander1 : MonoBehaviour {
	
	/* The forward  of the wander square 偏移量 */
	public float wanderOffset = 1.5f;
	
	/* The radius of the wander square 半径 */
	public float wanderRadius = 4;
	
	/* The rate at which the wander orientation can change  比率 */
	public float wanderRate = 0.4f;
	
	private float wanderOrientation = 0;
	
	private SteeringBasics steeringBasics;

    //private GameObject debugRing;

    void Start() {
		steeringBasics = GetComponent<SteeringBasics> ();
	}

    public Vector3 getSteering() {
        // 当前角色的方向（弧度）
		float characterOrientation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        // 随机一个漫游方向（弧度）
        wanderOrientation += randomBinomial() * wanderRate;

        // 目标方向结合（弧度）， 就是在角色方向上的一个偏移
        float targetOrientation = wanderOrientation + characterOrientation;
		
		// 角色的前方（偏移量）  
		Vector3 targetPosition = transform.position + (orientationToVector (characterOrientation) * wanderOffset);
		
		// 得到目标位置 向量 c = a + b 就是对角线的方向
		targetPosition = targetPosition + (orientationToVector(targetOrientation) * wanderRadius);
		
		Debug.DrawLine (transform.position, targetPosition);
		 
        // 得到 最大加速度
		return steeringBasics.seek (targetPosition);
	}

    /*返回一个随机数介于-1和1。 值为零的可能性更大。*/
    float randomBinomial() {
		return Random.value - Random.value;
	}

    /* 返回方向的单位向量 */
    Vector3 orientationToVector(float orientation) {
        // 位置 向量（极坐标 得到）     // x = rcos（θ），     y = rsin（θ），
        return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
	}

}
