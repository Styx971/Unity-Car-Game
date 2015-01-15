using UnityEngine;
using System.Collections;

public class golemScript : MonoBehaviour {

	private Animator animator;
	private GameObject backgroundMusic;
	
	void Start()
	{
		animator = GetComponent<Animator>();
		backgroundMusic = GameObject.Find ("backgroundMusic");
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
							StartCoroutine(PlaySound(backgroundMusic));
						}
						
						animator.SetInteger("State",1);
					}
					
					
				}
			}
		}
	}

	IEnumerator PlaySound(GameObject otherObject)
	{
		otherObject.audio.mute = true;
		this.audio.Play();
		while(audio.isPlaying){
			yield return 0;
		}
		otherObject.audio.mute = false;
	}
	
	void Update()
	{
		//idle state
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("hpunch"))
		{
			animator.SetInteger("State",0);
		}
	}
}
