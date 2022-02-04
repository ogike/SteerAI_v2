using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringSeparation : SteeringComponent
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

            foreach (Transform otherTrans in neighbours)
            {
                Vector3 toOther = (curPos - otherTrans.position);

                //TOTEST: inversely proportional to the distance
                steeringDir += toOther
                                .normalized
                                * (distance / toOther.magnitude)
                                * myHandler.steerMaxForce;
            }

            steeringDir.z = 0; //cos 2d space

            //TODO: this could be made smoother depending on distance
            //separationDir = separationDir.normalized * maxForce;
            //newDir.Normalize();
            //newDir /= neighbours.Count;
        }

        steeringMag = steeringDir.magnitude;
        return steeringDir;
    }
}
 