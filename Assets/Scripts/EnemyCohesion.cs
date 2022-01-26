using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCohesion : SteeringComponent
{
    //public float maxForce;
    public float distance;

    public float steerBehaviourWeight = 1;

    public float smoothTime = 0.5f;
    Vector2 smoothTempVel; //temp for smoothDamp

    List<Transform> neighbours;

    public override Vector3 CalcSteeringDir()
    {
        Vector3 newDir = Vector3.zero;
        Vector3 centerOfMass = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                centerOfMass += otherTrans.position;
            }

            //the center is the avarage of the sum of the positions
            centerOfMass /= neighbours.Count;

            //Seek towards centerOfMass
            Vector3 desiredVel = (centerOfMass - myTrans.position)
                                    .normalized
                                    * myHandler.steerMaxSpeed;
            newDir = desiredVel - (Vector3)myRigidbody.velocity;
        }
        steeringDir = newDir;
        return newDir;
    }
}
