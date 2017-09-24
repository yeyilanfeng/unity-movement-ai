using UnityEngine;
using System.Collections;

public class EvadeUnit : MonoBehaviour
{
    public Rigidbody target;

    private SteeringBasics steeringBasics;
    private Evade evade;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        evade = GetComponent<Evade>();
    }

    void Update()
    {
        Vector3 accel = evade.GetSteering(target);

        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();
    }
}
