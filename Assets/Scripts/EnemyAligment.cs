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
        steeringDir = myRigidbody.velocity.normalized; //could be myTrans.up too?

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the avarage heading
            foreach (Transform otherTrans in neighbours)
            {
                steeringDir += otherTrans.up; //could be the velocity of otherTrans too?
                                         //point is it should be normalizd of where they are headed)
            }
            steeringDir /= neighbours.Count;
            steeringDir.z = 0; //cos 2d space

            //calculating the desired vel
            steeringDir *= maxSpeed;

            //calculating the force
            //steeringDir = steeringDir - (Vector3)myRigidbody.velocity;
            steeringDir *= (maxForce / maxSpeed);
        }

        return steeringDir;
    }
}
