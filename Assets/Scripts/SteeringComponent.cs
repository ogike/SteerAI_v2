using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHandler))]
public abstract class SteeringComponent : MonoBehaviour
{
    protected RoomHandler myRoomHandler;
    protected Transform myTrans;
    protected Rigidbody2D myRigidbody;
    protected EnemyHandler myHandler;

    protected Vector3 curPos;
    public Vector3 steeringDir;
    public float steeringMag;

    public float steeringWeight = 1;

    //TODO: centralize debugging
    public float debugLineLength = 1; //0 if no debug
    public Color debugLineColor = Color.blue;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myTrans     = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myHandler   = GetComponent<EnemyHandler>();

        //TODO: in enemyHandler
        myRoomHandler = myTrans.parent.GetComponent<RoomHandler>();

        steeringDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        steeringDir = CalcSteeringDir();
        //myHandler.AddSteerDir(steeringDir, steeringWeight);

        //if (debugLineLength > 0)
        //    DrawDebugLine();
    }

    public abstract Vector3 CalcSteeringDir();

    public virtual void DrawDebugLine()
	{
        curPos = myTrans.position;
        Debug.DrawLine(curPos, curPos + (steeringDir * debugLineLength), debugLineColor, Time.deltaTime);
    }
}
