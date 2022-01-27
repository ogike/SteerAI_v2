using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/* Script to put onto every Room GameObject
 * Also every instance of these should be added to the "rooms" list in the RoomManager
 * Handles activating/deactivating stuff in the room on entering
 */

public class RoomHandler : MonoBehaviour
{
    public GameObject enemiesToWakeOnEnter; //all the enemies to activate on room-enter

    [HideInInspector] public int numOfenemiesInThisRoom;
    public List<Transform> curEnemiesInThisRoom; //debug public


    public void Awake()
	{
        Transform enemiesParent = enemiesToWakeOnEnter.transform;

        numOfenemiesInThisRoom = enemiesParent.childCount;

		//TODO: probably very performance heavy, should be baked in during editor-time with a burron or sth
		for (int i = 0; i < numOfenemiesInThisRoom; i++)
		{
            curEnemiesInThisRoom.Add(enemiesParent.GetChild(i));
        }
    }

    public void EnemyDied(Transform deceased)
	{
        curEnemiesInThisRoom.Remove(deceased);
	}

    public void EnemySpawned(Transform spawned)
	{
        curEnemiesInThisRoom.Add(spawned);
	}

    public List<Transform> GetNeighbours (Transform orig, float maxDist)
	{
        Vector3 origPos = orig.position;

        List<Transform> neighbours = new List<Transform>();

        //mayhaps better: using Physics2D.OverlapCircleAll() ??
		foreach (Transform enemy in curEnemiesInThisRoom)
		{
            //TODO: make active check more optimized..?
            if (enemy != orig && enemy.gameObject.activeInHierarchy && Vector2.Distance(origPos, enemy.position) < maxDist)
			{
                neighbours.Add(enemy);
			}
		}

        return neighbours;
	}
}
