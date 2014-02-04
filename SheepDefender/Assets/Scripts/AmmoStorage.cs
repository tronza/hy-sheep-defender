using UnityEngine;
using System.Collections;

/**
 * Copyright 2014 Agostino Sturaro
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
