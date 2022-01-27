using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeparation : SteeringComponent
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

            foreach (Transform otherTrans in neighbours)
            {
                Vector3 toOther = curPos - otherTrans.position;

                //make the force inversely proportional to the distance
                newDir += toOther.normalized / toOther.magnitude;
            }

            newDir.z = 0; //cos 2d space

            //separationDir = separationDir.normalized * maxForce;
            //newDir.Normalize();
            //newDir /= neighbours.Count;

            //Note: we dont divide by neighbour count, so the inversiely proportional forces can work properly
            //      this way, we get a proper force too
            //      but it isnt guaranteed that we will stay inside maxForce...?

            neighbours.Clear(); //resetting the cached memory
        }

        steeringDir = newDir;
        steeringMag = steeringDir.magnitude;
        if (debugLineLength > 0)
            DrawDebugLine();
        return newDir;
    }
}
 