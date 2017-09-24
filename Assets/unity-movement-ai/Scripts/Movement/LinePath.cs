using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class LinePath  {
    // 路径上的节点
	public Vector3[] nodes;
    // 节点间距离
	private float[] distances;
    // 最大距离（起始节点 到 最后节点的距离）
    [System.NonSerialized]
	public float maxDist;

	// 定义索引器
	public Vector3 this[int i]
	{
		get
		{
			return nodes[i];
		}
		
		set
		{
			nodes[i] = value;
		}
	}

	public int Length
	{
		get {
			return nodes.Length;
		}
	}

    // 最后一个节点
	public Vector3 endNode {
		get {
			return nodes[nodes.Length-1];
		}
	}

    ///* 构造函数 */
    //public LinePath(Vector3[] nodes) {
    //	this.nodes = nodes;

    //	calcDistances();
    //}

    /* 遍历路径的节点，并确定路径中每个节点到起始节点的距离 */
    public void calcDistances() {
		distances = new float[nodes.Length];
		distances[0] = 0;

		for(var i = 0; i < nodes.Length - 1; i++) {
			distances[i+1] = distances[i] + Vector3.Distance(nodes[i], nodes[i+1]);
		}
		
		maxDist = distances[distances.Length-1];
	}
	
	/* 为了调试 ，在场景中绘制路径线 */
	public void draw() {
		for (int i = 0; i < nodes.Length-1; i++) {
			Debug.DrawLine(nodes[i], nodes[i+1], Color.cyan, 0.0f, false);
		}
	}

    /* 得到在 路径上最接近的点的参数*/
    public float getParam(Vector3 position) {
        // 找到这个点 最接近的路径  的索引
		int closestSegment = getClosestSegment(position);
		
		float param = this.distances[closestSegment] + getParamForSegment(position, nodes[closestSegment], nodes[closestSegment+1]);
		
		return param; 
	}

    /* 找到最接近路径的索引 */
    public int getClosestSegment(Vector3 position) {
        // 遍历到所有线段的 距离
        float closestDist = distToSegment(position, nodes[0], nodes[1]);
		int closestSegment = 0;
		
		for(int i = 1; i < nodes.Length - 1; i++) {
			float dist = distToSegment(position, nodes[i], nodes[i+1]);
			
			if(dist <= closestDist) {
				closestDist = dist;
				closestSegment = i;
			}
		}

		return closestSegment;
	}

    /* 给定一个参数，它得到路径上的位置*/
    public Vector3 getPosition(float param, bool pathLoop = false) {
        /* 不要越界，    确保param没有经过开始或结束的路径 */
        if (param < 0) {
			param = (pathLoop) ? param + maxDist : 0;
		} else if (param > maxDist) {
			param = (pathLoop) ? param - maxDist : maxDist;
		}

        /* Find the first node that is farther than given param */
        // 找到比给定的param还要远的第一个节点
        int i = 0;
		for(; i < distances.Length; i++) {
			if(distances[i] > param) {
				break;
			}
		}

        /* Convert it to the first node of the line segment that the param is in */
        // 将其转换为param所在的行段的第一个节点
        if (i > distances.Length - 2) {
			i = distances.Length - 2;
		} else {
			i -= 1;
		}

        /* Get how far along the line segment the param is */
        // 沿着这条线段走多远
        float t = (param - distances[i]) / Vector3.Distance(nodes[i], nodes[i+1]);

        /* Get the position of the param */
        // 得到该参数的位置
        return Vector3.Lerp(nodes[i], nodes[i+1], t);
	}


    // 给出一个点到线段的距离。p是点，v和w是线段的两个点
    private static float distToSegment(Vector3 p, Vector3 v, Vector3 w) { 
		Vector3 vw = w - v;
		
		float l2 = Vector3.Dot(vw, vw);

        if (l2 == 0)
        {  // 两点重合
            return Vector3.Distance(p, v);
		}
		
		float t = Vector3.Dot(p - v, vw) / l2;
		
		if (t < 0) {   // 以 v 点为钝角   (离v最近)
			return Vector3.Distance(p, v);
		}
		
		if (t > 1) {  // 以 w 为钝角 （理解为V为锐角， 但是pv 长度大于 vw）（离w最近）
			return Vector3.Distance(p, w);
		}
		
        // 在线段上的点    （t 在 0~1 之间， 正好是插值的时间），  逻辑帧牛逼！
		Vector3 closestPoint = Vector3.Lerp(v, w, t);
		
		return Vector3.Distance(p, closestPoint);
	}


    // 找到与所给出的线段vw，最近点p的参数
    // 跟函数 distToSegment 类似，  p在vw线段上，距离v的长度
    private static float getParamForSegment(Vector3 p, Vector3 v, Vector3 w) {
		Vector3 vw = w - v;

		float l2 = Vector3.Dot(vw, vw);  
		
		if (l2 == 0)
        {// 两点重合
            return 0;
		}
		
		float t = Vector3.Dot(p - v, vw) / l2;
		
		if(t < 0) {
			t = 0;
		} else if (t > 1) {
			t = 1;
		}
		
		return t * Mathf.Sqrt(l2);
	}

 //   // 移除几点
	//public void removeNode(int i ) {
	//	Vector3[] newNodes = new Vector3[nodes.Length - 1];

	//	int newNodesIndex = 0;
	//	for (int j = 0; j < newNodes.Length; j++) {
	//		if(j != i) {
	//			newNodes[newNodesIndex] = nodes[j];
	//			newNodesIndex++;
	//		}
	//	}

	//	this.nodes = newNodes;
		
	//	calcDistances();
	//}

    // 反转路径，就是直接把节点反转， 然后重新初始化一下内容
    public void reversePath()
    {
        Array.Reverse(nodes);

        calcDistances();
    }
}
