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
            newDir /= neighbours.Count;

            newDir.z = 0; //cos 2d space

            neighbours.Clear(); //resetting the cached memory
        }

        return newDir;
    }
}
