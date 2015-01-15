using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	[SerializeField]
	private CheckpointManager _manager;

	[SerializeField]
	private int _index;

	void OnTriggerEnter(Collider other)
	{
		if (other as WheelCollider == null)
		{
			if( other.tag!="BonusAccelerator" && other.tag!="PickUp"  && other.tag!="Item" ) 
			_manager.CheckpointTriggered(other.transform.parent.parent.GetComponent<CarController>(),_index);
		}
	}
}
