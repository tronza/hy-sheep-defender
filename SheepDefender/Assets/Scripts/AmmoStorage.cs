using UnityEngine;
using System.Collections;

//this class is a singleton (simple, non multithreaded)
public class AmmoStorage : MonoBehaviour
{
	public enum AmmoType {RedLaser, GreenLaser};
	
	private static AmmoStorage instance;
	int[] ammoCounts;
	
	private AmmoStorage ()
	{
		//size must be the # values of the Type enum
		int ammoTypes = System.Enum.GetValues(typeof(AmmoType)).Length;
		ammoCounts = new int[ammoTypes];
		for(int i = 0; i < ammoCounts.Length; ++i) {
			ammoCounts[i] = 1000; //TODO: this is for testing, set to zero in final
		}
		//ammo counts will be set in the shop at the start of the game
	}

	public static AmmoStorage Instance {
		get {
			if (instance == null) {
				instance = new AmmoStorage ();
			}
			return instance;
		}
	}
	
	public int AvailabeAmmo(AmmoType type)
	{
		return instance.ammoCounts[(int)type];
	}
	
	public void AddAmmo(AmmoType type, int quantity)
	{
		instance.ammoCounts[(int)type] += quantity;
	}
	
	//TODO: throw exception or return error code if consuming more than available
	public void ConsumeAmmo(AmmoType type, int quantity)
	{
		instance.ammoCounts[(int)type] -= quantity;
	}
}
