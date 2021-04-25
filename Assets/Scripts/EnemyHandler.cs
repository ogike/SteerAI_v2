using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is like the manager of the enemy, handles all the basic stuff
 * For now, it calculates the distance/direction to the player every frame
 *      This data can be accesed with the GetVectorData function
 *      So every enemy behaviour doesnt have to calculate them each frame
 * This should be added to every enemy!!
 */

/* Now this is also a steering agent for now
 * lol.
 * Im just prototyping at this point
 */

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHandler : MonoBehaviour
{
    Transform targetTrans;
    Transform myTrans;
    Rigidbody2D myRigidbody;

    Vector3 dirToTarget; //the normalized direction to the player
    float distToTarget;  //the distance to the player

    public float driveFactor = 10; //a final constant factor for the speed, so we can set the overall speed at one place

    public Vector3 curSteerVel; //why the fuck am i using vector3 here
    public Vector2 curSteerAcc;
    //public float steerForce; //old
    public float steerMaxForce;
    public float steerMaxSpeed;
    float steerMaxSpeedSqr;

    public float rotationSpeed;
    public float plusRotation;

    public bool canMove = true;
    public bool debugVisuals;

    public float debugLineLengthFactor = 1f;

    //public Vector2 curSteelVelDebug;
    //public Vector2 curRigidVelDebug;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        targetTrans = GameManagerScript.Instance.playerTransform; //automatically set it as the player thru the gameManager

        myTrans.position = new Vector3(myTrans.position.x, myTrans.position.y, 0); //setting z position to be 0

        steerMaxSpeedSqr = Mathf.Pow(steerMaxSpeed, 2);
    }

	// Update is called once per frame
	void Update()
    {
        dirToTarget = targetTrans.position - myTrans.position; //elõször csak megszerezzük az irányt, hosszal együtt
        distToTarget = dirToTarget.magnitude; //kinyerjül ebbõl a távolságot (hosszát a vektornak)
        dirToTarget.Normalize(); //utána már csak a normalizált irányvektor érdekel minket
    }

	private void LateUpdate()
	{
        //after all the other calculations are done, we can move
        if(canMove)
            Move();
        LookInDir(myRigidbody.velocity.normalized); //TODO optimize
        curSteerVel = Vector3.zero; //resetting for next frame
    }

	public void AddSteerDir(Vector3 plusDir, float weight)
	{
        curSteerVel += plusDir * weight;

        //if(curSteerVel.magnitude > weight
        //  curSteerVel = curSteerVel.normalized * weight;
	}

	void Move()
	{
        curSteerVel *= Time.deltaTime;

        //not the best, since this overrides any external forces
        curSteerVel = Vector3.ClampMagnitude(curSteerVel, steerMaxForce) * driveFactor;
        curSteerAcc = curSteerVel / myRigidbody.mass;
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity + curSteerAcc, steerMaxSpeed);


        /*myRigidbody.velocity += (Vector2)curSteerVel;

        //if (steerMaxVelSqr < myRigidbody.velocity.sqrMagnitude)
        if (steerMaxSpeed < myRigidbody.velocity.magnitude)
            myRigidbody.velocity = myRigidbody.velocity.normalized * steerMaxSpeed;*/

        if (debugVisuals)
            DrawDebugVisuals();

        //curRigidVelDebug = myRigidbody.velocity;
        //curSteelVelDebug = curSteerVel;

    }

	void MoveOld()
	{
        curSteerVel.z = 0; //safety, probably should be done better with proper types

        //TODO: replace this with squared checks
        /*if (curSteerVel.magnitude > steerMaxVel)
            curSteerVel = curSteerVel.normalized * steerMaxVel;

        myRigidbody.AddForce(curSteerVel * steerForce * Time.deltaTime);*/

        myRigidbody.AddForce(myTrans.up * steerMaxSpeed * Time.deltaTime); //just move forward

        //curSteerVel = Vector3.zero; //resetting for next frame
    }

    void LookInDir(Vector3 targDir)
    {
        transform.up = targDir;

        /* smooth
        float angleToTarget = Mathf.Atan2(targDir.x, targDir.y) * Mathf.Rad2Deg * (-1);
        Quaternion targRot = Quaternion.Euler(0, 0, angleToTarget);

        myTrans.rotation = Quaternion.Slerp(myTrans.rotation, targRot, rotationSpeed * Time.deltaTime);
        */

        //float rotZ = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg;  //turn the direction vector into an angle with trigonometry
        //myTrans.rotation = Quaternion.Euler(0, 0, rotZ - plusRotation); //set the z rotation
    }

    public Vector3 GetDirToPlayer ()
	{
        return dirToTarget;
	}

    public float GetDistToTarget ()
	{
        return distToTarget;
	}

    //parameters are references, which are like pointers
    //basically "ref" in c# = "&" in c++
    public void GetTargetVectorData (ref Vector3 dir, ref float dist)
	{
        dir = dirToTarget;    //updated the passed direction variable to the current one
        dist = distToTarget;  //same here
	}

    void DrawDebugVisuals()
	{
        Debug.DrawLine( myTrans.position, myTrans.position + 
            ((Vector3)myRigidbody.velocity * debugLineLengthFactor), Color.green, Time.deltaTime );
	}
}
