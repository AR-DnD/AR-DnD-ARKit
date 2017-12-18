using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public GameObject orc;
		public GameObject warrior;
		public GameObject tree;
		public GameObject wolf;
		public GameObject skeleton;
		public GameObject rock;
		public GameObject fort;
		public GameObject chest;
		public Transform m_HitTransform;
		public GameObject generatePlanes;
		public GameObject pointCloud;
		public GameObject cleric;
		public GameObject mage;
		public GameObject rogue;
		public GameObject evilKnight;
		public GameObject evilMage;

		Boolean mapLoaded = false;
		Boolean alreadyHit = false;

		public void Start() {
//			generatePlanes.SetActive(true);
//			generatePlanes.GetComponent<UnityARGeneratePlane> ().enabled = true;
			mapLoaded = false;
			alreadyHit = false;
		}

        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
                    m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
					m_HitTransform.LookAt (Camera.main.transform.position);
					m_HitTransform.eulerAngles = new Vector3 (0, m_HitTransform.eulerAngles.y, 0);
                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    return true;
                }
            }
            return false;
        }

		// Update is called once per frame
		void Update () {
			if (Input.touchCount > 0 && m_HitTransform != null && !alreadyHit)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
//                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
                        // if you want to use infinite planes use this:
                        ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    }; 
					
                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType (point, resultType))
                        {
							if (!mapLoaded) {
								Debug.Log ("About to load map.");
								mapLoaded = true;
								alreadyHit = true;
								loadUnits ();
//								generatePlanes.SetActive(false);
//								generatePlanes.GetComponent<UnityARGeneratePlane> ().enabled = false;
//								pointCloud.SetActive (false);
							}
                            return;
                        }
                    }

				}
			}
		}

		public void loadUnits () {
			string urlJSON = MapUnitData.MapPositionData;
			Debug.Log (urlJSON);
			var mapJSON = JSON.Parse("{\"arr\":" + urlJSON + "}");
			JSONArray mapArray = mapJSON ["arr"].AsArray;
			spawn (mapArray);
		}

		public void spawn(JSONArray mapArray) {
			for (int i = 0, zCoord = -6; i < 6; i++, zCoord += 2) {
				var innerArray = mapArray [i].AsArray;
				for (int j = 0, xCoord = 6; j < 6; j++, xCoord -= 2) {
					if (innerArray [j] != null) {
						if (innerArray [j].Equals ("obj-Orc")) {
							var instantiatedOrc = GameObject.Instantiate (orc, Vector3.zero, Quaternion.identity) as GameObject;
							prepOrc (instantiatedOrc, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-Tree")) {
							var instantiatedTree = GameObject.Instantiate (tree, Vector3.zero, Quaternion.identity) as GameObject;
							prepTree (instantiatedTree, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-Wolf")) {
							var instantiatedWolf = GameObject.Instantiate (wolf, Vector3.zero, Quaternion.identity) as GameObject;
							prepWolf (instantiatedWolf, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-Skeleton")) {
							var instantiatedSkeleton = GameObject.Instantiate (skeleton, Vector3.zero, Quaternion.identity) as GameObject;
							prepSkeleton (instantiatedSkeleton, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-Fort")) {
							var instantiatedFort = GameObject.Instantiate (fort, Vector3.zero, Quaternion.identity) as GameObject;
							prepFort (instantiatedFort, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-Chest")) {
							var instantiatedChest = GameObject.Instantiate (chest, Vector3.zero, Quaternion.identity) as GameObject;
							prepChest (instantiatedChest, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-Rock")) {
							var instantiatedRock = GameObject.Instantiate (rock, Vector3.zero, Quaternion.identity) as GameObject;
							prepRock (instantiatedRock, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-EvilKnight")) {
							var instantiatedEvilKnight = GameObject.Instantiate (evilKnight, Vector3.zero, Quaternion.identity) as GameObject;
							prepEvilKnight (instantiatedEvilKnight, xCoord, zCoord);
						} else if (innerArray [j].Equals ("obj-EvilMage")) {
							var instantiatedEvilMage = GameObject.Instantiate (evilMage, Vector3.zero, Quaternion.identity) as GameObject;
							prepEvilMage (instantiatedEvilMage, xCoord, zCoord);
						} else if (innerArray [j].ToString ().Split ('-') [1].Equals ("Warrior")) {
							var instantiatedWarrior = GameObject.Instantiate (warrior, Vector3.zero, Quaternion.identity) as GameObject;
							prepWarrior (instantiatedWarrior, xCoord, zCoord);
						} else if (innerArray [j].ToString ().Split ('-') [1].Equals ("Mage")) {
							var instantiatedMage = GameObject.Instantiate (mage, Vector3.zero, Quaternion.identity) as GameObject;
							prepMage (instantiatedMage, xCoord, zCoord);
						} else if (innerArray [j].ToString ().Split ('-') [1].Equals ("Cleric")) {
							var instantiatedCleric = GameObject.Instantiate (cleric, Vector3.zero, Quaternion.identity) as GameObject;
							prepCleric (instantiatedCleric, xCoord, zCoord);
						} else if (innerArray [j].ToString ().Split ('-') [1].Equals ("Rogue")) {
							var instantiatedRogue = GameObject.Instantiate (rogue, Vector3.zero, Quaternion.identity) as GameObject;
							prepRogue (instantiatedRogue, xCoord, zCoord);
						}
					}
				}
			}
		}

		public void prepOrc(GameObject instantiatedOrc, int xCoord, int zCoord) {
			instantiatedOrc.AddComponent<Move> ();
			instantiatedOrc.transform.parent = transform;
			instantiatedOrc.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedOrc.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedOrc.transform.LookAt (Camera.main.transform.position);
			instantiatedOrc.transform.eulerAngles = new Vector3 (0, instantiatedOrc.transform.eulerAngles.y, 0);
		}

		public void prepTree(GameObject instantiatedTree, int xCoord, int zCoord) {
			instantiatedTree.transform.parent = transform;
			instantiatedTree.transform.localPosition = new Vector3 (xCoord, 0, zCoord);
			instantiatedTree.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
			instantiatedTree.transform.LookAt (Camera.main.transform.position);
			instantiatedTree.transform.eulerAngles = new Vector3 (0, instantiatedTree.transform.eulerAngles.y, 0);
		}

		public void prepWarrior(GameObject instantiatedWarrior, int xCoord, int zCoord) {
			instantiatedWarrior.AddComponent<Move> ();
			instantiatedWarrior.transform.parent = transform;
			instantiatedWarrior.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedWarrior.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedWarrior.transform.LookAt (Camera.main.transform.position);
			instantiatedWarrior.transform.eulerAngles = new Vector3 (0, instantiatedWarrior.transform.eulerAngles.y, 0);
		}

		public void prepMage(GameObject instantiatedMage, int xCoord, int zCoord) {
			instantiatedMage.AddComponent<Move> ();
			instantiatedMage.transform.parent = transform;
			instantiatedMage.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedMage.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedMage.transform.LookAt (Camera.main.transform.position);
			instantiatedMage.transform.eulerAngles = new Vector3 (0, instantiatedMage.transform.eulerAngles.y, 0);
		}

		public void prepCleric(GameObject instantiatedCleric, int xCoord, int zCoord) {
			instantiatedCleric.AddComponent<Move> ();
			instantiatedCleric.transform.parent = transform;
			instantiatedCleric.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedCleric.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedCleric.transform.LookAt (Camera.main.transform.position);
			instantiatedCleric.transform.eulerAngles = new Vector3 (0, instantiatedCleric.transform.eulerAngles.y, 0);
		}

		public void prepRogue(GameObject instantiatedRogue, int xCoord, int zCoord) {
			instantiatedRogue.AddComponent<Move> ();
			instantiatedRogue.transform.parent = transform;
			instantiatedRogue.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedRogue.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedRogue.transform.LookAt (Camera.main.transform.position);
			instantiatedRogue.transform.eulerAngles = new Vector3 (0, instantiatedRogue.transform.eulerAngles.y, 0);
		}

		public void prepWolf(GameObject instantiatedWolf, int xCoord, int zCoord) {
			instantiatedWolf.AddComponent<Move> ();
			instantiatedWolf.transform.parent = transform;
			instantiatedWolf.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedWolf.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedWolf.transform.LookAt (Camera.main.transform.position);
			instantiatedWolf.transform.eulerAngles = new Vector3 (0, instantiatedWolf.transform.eulerAngles.y, 0);
		}

		public void prepSkeleton(GameObject instantiatedSkeleton, int xCoord, int zCoord) {
			instantiatedSkeleton.AddComponent<Move> ();
			instantiatedSkeleton.transform.parent = transform;
			instantiatedSkeleton.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedSkeleton.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedSkeleton.transform.LookAt (Camera.main.transform.position);
			instantiatedSkeleton.transform.eulerAngles = new Vector3 (0, instantiatedSkeleton.transform.eulerAngles.y, 0);
		}

		public void prepEvilKnight(GameObject instantiatedEvilKnight, int xCoord, int zCoord) {
			instantiatedEvilKnight.AddComponent<Move> ();
			instantiatedEvilKnight.transform.parent = transform;
			instantiatedEvilKnight.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedEvilKnight.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedEvilKnight.transform.LookAt (Camera.main.transform.position);
			instantiatedEvilKnight.transform.eulerAngles = new Vector3 (0, instantiatedEvilKnight.transform.eulerAngles.y, 0);
		}

		public void prepEvilMage(GameObject instantiatedEvilMage, int xCoord, int zCoord) {
			instantiatedEvilMage.AddComponent<Move> ();
			instantiatedEvilMage.transform.parent = transform;
			instantiatedEvilMage.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedEvilMage.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedEvilMage.transform.LookAt (Camera.main.transform.position);
			instantiatedEvilMage.transform.eulerAngles = new Vector3 (0, instantiatedEvilMage.transform.eulerAngles.y, 0);
		}

		public void prepFort(GameObject instantiatedFort, int xCoord, int zCoord) {
			instantiatedFort.transform.parent = transform;
			instantiatedFort.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedFort.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
			instantiatedFort.transform.LookAt (Camera.main.transform.position);
			instantiatedFort.transform.eulerAngles = new Vector3 (0, instantiatedFort.transform.eulerAngles.y, 0);
		}

		public void prepChest(GameObject instantiatedChest, int xCoord, int zCoord) {
			instantiatedChest.transform.parent = transform;
			instantiatedChest.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedChest.transform.localScale = new Vector3 (1f, 1f, 1f);
			instantiatedChest.transform.LookAt (Camera.main.transform.position);
			instantiatedChest.transform.eulerAngles = new Vector3 (0, instantiatedChest.transform.eulerAngles.y, 0);
		}

		public void prepRock(GameObject instantiatedRock, int xCoord, int zCoord) {
			instantiatedRock.transform.parent = transform;
			instantiatedRock.transform.localPosition = new Vector3 (xCoord, 1, zCoord);
			instantiatedRock.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
			instantiatedRock.transform.LookAt (Camera.main.transform.position);
			instantiatedRock.transform.eulerAngles = new Vector3 (0, instantiatedRock.transform.eulerAngles.y, 0);
		}
	}
}

