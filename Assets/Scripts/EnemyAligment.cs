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
        Vector3 newDir = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                newDir += otherTrans.up; //????
            }

            //make it a normalized vector basically
            newDir.z = 0; //cos 2d space
            newDir /= neighbours.Count;

            //make it a force to the desired heading (velocity)
            //   but this makes the weight of this component more important (turning speed?)
            //      TODO: make it scale with maxForce
            newDir -= myTrans.up;
            newDir.z = 0; //cos 2d space

            //TODO: make it proprtional to the max force we can use (maxForce / maxVel
            newDir *= myHandler.steerMaxForce;

        }
        steeringDir = newDir;
        steeringMag = steeringDir.magnitude;
        if (debugLineLength > 0)
            DrawDebugLine();
        return newDir;
    }
}
