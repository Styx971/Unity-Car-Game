using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour 
{

	[SerializeField]
	private GameObject _carContainer;

	[SerializeField]
	private int _checkPointCount;
	[SerializeField]
	private int _totalLaps;

	private bool _finished = false;
	
	private Dictionary<CarController,PositionData> _carPositions = new Dictionary<CarController, PositionData>();

	private class PositionData
	{
		public int lap;
		public int checkPoint;
		public int position;
	}

	// Use this for initialization
	void Awake () 
	{
		foreach (CarController car in _carContainer.GetComponentsInChildren<CarController>(true))
		{
			_carPositions[car] = new PositionData();
		}
	}
	public Transform findFirstPlace()
	{
		int maxLaps = -1;
		int maxCheckPoint = -1;
		foreach (CarController car in _carContainer.GetComponentsInChildren<CarController>(true)) 
		{
			PositionData carData = _carPositions[car];
			if (carData.lap > maxLaps)
			{
				maxLaps = carData.lap;
			}
		}
		foreach (CarController car in _carContainer.GetComponentsInChildren<CarController>(true)) 
		{
			PositionData carData = _carPositions[car];
			if (carData.lap == maxLaps)
			{
				if (carData.checkPoint > maxCheckPoint)
				{
					maxCheckPoint = carData.checkPoint;
				}
			}
		}
		List <Transform>carsPosition = new List<Transform>();
		foreach (CarController car in _carContainer.GetComponentsInChildren<CarController>(true)) 
		{
			PositionData carData = _carPositions[car];
			if (carData.lap == maxLaps)
			{
				if (carData.checkPoint == maxCheckPoint)
				{
					carsPosition.Add(car.transform);
				}
			}
		}
		// on a trouve la voiture a la premiere place
		if (carsPosition.Count == 1) 
		{
			print ("PREMIER :" + carsPosition[0].name);
			return carsPosition[0];
		}
		// liste des voitures qui sont au meme checkpoint et au meme lap, il faut affiner la recherche
		else
		{
			int nextCheckPoint = -1;
			if (maxCheckPoint == _checkPointCount-1)
			{
				nextCheckPoint = 0;
			}
			else
			{
				nextCheckPoint=maxCheckPoint+1;
			}
			Transform positionNextCheckPoint = this.transform;
			switch(nextCheckPoint)
			{
			case 0:
				positionNextCheckPoint = GameObject.Find("Start").transform;
				break;
			case 1:
				positionNextCheckPoint = GameObject.Find("Middle").transform;
				break;
			case 2:
				positionNextCheckPoint = GameObject.Find("End").transform;
				break;
			}
			float distance = Mathf.Infinity;
			Transform theFirstPlace = this.transform;
			foreach (Transform carPosition in carsPosition) 
			{
				// on recupère la norme du vecteur de difference
				float difference = (positionNextCheckPoint.position - carPosition.position).sqrMagnitude;
				if (difference < distance)
				{
					distance = difference;
					theFirstPlace = carPosition;
				}
				
			}
			print ("PREMIER :" + theFirstPlace.name);
			return theFirstPlace;
		}
	}
	public void CheckpointTriggered(CarController car, int checkPointIndex)
	{

		PositionData carData = _carPositions[car];

		if (!_finished)
		{
			if (checkPointIndex == 0)
			{
				if (carData.checkPoint == _checkPointCount-1)
				{
					carData.checkPoint = checkPointIndex;
					carData.lap += 1;
					Debug.Log(car.name + " lap " + carData.lap);
					if (IsPlayer(car))
					{
						GetComponent<RaceManager>().Announce("Tour " + (carData.lap+1).ToString());
					}

					if (carData.lap >= _totalLaps)
					{
						_finished = true;
						GetComponent<RaceManager>().EndRace(car.name.ToLower());
					}
				}
			}
			else if (carData.checkPoint == checkPointIndex-1) //Checkpoints must be hit in order
			{
				carData.checkPoint = checkPointIndex;
			}
		}


	}

	bool IsPlayer(CarController car)
	{
		return car.GetComponent<CarUserControlMP>() != null;
	}
}
