using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Can be used as a template for new movement behaviours
 * Gets the direction/distance to player from the EnemyHandler every frame
 *      So this needs an "EnemyHandler" component on the same GameObject this is attached to, or it wont work!
 * Moves towards player in a straight lines
 *      Only if the distance to playes is smaller than "distToStart", and bigger than "distToStop"
 */

[RequireComponent(typeof(EnemyHandler))]
public class EnemyBasicFollow : MonoBehaviour
{
    public float moveSpeed;
    public float steerWeight;
    //public float distToStop; //if within this distance from the player, we wont move
    //public float distToStart; //if farther than this distance from the player, we wont move

    //TEMPORARY, csak arra van hogyha megnyitod a játékot ne egybõl rohanjanak

    EnemyHandler myHandler;

    Transform targetTrans;
    Transform myTrans;
    Rigidbody2D myRigidbody;

    Vector3 dirToTarget; //the normalized direction
    float   distToTarget;


    // Start is called before the first frame update
    void Start()
    {
        myHandler   = GetComponent<EnemyHandler>();
        myTrans     = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();

        targetTrans = GameManagerScript.Instance.playerTransform; //automatically set it as the player thru the gameManager
    }

    // Update is called once per frame
    // LateUpdate is the same, but its called after the normal Update()-s, which means it will be after the EnemyScripts's Update, and that all the variables will be up-to-date
    void LateUpdate()
    {
        //get the vector data from the enemy handler
        dirToTarget = myHandler.GetDirToPlayer();
        distToTarget = myHandler.GetDistToTarget();

        //myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        //if (distToTarget > distToStop && distToStart > distToTarget)
        {
            MoveInDir(dirToTarget);
        }
    }

    void MoveInDir(Vector3 dir)
    {
        myHandler.AddSteerDir(dir * moveSpeed, steerWeight);
        //myRigidbody.AddForce(dir * moveSpeed * Time.deltaTime);
    }
}
