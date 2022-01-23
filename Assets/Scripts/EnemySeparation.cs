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
        steeringDir = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            foreach (Transform otherTrans in neighbours)
            {
                Vector3 newDir = curPos - otherTrans.position;

                //make the force inversely proportional to the distance
                newDir *= ( 1 - (newDir.magnitude / distance) );
                steeringDir += newDir;
            }

            steeringDir.z = 0; //cos 2d space

            //TODO: this could be made smoother depending on distance
            //separationDir = separationDir.normalized * maxForce;
            //newDir.Normalize();
            steeringDir /= neighbours.Count;
            steeringDir *= maxForce;
        }

        return steeringDir;
    }
}
 