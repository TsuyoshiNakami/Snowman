using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData {



	public static bool Save() {
		try {
			zFoxDataPackString playerData = new zFoxDataPackString ();
		//	PlayerController pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
			playerData.Add ("CheckPointNumber", PlayerController.CheckPointNumber);

			PlayerPrefs.Save ();
			return true;
		} catch (System.Exception e) {
		}
		return false;
	}

	public static bool Load() {
		try {
		zFoxDataPackString playerData = new zFoxDataPackString ();
		playerData.DecodeDataPackString (playerData.PlayerPrefsGetStringUTF8 ("playerData"));
		PlayerController.CheckPointNumber =
			(int)playerData.GetData ("CheckPointNumber");
			return true;

		} catch(System.Exception e) {
		}
		return false;
	}


}
