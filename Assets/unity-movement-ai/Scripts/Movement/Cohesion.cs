using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringBasics))]
public class Cohesion : MonoBehaviour {
    // 我的前方视野
    public float facingCosine = 120f;

    private float facingCosineVal;

    private SteeringBasics steeringBasics;

	// Use this for initialization
	void Start () {
        facingCosineVal = Mathf.Cos(facingCosine * Mathf.Deg2Rad);
        steeringBasics = GetComponent<SteeringBasics>();
	}

    public Vector3 GetSteering(ICollection<Rigidbody> targets)
    {
        Vector3 centerOfMass = Vector3.zero;
        int count = 0;

        /* 得到我前方视野内所有角色的  中心 */
        foreach (Rigidbody r in targets)
        {
            // 在视野内  （视野是  无限扇形）
            if (steeringBasics.isFacing(r.position, facingCosineVal))
            {
                centerOfMass += r.position;
                count++;
            }
        }

        if (count == 0)   // 我前面没有人。 漫游好了
        {
            return Vector3.zero;
        }
        else
        {
            // 目标目标位置
            centerOfMass = centerOfMass / count;

            return steeringBasics.Arrive(centerOfMass);
        }
    }
}
