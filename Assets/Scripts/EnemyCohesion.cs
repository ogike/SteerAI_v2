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
    Vector2 smoothTempVel; //temp for smoothDamp

    //TEMP: not public
    public List<Transform> neighbours;

    public override Vector3 CalcSteeringDir()
    {
        Vector3 newDir = Vector3.zero;
        centerOfMass = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                if(otherTrans.gameObject.activeInHierarchy) //TODO: optimize
				{
                    centerOfMass += otherTrans.position;
				}
            }

            //the center is the avarage of the sum of the positions
            centerOfMass /= neighbours.Count;
            
            if(centerDebugObject != null)
			{
                centerDebugObject.position = centerOfMass;
			}

            //Seek towards centerOfMass
            Vector3 desiredVel = (centerOfMass - myTrans.position)
                                    .normalized
                                    * myHandler.steerMaxSpeed;
            newDir = desiredVel - (Vector3)myRigidbody.velocity;
        }
        steeringDir = newDir;
        steeringMag = steeringDir.magnitude;
        if (debugLineLength > 0)
            DrawDebugLine();
        return newDir;
    }
}
