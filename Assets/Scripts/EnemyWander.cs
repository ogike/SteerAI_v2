using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHandler))]
public class EnemyWander : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public float steerBehaviourWeight = 1;

    public float decideTimeMin;
    public float decideTimeMax;

    public float collisionCheckTime; //how much time there is between 2 raycast checks
    float curColCheckTime;

    public float wallAvoidanceDist = 2f;
    public LayerMask wallAvoidLayers;

    public float wallAvoidanceSideVecDiff; //how much the side raycasts differ from the original in angles

    public int maxNumOfDecideTries = 20; //how many times we can try for a random direction before disabling
    public float timeUntilReenable = 2; //how many seconds until this is reenabled after we are stuck in an endless loop
                                        //TODO: make this prettier/easier to understand and handle

    public bool debugVisuals;
    public float debugLineTimes; //for how long the debug lines stay on

    float curDecideTime;
    Vector3 curDir;

    bool isDisabled; //safety mechanism if we are stuck in an endless loop during decision making
                     //not the same things as the component being disabled!!
    float curTimeUntilReenable;

    Rigidbody2D myRigidbody;
    Transform myTrans;
    EnemyHandler myHandler;

    RaycastHit2D tempRayHit;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myTrans = GetComponent<Transform>();
        myHandler = GetComponent<EnemyHandler>();

        //setting it to a random number at the start
        //so that different enemies wont visibly update at the same time
        curColCheckTime = Random.Range(0, collisionCheckTime); 

        ChooseNewdir();

        isDisabled = false;
    }

    //this is really ugly at this point
    void Update()
    {
        if(isDisabled)
		{
            if(curTimeUntilReenable > 0)
			{
                curTimeUntilReenable -= Time.deltaTime;
                return;
            }
            else
			{
                isDisabled = false;
			}
        }

        if(curDecideTime > 0)
		{
            curDecideTime -= Time.deltaTime;

            //if we already have a direction, we check if the directions is still good (doesnt lead into a wall)
            if(curColCheckTime > 0)
			{
                curColCheckTime -= Time.deltaTime;
			}
            else
			{
                CheckForCollisions();
                curColCheckTime = collisionCheckTime; //resetting the timer
            }
        }
        else
		{
            ChooseNewdir();
        }

        myHandler.AddSteerDir(curDir, steerBehaviourWeight);
        //LookInDir(curDir);
    }

	private void FixedUpdate()
	{
       // if (debugVisuals)
       //     DebugUpdate();
    }

    void CheckForCollisions()
	{
        if (CheckRaycastHit(curDir))
        {
            DrawDebugLine(curDir, Color.red);

            //we should check for the other directions first
            ChooseNewdir();
        }
        else
		{
            DrawDebugLine(curDir, Color.green);

            Vector3 leftDirVector = Quaternion.Euler(0, 0, wallAvoidanceSideVecDiff) * curDir;
            Vector3 rightDirVector = Quaternion.Euler(0, 0, -wallAvoidanceSideVecDiff) * curDir;

            if (CheckRaycastHit(leftDirVector))
            {
                DrawDebugLine(leftDirVector, Color.red);

                if (CheckRaycastHit(rightDirVector))
                    ChooseNewdir();
				else //if the right vector didnt hit, its probably a good new direction
				{
                    DrawDebugLine(rightDirVector, Color.yellow);

                    curDir = rightDirVector;
                    curDir.Normalize();
				}
            }
            else
            {
                DrawDebugLine(leftDirVector, Color.green);

                if (CheckRaycastHit(rightDirVector))
                {
                    DrawDebugLine(rightDirVector, Color.red);

                    DrawDebugLine(leftDirVector, Color.yellow);
                    //ChooseNewdir();
                    //we got to this statement because the left direction was green
                    //so we can probably use that as the new direction
                    curDir = leftDirVector;
                    curDir.Normalize();
                }
                else
				{
                    DrawDebugLine(rightDirVector, Color.green);
                }
            }
        }
    }

    //returns true if the raycast hit sth
    bool CheckRaycastHit (Vector2 checkDir)
	{
        tempRayHit = Physics2D.Raycast(myTrans.position, checkDir, wallAvoidanceDist, wallAvoidLayers);

        return tempRayHit.collider != null;
    }

    void DrawDebugLine (Vector3 dirVec, Color col)
	{
        if (debugVisuals)
            Debug.DrawLine(myTrans.position, myTrans.position + (dirVec * wallAvoidanceDist), col, collisionCheckTime);
    }

	void ChooseNewdir ()
	{
        bool isCorrectDir = true; //false if the next dir sends us into a wall
        int curTries = 0;

        do
        {
            curDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            isCorrectDir = !CheckRaycastHit(curDir); //if the raycast didnt hit anything, its a correct direction

			if (!isCorrectDir)
			{
                //DEBUG!!
                //Debug.LogWarning("Wanderer found obstacle in dir: (" + curDir.x + ", " + curDir.y + "), with name: " + tempRayHit.transform.name);
			}

            curTries++;
            if(curTries >= maxNumOfDecideTries) //if we are stuck in an endless loop
            {
                //Debug.LogWarning("This wanderers decision making is stuck in an endless loop, disabling");

                isDisabled = true;
                curTimeUntilReenable = timeUntilReenable;

                return;
			}

        } while (!isCorrectDir);

        curDir.Normalize(); //normalize so we can use as direction vector during Move()

        curDecideTime = Random.Range(decideTimeMin, decideTimeMax);
    }

    void LookInDir(Vector3 targDir)
	{
        float angleToTarget = Mathf.Atan2(targDir.x, targDir.y) * Mathf.Rad2Deg * (-1);
        Quaternion targRot = Quaternion.Euler(0, 0, angleToTarget);

        //???
        myTrans.rotation = Quaternion.Slerp(myTrans.rotation, targRot, rotationSpeed * Time.deltaTime);
    }

   // void MoveInCurDir()
    //{
    //    myRigidbody.AddForce(myTrans.up * moveSpeed * Time.deltaTime);
        //myRigidbody.AddForce(curDir * moveSpeed * Time.deltaTime);
    //}

    //for debug visuals
    void DebugUpdate ()
    {
        Debug.DrawLine(myTrans.position, myTrans.position + (curDir * wallAvoidanceDist), Color.green, Time.fixedDeltaTime);
    }
}
