using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCohesion : SteeringComponent
{
    //public float maxForce;
    public float distance;

    public float smoothTime = 0.5f;
    Vector2 smoothTempVel; //temp for smoothDamp

    List<Transform> neighbours;

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
                steeringDir += otherTrans.position;
            }
            steeringDir /= neighbours.Count; //the center of mass to move forward to

            //create offset from our position
            steeringDir -= curPos;  //the desired velocity
            steeringDir.z = 0; //cos 2d space

            steeringDir *= (maxSpeed / steeringDir.magnitude);

            //TODO: why am i using a smooth function for sth that runs every frame???
            //from YT Unity Flocking tut part 5B
            //newDir = Vector2.SmoothDamp(myTrans.up, newDir, ref smoothTempVel, smoothTime);

            //TODO: TEST
            //steeringDir = steeringDir - (Vector3)myRigidbody.velocity; //the wanted force
            steeringDir *= (maxForce / maxSpeed);

        }

        return steeringDir;
    }
}
