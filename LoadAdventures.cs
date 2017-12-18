using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LoadAdventures : MonoBehaviour {
	public GameObject UI;
	public Text emailText;
	public Text passwordText;
	public GameObject passwordInputField;
	public GameObject AdventuresBig;
	public GameObject MapsBig;
	public GameObject AdventuresPanel;
	public GameObject MapsPanel;
	public GameObject PausePanel;
	public GameObject AdvLink;
	public GameObject MapLink;

	public void Start() {
    //This is triggered when coming back from an ARview.
    //If the adventure data already has been loaded, then we set the maps and
    //adventure views using that data.
    //We then attach all the components need to render the menue.
		if (MapUnitData.userAdventureData != null) {
			AdventuresBig.SetActive (true);
			MapsBig.SetActive (true);
			var adventuresJSON = JSON.Parse (MapUnitData.userAdventureData);
			JSONArray adventuresArray = adventuresJSON ["arr"].AsArray;
			for (int i = 0; i < adventuresArray.Count; i++) {
				var advLink = Instantiate(AdvLink);
				int tempInt = i;
				advLink.GetComponentInChildren<Text>().text = adventuresArray[i]["title"];
				advLink.gameObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
				advLink.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
				advLink.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(.5f, .5f);
				advLink.GetComponent<RectTransform>().SetParent(AdventuresPanel.transform);
				advLink.GetComponentInChildren<Text> ().font = Resources.Load<Font>("EnchantedLand");
				advLink.GetComponentInChildren<Text> ().fontSize = 50;
				advLink.GetComponent<Button>().onClick.AddListener(() => loadMaps(adventuresArray, tempInt));
				advLink.SetActive (true);
			}
			loadMaps (adventuresArray, MapUnitData.currAdvIndex);
		}
	}

  //If no data is present when the "Login" button is pressed, start the coroutine to load
  //user data
	public void loadData () {
		if (MapUnitData.userAdventureData == null) {
			Debug.Log ("Hi");
			StartCoroutine (LoadWWW ());
		}
	}

	IEnumerator LoadWWW () {

    //This makes a POST request to the mobile_adventures route on the Rails server,
    //using the email and password fields of the user.
		string url = "https://ar-dnd.herokuapp.com/mobile_adventures";
		WWWForm form = new WWWForm();
		form.AddField("email", emailText.text);
		form.AddField("password", passwordInputField.GetComponent<InputField>().text);
		WWW www = new WWW (url, form);
		yield return www;
    //if the return value is invalid, alert the user
		if (www.text.Equals("invalid")) {
			Debug.Log ("here in failure");
			PausePanel.SetActive (true);
		} else {
      //otherwise store the user data to a static variable, parse the JSON file to C# nested array,
      //and iterate through it to create UI buttons allowing the user to select which adventure they want
			AdventuresBig.SetActive (true);
			string adventuresString = www.text;
			storeUserData ("{\"arr\":" + adventuresString + "}");
			var adventuresJSON = JSON.Parse("{\"arr\":" + adventuresString + "}");
			JSONArray adventuresArray = adventuresJSON ["arr"].AsArray;

			for (int i = 0; i < adventuresArray.Count; i++) {
				Debug.Log(adventuresArray[i]["title"] + " " + i);
				var advLink = Instantiate(AdvLink);
				int tempInt = i;
				advLink.GetComponentInChildren<Text>().text = adventuresArray[i]["title"];
				advLink.gameObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
				advLink.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
				advLink.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(.5f, .5f);
				advLink.GetComponent<RectTransform>().SetParent(AdventuresPanel.transform);
				advLink.GetComponentInChildren<Text> ().font = Resources.Load<Font>("EnchantedLand");
				advLink.GetComponentInChildren<Text> ().fontSize = 50;
        //on clicking the link to an adventure, opens the map panel with list of maps
				advLink.GetComponent<Button>().onClick.AddListener(() => loadMaps(adventuresArray, tempInt));
				advLink.SetActive (true);
			}
		}
	}

	public void loadMaps(JSONArray adventuresArray, int num) {
    //destroy existing maps if on different map page
    //Then loop through the correct array of maps for the adventure specified, and create a new set of UI
    //buttons to select.
		var children = new List<GameObject>();
		foreach (Transform child in MapsPanel.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
		MapUnitData.currAdvIndex = num;
		JSONArray mapsArray = adventuresArray [num] ["maps"].AsArray;
		for (int i = 0; i < mapsArray.Count; i++) {
			var mapLink = Instantiate(MapLink);
			int tempInt = i;
			mapLink.GetComponentInChildren<Text>().text = mapsArray[i]["name"];
			mapLink.gameObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
			mapLink.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
			mapLink.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(.5f, .5f);
			mapLink.GetComponent<RectTransform>().SetParent(MapsPanel.transform);
			mapLink.GetComponentInChildren<Text> ().font = Resources.Load<Font>("EnchantedLand");
			mapLink.GetComponentInChildren<Text> ().fontSize = 50;
      //When the user selects a map, save that maps data to a static variable for use in the AR scene
			mapLink.GetComponent<Button>().onClick.AddListener(() => saveMapData(mapsArray[tempInt]["data"]));
			mapLink.SetActive (true);
		}

	}

	public void storeUserData(string userData) {
		MapUnitData.userAdventureData = userData;
	}

	public void saveMapData(string mapData) {
		AdventuresBig.SetActive (false);
		MapsBig.SetActive (false);
		MapUnitData.MapPositionData = mapData;
	}
}
