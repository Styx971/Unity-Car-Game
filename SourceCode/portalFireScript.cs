using UnityEngine;
using System.Collections;

public class portalFireScript : MonoBehaviour {

	//IMPORTANT : This code needs refactoring and to be generic,
	//it's bad to depend of your parent transform.

	void OnTriggerEnter(Collider playerCollider)
	{
		if (playerCollider.gameObject.transform.parent != null) 
		{
			if (playerCollider.gameObject.transform.parent.transform.parent != null) 
			{
				if (playerCollider.gameObject.transform.parent.transform.parent.tag == "Player") 
				{
					Transform teleportPosition = GameObject.Find("spawnPointFire").transform;
					playerCollider.gameObject.transform.parent.transform.parent.transform.position  = teleportPosition.position;
				}
			}

		}

	}
}
