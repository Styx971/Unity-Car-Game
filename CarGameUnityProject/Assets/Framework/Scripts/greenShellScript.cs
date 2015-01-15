using UnityEngine;
using System.Collections;

public class greenShellScript : MonoBehaviour {
	public GameObject _greenShellProjectile;
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
		int countMunitionGreen = _player1.GetComponent<CountAmunition>().countCarapaceG;
		if (Input.GetButton ("Fire1") && Time.time > _nextShot && countMunitionGreen>0) 
		{
			// on incrémente le temps avant le prochain tir
			_nextShot = Time.time  + _fireRate;
			_player1.GetComponent<CountAmunition>().countCarapaceG = countMunitionGreen-1;
			_player1.GetComponent<CountAmunition>().CountCarapGreen.text = "Carapace verte: " + _player1.GetComponent<CountAmunition>().countCarapaceG.ToString();
			// crée un clone à la position courante de l'objet à chaque fois qu'on appuie sur la touche fire1 (mouse0/left ctrl)
			GameObject clone = (GameObject)Instantiate(_greenShellProjectile,new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z),this.transform.rotation);
			//donne une vitesse initiale à l'objet venant d'etre créé
			clone.rigidbody.velocity = transform.TransformDirection(new Vector3(0,0,this.transform.parent.parent.rigidbody.velocity.magnitude + _projectionSpeed));
		}
	}
}
