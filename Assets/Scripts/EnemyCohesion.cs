using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCohesion : SteeringComponent
{
    //public float maxForce;
    public float distance;

    public float steerBehaviourWeight = 1;

    public Vector3 centerOfMass;
    public Transform centerDebugObject;
    public GameObject centerDebugPrefab;
    Vector2 smoothTempVel; //temp for smoothDamp

    //TEMP: not public
    public List<Transform> neighbours;

    protected override void Start()
    {
        base.Start();

        if (centerDebugObject == null)
            centerDebugObject = GameObject.Instantiate(centerDebugPrefab, myTrans, true).transform;
    }

    public override Vector3 CalcSteeringDir()
    {
        steeringDir = Vector3.zero;
        centerOfMass = myTrans.position;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                centerOfMass += otherTrans.position;
            }

            //the center is the avarage of the sum of the positions
            centerOfMass /= neighbours.Count + 1;
            
            if(centerDebugObject != null)
			{
                centerDebugObject.position = centerOfMass;
			}

            Vector3 toCenter = (centerOfMass - myTrans.position);

            Vector3 desiredVel = toCenter
                                    .normalized
                                    * myHandler.steerMaxSpeed
                                    * (toCenter.magnitude / distance); //make it proportional to the distance 

            steeringDir = desiredVel - (Vector3)myRigidbody.velocity;
        }

        steeringMag = steeringDir.magnitude;

        if (debugLineLength > 0)
            DrawDebugLine();

        return steeringDir;
    }
}
