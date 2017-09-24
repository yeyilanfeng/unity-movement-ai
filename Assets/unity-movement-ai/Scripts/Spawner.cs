using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
    // Prefab
    public Transform obj;
    // 用于随机的范围
    public Vector2 objectSizeRange = new Vector2(1, 2);
    // 一次性创建多少个
    public int numberOfObjects = 10;
    // 是否随机方向
    public bool randomizeOrientation = false;
    // 生成的内容要在  屏幕边界内
    public float boundaryPadding = 1f;
    // 生成的内容 距离现有的对象 要保持的最小距离
    public float spaceBetweenObjects = 1f;
    public Transform[] thingsToAvoid;

    private Vector3 bottomLeft;
    private Vector3 widthHeight;


    private float[] thingsToAvoidRadius;

    // 纪录现有生成的对象
    [System.NonSerialized]
    public List<Rigidbody> objs = new List<Rigidbody>();

    void Start()
    {
        // 得到屏幕大小
        float z = -1 * Camera.main.transform.position.z;

        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, z));
        widthHeight = topRight - bottomLeft;

        //  如果需要的话 要避免和场景内其他对象重叠  纪录他们的数据
        thingsToAvoidRadius = new float[thingsToAvoid.Length];

        for (int i = 0; i < thingsToAvoid.Length; i++)
        {
            thingsToAvoidRadius[i] = SteeringBasics.GetBoundingRadius(thingsToAvoid[i].transform);
        }

        // 创建就行了
        for (int i = 0; i < numberOfObjects; i++)
        {
            // 每次不一定创建成功，这里增加概率
            for(int j = 0; j < 10; j++)
            {
                if(TryToCreateObject())
                {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 尝试创建对象
    /// </summary>
    /// <returns></returns>
    private bool TryToCreateObject()
    {
        // 随机位置  和  大小
        float size = Random.Range(objectSizeRange.x, objectSizeRange.y);
        float halfSize = size / 2f;

        Vector3 pos = new Vector3();
        pos.x = bottomLeft.x + Random.Range(boundaryPadding + halfSize, widthHeight.x - boundaryPadding - halfSize);
        pos.y = bottomLeft.y + Random.Range(boundaryPadding + halfSize, widthHeight.y - boundaryPadding - halfSize);

        // 这个位置可以方式那就实例化
        if(CanPlaceObject(halfSize, pos))
        {
            Transform t = Instantiate(obj, pos, Quaternion.identity) as Transform;
            t.localScale = new Vector3(size, size, obj.localScale.z);

            if(randomizeOrientation)
            {
                Vector3 euler = transform.eulerAngles;
                euler.z = Random.Range(0f, 360f);
                transform.eulerAngles = euler;
            }

            objs.Add(t.GetComponent<Rigidbody>());

            return true;
        }

        return false;
    }

    /// <summary>
    /// 判断是否可以放置， 主要判断是否重叠
    /// </summary>
    /// <param name="halfSize"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CanPlaceObject(float halfSize, Vector3 pos)
    {
        // 确保它不会与任何东西重叠
        for (int i = 0; i < thingsToAvoid.Length; i++)
        {
            float dist = Vector3.Distance(thingsToAvoid[i].position, pos);

            if(dist < halfSize + thingsToAvoidRadius[i])
            {
                return false;
            }
        }

        //确保它不会与任何现有对象重叠
        foreach (Rigidbody o in objs)
        {
            float dist = Vector3.Distance(o.position, pos);

            float oRadius = SteeringBasics.GetBoundingRadius(o.transform);

            if (dist < oRadius + spaceBetweenObjects + halfSize)
            {
                return false;
            }
        }

        return true;
    }
}
