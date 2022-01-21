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
                newDir += curPos - otherTrans.position;
            }

            newDir.z = 0; //cos 2d space

            //TODO: this could be made smoother depending on distance
            //separationDir = separationDir.normalized * maxForce;
            //newDir.Normalize();
            newDir /= neighbours.Count;

            neighbours.Clear(); //resetting the cached memory
        }

        return newDir;
    }
}
 