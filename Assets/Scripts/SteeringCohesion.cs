using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringCohesion : SteeringComponent
{
    //public float maxForce;
    public float distance;

    //public float smoothTime = 0.5f;
    //Vector2 smoothTempVel; //temp for smoothDamp

    public List<Transform> neighbours;

    public override Vector3 CalcSteeringDir()
    {
        steeringDir = Vector3.zero;
        Vector3 centerOfGroup = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                centerOfGroup += otherTrans.position;
            }
            centerOfGroup /= neighbours.Count;

            //create offset from our position
            Vector3 toCenter = centerOfGroup - myTrans.position;
            toCenter.z = 0; //cos 2d space

            //NOTE: new, was just returning toCenter in old branch
            steeringDir = toCenter
                            .normalized
                            * myHandler.steerMaxForce //TOTEST: could be vel..?
                            * (toCenter.magnitude / distance); //proportional to the distance

            //TODO: why am i using a smooth function for sth that runs every frame???
            //newDir = Vector2.SmoothDamp(myTrans.up, newDir, ref smoothTempVel, smoothTime);
        }

        steeringMag = steeringDir.magnitude;
        return steeringDir;
    }
}
