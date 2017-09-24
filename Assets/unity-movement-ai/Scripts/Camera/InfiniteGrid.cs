using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 这个类将在x / y方向上为相机创建一个无限的网格。把这个脚本放在你的相机上。
 */
public class InfiniteGrid : MonoBehaviour { 
	
	public bool show = true; 
	
	public float cellSize = 1;

	public float zPosition = 0;

	private float[] GetGridBounds() {
		float distToGrid = zPosition - transform.position.z;

		float angle = (GetComponent<Camera>().fieldOfView / 2) * Mathf.Deg2Rad;

		float halfHeight = Mathf.Tan (angle) * distToGrid;
		float halfWidth = GetComponent<Camera>().aspect * halfHeight;

		float[] bounds = new float[4];

		// Get the camera bounds
		bounds [0] = transform.position.x - halfWidth;
		bounds [1] = transform.position.y - halfHeight;
		bounds [2] = transform.position.x + halfWidth;
		bounds [3] = transform.position.y + halfHeight;

		// Convert the camera bounds to the grid bounds
		bounds [0] = Mathf.Floor(bounds [0] / cellSize) * cellSize;
		bounds [1] = Mathf.Floor(bounds [1] / cellSize) * cellSize;
		bounds [2] = Mathf.Ceil(bounds [2] / cellSize) * cellSize;
		bounds [3] = Mathf.Ceil(bounds [3] / cellSize) * cellSize;

		return bounds;
	}

    public Material lineMat;
    // GL 是Unity提供很底层的API, 可以画一些图形
    void OnPostRender() 
	{
		GL.Begin( GL.LINES );
		
		if(show)
		{
			Material lineMaterial = lineMat;
			lineMaterial.SetPass( 0 );

			float[] bounds = GetGridBounds ();
			
			//X axis lines
			for(float j = 0; bounds[1] + j <= bounds[3]; j += cellSize)
			{
				GL.Vertex3( bounds[0], bounds[1] + j, zPosition);
				GL.Vertex3( bounds[2], bounds[1] + j, zPosition);
			}
			
			//Y axis lines
			for(float k = 0; bounds[0] + k <= bounds[2]; k += cellSize)
			{
				GL.Vertex3( bounds[0] + k, bounds[1], zPosition);
				GL.Vertex3( bounds[0] + k, bounds[3], zPosition);
			}
		}
		
		GL.End();
	}
}