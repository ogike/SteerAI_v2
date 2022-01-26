using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Gets the direction/distance to player from the EnemyHandler every frame
 * Moves towards player in a straight lines
 *      Only if the distance to playes is smaller than "distToStart", and bigger than "distToStop"
 */

public class EnemySeek : SteeringComponent
{
    public float moveSpeed;
    public float steerWeight;
    //public float distToStop; //if within this distance from the player, we wont move
    //public float distToStart; //if farther than this distance from the player, we wont move

    //TEMPORARY, csak arra van hogyha megnyitod a játékot ne egybõl rohanjanak


    public Transform targetTrans;

    Vector3 dirToTarget; //the normalized direction
    float   distToTarget;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //automatically set it as the player thru the gameManager
        targetTrans = GameManagerScript.Instance.playerTransform;
    }

    //FIXME: (?) was originally in LateUpdate
	public override Vector3 CalcSteeringDir()
	{
        //get the vector data from the enemy handler
        //TODO: this only made sense with the group project, replace
        dirToTarget = myHandler.GetDirToPlayer();
        distToTarget = myHandler.GetDistToTarget();

        //myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        //if (distToTarget > distToStop && distToStart > distToTarget)
        {
            Vector3 desiredVel = (targetTrans.position - myTrans.position)
                                    .normalized
                                    * myHandler.steerMaxSpeed; //TODO: shoudlnt i replace moveSpeed?

            //make it into a force
            //TODO: force/vel constant?

            steeringDir = desiredVel - (Vector3)myRigidbody.velocity;

            return steeringDir;
        }
    }
}
