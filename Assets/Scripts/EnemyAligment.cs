using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAligment : SteeringComponent
{
    //public float maxForce;
    public float distance;

    List<Transform> neighbours;

    public override Vector3 CalcSteeringDir()
    {
        Vector3 desiredVel = Vector3.zero;
        steeringDir = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                desiredVel += otherTrans.up; //????
            }

            //'normalize' it with the group size (cant go over magnitude of 1)
            desiredVel.z = 0; //cos 2d space
            desiredVel /= neighbours.Count;

            //desiredVel *= myHandler.steerMaxForce;

            //make it a force to the desired heading (velocity)
            //   but this makes the weight of this component more important (turning speed?)
            //      TODO: make it scale with maxForce
            //steeringDir = desiredVel - (Vector3)myRigidbody.velocity;
            steeringDir = desiredVel; //TEMP
            steeringDir.z = 0; //cos 2d space

            //TODO: make it proprtional to the max force we can use (maxForce / maxVel

        }
        steeringMag = steeringDir.magnitude;

        if (debugLineLength > 0)
        {
            if (debugShowDesired)
                DrawDebugLine(desiredVel, 1, debugArrowSize);
            //else
                DrawDebugLine(steeringDir, steeringWeight, 0);
        }

        return steeringDir;
    }
}
