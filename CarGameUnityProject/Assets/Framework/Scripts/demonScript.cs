using UnityEngine;
using System.Collections;

public class demonScript : MonoBehaviour {

	private Animator animator;
	
	void Start()
	{
		animator = GetComponent<Animator>();
	}
		
	void OnTriggerEnter(Collider playerCollider)
	{
		if (playerCollider.gameObject.transform.parent != null) 
		{
			if (playerCollider.gameObject.transform.parent.transform.parent != null) 
			{
				if (playerCollider.gameObject.transform.parent.transform.parent.tag == "Player" && animator!=null) 
				{
					//Big Hit attack
					float i = Random.Range(1, 5);
					if (i ==4)
					{
						if (!audio.isPlaying)
						{
							this.audio.Play();
						}

						animator.SetInteger("State",1);
					}
						
					
				}
			}
		}
	}

	void Update()
	{
		//idle state
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("demon_SecondHit"))
		{
			animator.SetInteger("State",0);
		}
	}
}
