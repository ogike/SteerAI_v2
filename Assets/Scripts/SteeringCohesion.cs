using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCohesion : SteeringComponent
{
    //public float maxForce;
    public float distance;

    public float steerBehaviourWeight = 1;

    public float smoothTime = 0.5f;
    Vector2 smoothTempVel; //temp for smoothDamp

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
                newDir += otherTrans.position;
            }
            newDir /= neighbours.Count;

            //create offset from our position
            newDir -= curPos; 
            newDir.z = 0; //cos 2d space

            //TODO: why am i using a smooth function for sth that runs every frame???
            newDir = Vector2.SmoothDamp(myTrans.up, newDir, ref smoothTempVel, smoothTime);


            //TODO: this isnt needed bcos gc???
            neighbours.Clear(); //resetting the cached memory
        }

        return newDir;
    }
}
