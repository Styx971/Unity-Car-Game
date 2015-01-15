using UnityEngine;
using System.Collections;

public class deahtZoneScript : MonoBehaviour {

	//IMPORTANT : This code needs refactoring and to be generic,
	//it's bad to depend of your parent transform.

	public Transform respawnPoint;

	void OnTriggerEnter(Collider playerCollider)
	{
		if (playerCollider.gameObject.transform.parent != null) 
		{
			if (playerCollider.gameObject.transform.parent.transform.parent != null) 
			{
				if (playerCollider.gameObject.transform.parent.transform.parent.tag == "Player") 
				{
					playerCollider.gameObject.transform.parent.transform.parent.transform.position  = respawnPoint.position;
					playerCollider.gameObject.transform.parent.transform.parent.transform.rotation = respawnPoint.rotation;
					playerCollider.gameObject.transform.parent.transform.parent.rigidbody.velocity = new Vector3(0,0,0);
				}
			}
		}
	}

}
