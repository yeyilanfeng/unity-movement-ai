using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SteeringBasics))]
public class FollowPath : MonoBehaviour {
    // 用于结束的判断
    public float stopRadius = 0.005f;
	
    // 偏移量
	public float pathOffset = 0.71f;

    // 路径方向
	public float pathDirection = 1f;

    // 其他组件
	private SteeringBasics steeringBasics;
	private Rigidbody rb;

	void Start () {
		steeringBasics = GetComponent<SteeringBasics> ();
		rb = GetComponent<Rigidbody> ();
	}

    // 得到线性加速度
	public Vector3 getSteering (LinePath path) {
		return getSteering (path, false);
	}


	public Vector3 getSteering (LinePath path, bool pathLoop) {
		Vector3 targetPosition;
		return getSteering(path, pathLoop, out targetPosition);
	}

	public Vector3 getSteering (LinePath path, bool pathLoop, out Vector3 targetPosition) {

        // 如果路径只有一个节点, 那么只需转到该位置
        if (path.Length == 1) {
			targetPosition = path[0];
		}
        //否则, 在该路径上找到最接近的点, 然后转到该位置。
        else
        {
            if (!pathLoop)
            {
                /* 查找此路径上的 最终目标 */
                Vector2 finalDestination = (pathDirection > 0) ? path[path.Length - 1] : path[0];

                // 如果我们足够接近最终目的地,  就停止
                /* If we are close enough to the final destination then either stop moving or reverse if 
                 * the character is set to loop on paths */
                if (Vector2.Distance(transform.position, finalDestination) < stopRadius)
                {
                    targetPosition = finalDestination;

                    rb.velocity = Vector2.zero;
                    return Vector2.zero;
                }
            }

            /* 在给定路径上，得到最接近的位置点的参数*/
            float param = path.getParam(transform.position);

            /* 向下移动的路径 */
            param += pathDirection * pathOffset;
			
			/* 得到目标位置 */
			targetPosition = path.getPosition(param, pathLoop);
		}
		
		return steeringBasics.Arrive(targetPosition);
	}
}
