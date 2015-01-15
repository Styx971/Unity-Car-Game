using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
     
	public int RespawnTime = 25;
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}

	void  OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Wheel"){
			this.gameObject.SetActive(false);
			Invoke("Enabled",RespawnTime);
		}
	}
	void Enabled(){

		this.gameObject.SetActive(true);
	}
}
