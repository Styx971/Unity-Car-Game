using UnityEngine;
using System.Collections;

public class blueShellScript : MonoBehaviour {

	public GameObject _blueShellProjectile;
	public float 	  _fireRate = 1.0f;
	private float 	  _nextShot=0.0f;
	private GameObject _player1;

	void Start () 
	{
		_player1 = GameObject.Find("Joueur 1");
	}
	
	// Update is called once per frame
	void Update () 
	{
		int countMunitionBleu = _player1.GetComponent<CountAmunition>().countCarapaceB;
		if (Input.GetKeyDown("space") && Time.time > _nextShot && countMunitionBleu>0) 
		{
			//increment time before next shot and create a clone of blueshellPrefab
			_nextShot = Time.time  + _fireRate;
			_player1.GetComponent<CountAmunition>().countCarapaceB = countMunitionBleu-1;
			_player1.GetComponent<CountAmunition>().CountCarapBlue.text = "Carapace bleu: " + _player1.GetComponent<CountAmunition>().countCarapaceB.ToString();
			GameObject clone = (GameObject)Instantiate(_blueShellProjectile,this.transform.position,this.transform.rotation);
		}
	}
}
