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
    public float debugArrowSize = 0.5f;
    public Color debugLineColor = Color.blue;
    public bool debugShowDesired = true;

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
        //steeringDir = CalcSteeringDir();
        //myHandler.AddSteerDir(steeringDir, steeringWeight);

        //if (debugLineLength > 0)
        //    DrawDebugLine();
    }

    public abstract Vector3 CalcSteeringDir();

    public virtual void DrawDebugLine(Vector3 dir, float weight, float arrowSize)
	{
        //curPos = myTrans.position;
        //Debug.DrawLine(curPos, curPos + (dir * debugLineLength * weight), debugLineColor, Time.deltaTime);

        Vector3 endPoint = myTrans.position + (dir * weight *  debugLineLength);

        Debug.DrawLine(myTrans.position, endPoint, debugLineColor, Time.deltaTime);

        if (arrowSize > 0 && dir.magnitude > 0.1f)
        {
            float arrowLength = 0.5f * arrowSize;
            float arrowWideness = 0.33f * arrowSize;
            Vector3 arrowBackDir = (myTrans.position - endPoint).normalized;
            Vector3 arrowLeftDir = Vector3.Cross(arrowBackDir, Vector3.forward).normalized;
            Vector3 arrowTmpPoint = endPoint + (arrowBackDir * arrowLength);

            Debug.DrawLine(endPoint, arrowTmpPoint + (arrowLeftDir * arrowWideness), debugLineColor, Time.deltaTime);
            Debug.DrawLine(endPoint, arrowTmpPoint + (-arrowLeftDir * arrowWideness), debugLineColor, Time.deltaTime);

        }
    }
}
