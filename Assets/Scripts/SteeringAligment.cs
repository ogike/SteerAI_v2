using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAligment : SteeringComponent
{
    //public float maxForce;
    public float distance;

    public List<Transform> neighbours;

    public override Vector3 CalcSteeringDir()
    {
        steeringDir = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                steeringDir += otherTrans.up; //????
            }
            steeringDir /= neighbours.Count;

            steeringDir.z = 0; //cos 2d space
        }

        return steeringDir;
    }
}
