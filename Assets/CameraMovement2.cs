using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement2 : MonoBehaviour {

    public Transform target;

    private Vector3 displacement;

    private void Start()
    {
        // 记住两个的位置差异
        displacement = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (!target)
        {
            transform.position = target.position + displacement;
        }
    }
}
