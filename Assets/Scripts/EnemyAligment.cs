using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHandler))]
public class EnemyAligment : MonoBehaviour
{
    public float debugLineLength = 1;

    //public float maxForce;
    public float distance;

    public float steerBehaviourWeight = 1;

    List<Transform> neighbours;

    Vector3 cohesionDir;

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
        cohesionDir = CalcCohesionForce();

        myHandler.AddSteerDir(cohesionDir, steerBehaviourWeight);
        //Move(separationDir);
    }

    Vector3 CalcCohesionForce()
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

            newDir -= curPos; //create offset from our position
            newDir.z = 0; //cos 2d space

            neighbours.Clear(); //resetting the cached memory

            Debug.DrawLine(curPos, curPos + (newDir * debugLineLength), Color.blue, Time.deltaTime);
        }

        return newDir;
    }

    //should be modularized somewhere else
    //void Move(Vector3 dir)
    //{
    //    myRigidbody.AddForce(dir * maxForce * Time.deltaTime);
    //}
}
