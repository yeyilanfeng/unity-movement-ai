using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 接近传感器， 保存正在和我接触的，就是发生碰撞的
/// </summary>
public class NearSensor : MonoBehaviour {

	public HashSet<Rigidbody> targets = new HashSet<Rigidbody>();

	void OnTriggerEnter(Collider other) {
		targets.Add (other.GetComponent<Rigidbody>());
	}
	
	void OnTriggerExit(Collider other) {
		targets.Remove (other.GetComponent<Rigidbody>());
	}
}
