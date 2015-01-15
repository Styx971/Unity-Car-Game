using UnityEngine;
using System.Collections;

public class demonHeadScript : MonoBehaviour {

	private GameObject demon;
	private Animator animator;
	void Start()
	{
		demon = GameObject.Find("Jarraxus");
		animator = demon.GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider playerCollider)
	{
		if (playerCollider.gameObject.transform.parent != null) 
		{
			if (playerCollider.gameObject.transform.parent.transform.parent != null) 
			{
				if (playerCollider.gameObject.transform.parent.transform.parent.tag == "Player") 
				{
					int i = Random.Range(1, 5);
					//laugh animation
					if (i == 4)
					{
						if (!audio.isPlaying)
						{
							this.audio.Play();
						}
						
						animator.SetInteger("State",2);
					}

				}
			}
		}
	}

	void Update()
	{
		//idle state
		if ( animator.GetCurrentAnimatorStateInfo(0).IsName("demon_Roar"))
		{
			animator.SetInteger("State",0);
		}
	}
}
