using UnityEngine;
using System.Collections;

//this class is a singleton (simple, non multithreaded)
//it does _not_ need to be put in a GameObject, it creates its own
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
		// Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? (instance = new GameObject("AmmoStorage").AddComponent<AmmoStorage>()); }
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
