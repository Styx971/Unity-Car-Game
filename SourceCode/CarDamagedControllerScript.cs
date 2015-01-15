using UnityEngine;
using System.Collections;

public class CarDamagedControllerScript : MonoBehaviour {


	void objectTouchedByGreenShell()
	{
		animation.Play("hit_by_green_shell_anim");
	}
	void objectTouchedRedShell()
	{
		animation.Play("hit_by_red_shell_anim");
	}
	void objectTouchedBlueShell()
	{
		animation.Play("hit_by_blue_shell_anim");
	}
}
