using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHandler))]
public class EnemyCohesion : MonoBehaviour
{
    public float debugLineLength = 1;

    //public float maxForce;
    public float distance;

    public float steerBehaviourWeight = 1;

    List<Transform> neighbours;

    Vector3 aligmentDir;

    RoomHandler myRoomHandler;
    Transform myTrans;
    Rigidbody2D myRigidbody;
    EnemyHandler myHandler;
    Vector3 curPos;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myHandler = GetComponent<EnemyHandler>();

        //TODO: in enemyHandler
        myRoomHandler = myTrans.parent.GetComponent<RoomHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        aligmentDir = CalcAligmentForce();

        myHandler.AddSteerDir(aligmentDir, steerBehaviourWeight);
        //Move(separationDir);
    }

    Vector3 CalcAligmentForce()
    {
        Vector3 newDir = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            //calculating the global position
            foreach (Transform otherTrans in neighbours)
            {
                newDir += otherTrans.up; //????
            }
            newDir /= neighbours.Count;

            newDir.z = 0; //cos 2d space

            neighbours.Clear(); //resetting the cached memory

            Debug.DrawLine(curPos, curPos + (newDir * debugLineLength), Color.magenta, Time.deltaTime);
        }

        return newDir;
    }
}
