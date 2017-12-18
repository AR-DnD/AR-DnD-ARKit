using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapUnitData {
	private static string mapData;
	private static string userData;
	private static int currAdv;
	// Use this for initialization
  // This class is used to store the information received from login and
  // use it in the AR scene as a static variable
	public static string MapPositionData {
		get {
			return mapData;
		}
		set {
			mapData = value;
		}
	}

	public static string userAdventureData {
		get {
			return userData;
		}
		set {
			userData = value;
		}
	}

	public static int currAdvIndex {
		get {
			return currAdv;
		}
		set {
			currAdv = value;
		}
	}
}
