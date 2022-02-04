using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Can be used as a template for new movement behaviours
 * Gets the direction/distance to player from the EnemyHandler every frame
 *      So this needs an "EnemyHandler" component on the same GameObject this is attached to, or it wont work!
 * Moves towards player in a straight lines
 *      Only if the distance to playes is smaller than "distToStart", and bigger than "distToStop"
 */

public class SteeringSeek : SteeringComponent
{
    //public float moveSpeed;
    //public float distToStop; //if within this distance from the player, we wont move
    //public float distToStart; //if farther than this distance from the player, we wont move

    //TEMPORARY, csak arra van hogyha megnyitod a játékot ne egybõl rohanjanak


    Transform targetTrans;

    Vector3 dirToTarget; //the normalized direction
    float   distToTarget;
    public bool test;


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
        dirToTarget = myHandler.GetDirToPlayer();
        distToTarget = myHandler.GetDistToTarget();

        //myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        //if (distToTarget > distToStop && distToStart > distToTarget)
        {
            steeringDir = dirToTarget * myHandler.steerMaxSpeed;
            steeringMag = steeringDir.magnitude;
            return steeringDir;
        }
    }
}
