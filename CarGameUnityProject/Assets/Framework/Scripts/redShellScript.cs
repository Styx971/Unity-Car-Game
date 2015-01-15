using UnityEngine;
using System.Collections;

public class redShellScript : MonoBehaviour {


	public GameObject _redShellProjectile;
	public float 	  _fireRate = 1.0f;
	public float 	  _projectionSpeed = 60f;
	private float 	  _nextShot=0.0f;
	private GameObject _player1;
	void Start () 
	{
		_player1 = GameObject.Find("Joueur 1");
	}
	void Update () 
	{
		int countMunitionRed = _player1.GetComponent<CountAmunition>().countCarapaceR;
		if (Input.GetButton ("Fire2") && Time.time > _nextShot && countMunitionRed>0) 
		{
			// on incrémente le temps avant le prochain tir
			_nextShot = Time.time  + _fireRate;
			_player1.GetComponent<CountAmunition>().countCarapaceR = countMunitionRed-1;
			_player1.GetComponent<CountAmunition>().CountCarapRed.text = "Carapace rouge: " + _player1.GetComponent<CountAmunition>().countCarapaceR.ToString();
			// crée un clone à la position courante de l'objet à chaque fois qu'on appuie sur la touche fire1 (mouse0/left ctrl)
			GameObject clone = (GameObject)Instantiate(_redShellProjectile,this.transform.position,this.transform.rotation);
			clone.rigidbody.velocity = transform.TransformDirection(new Vector3(0,0,this.transform.parent.parent.rigidbody.velocity.magnitude + _projectionSpeed));
		}
	}
}
