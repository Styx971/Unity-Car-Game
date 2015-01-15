using UnityEngine;
using System.Collections;

public class CountAmunition : MonoBehaviour {
	public GUIText CountCarapRed;
	public GUIText CountCarapBlue;
	public GUIText CountCarapGreen;

	public int countCarapaceR=0;
	public int countCarapaceG=0;
	public int countCarapaceB=0;

	public int pickupAmoRed = 4;
	public int pickupAmoGreen = 20;
	public int pickupAmoBlue = 1;
	private bool _triggered=false;
	private static System.Random random = new System.Random();
	// Use this for initialization
	void Start () {
		countCarapaceR=0;
		countCarapaceG=0;
		countCarapaceB=0;
		CountCarapRed.text = "Carapace rouge: " + countCarapaceR.ToString();
		CountCarapBlue.text = "Carapace bleue: " + countCarapaceB.ToString();
		CountCarapGreen.text = "Carapace verte: " + countCarapaceG.ToString();

	}



	// function when the car enter in collision with an object
	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "PickUp"){

			int valueBonus=CalculateRandomNumberPerPercentage();

			switch(valueBonus)
			{
				case 1:
					countCarapaceR+=pickupAmoRed;
				    CountCarapRed.text = "Carapace rouge: : " + countCarapaceR.ToString();
					break;
			    case 2:
					countCarapaceG+=pickupAmoGreen;
				    CountCarapGreen.text = "Carapace verte: " + countCarapaceG.ToString();
					break; 
				case 3: 
					countCarapaceB+=pickupAmoBlue;
				    CountCarapBlue.text = "Carapace bleue: " + countCarapaceB.ToString();
				break;
			}
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "PickUp") {
			if (!_triggered)
			{
				return;
			}
			_triggered = false;

		}
			
	}
	static int CalculateRandomNumberPerPercentage()
	{

		if (random.NextDouble() < 0.2)
			return 3;		
		return random.Next(1, 3);
	}
}
