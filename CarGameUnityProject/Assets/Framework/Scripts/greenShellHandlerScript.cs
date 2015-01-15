using UnityEngine;
using System.Collections;


public class greenShellHandlerScript : MonoBehaviour {
	
	public int numberOfBounceMax=4;
	public float speed=10.0f;
	private int numberOfBounce=0;
	private float timeLimit = 10.0f;
	private float deltaTime =0.0f;

	void OnCollisionEnter (Collision hit)
	{


		if (hit.gameObject.tag != "Player" && hit.gameObject.tag=="Wall")
		{
			numberOfBounce = numberOfBounce + 1;
			if (numberOfBounce < numberOfBounceMax) 
			{
				rigidbody.AddForce(speed*this.transform.forward, ForceMode.Impulse);
			}
			else
			{
				Destroy(this.gameObject);
			}
		}
		else 
		{
			if (hit.gameObject.tag == "Player")
			{
				hit.transform.SendMessage("objectTouchedByGreenShell",SendMessageOptions.DontRequireReceiver);
				Destroy(this.gameObject,0.5f);
			}
		}
	}
	void Update()
	{
		deltaTime += Time.deltaTime;
		if (deltaTime > timeLimit) 
		{
			Destroy(this.gameObject);
		}
	}

}
