using UnityEngine;
using System.Collections;

public class MinimapFollowNoRotate : MonoBehaviour {

	public GameObject target;
	public int heightToFixed;
	// Update is called once per frame
	void Update () {

		this.gameObject.transform.position = new Vector3 (target.transform.position.x,heightToFixed, target.transform.position.z);
	}
}
