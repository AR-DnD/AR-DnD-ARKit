using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	Animator anim;
	int walkHash = Animator.StringToHash ("OrcWarrior_Walk");

	Vector3 targetPosition;
	Vector3 lookAtTarget;
	Quaternion playerRot;
	float rotSpeed = 1.5f;
	float speed = 0.5f;
	bool moving = false;
	bool isSelected = false;
	GameObject child;

  //This block equips the unit with the appropriate BoxCollider, AnimationController,
  //or RigidBody component depending on how the model was previously configured.
	void Start () {
		if (gameObject.name.Equals("OrcWarrior_prefab(Clone)")){
			child = transform.Find ("Bip001").gameObject;
			BoxCollider box = child.AddComponent<BoxCollider> ();

		}
		if (gameObject.name.Equals("ToonRTS_demo_Knight(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("Warrior") as RuntimeAnimatorController;
			child = transform.Find ("WK_HeavyIntantry").gameObject;
			BoxCollider box = child.AddComponent<BoxCollider> ();

		}

		if (gameObject.name.Equals("Orc_Wolfrider(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("Wolfrider") as RuntimeAnimatorController;
			child = transform.Find ("Orc_SM_light_cavalry").gameObject;
			BoxCollider box = child.AddComponent<BoxCollider> ();

		}

		if (gameObject.name.Equals("UD_demo_character(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("Undead") as RuntimeAnimatorController;
			child = transform.Find ("UD_light_infantry").gameObject;
			BoxCollider box = child.AddComponent<BoxCollider> ();
		}

		if (gameObject.name.Equals("WK_mage_A(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("Mage") as RuntimeAnimatorController;
			BoxCollider box = gameObject.AddComponent<BoxCollider> ();
			box.size = new Vector3 (0.844456f, 1.524742f, 1.023993f);
			box.center = new Vector3 (-0.07777202f, 0.7448943f, -0.01199627f);
		}

		if (gameObject.name.Equals("WK_light_archer_A(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("Rogue") as RuntimeAnimatorController;
			BoxCollider box = gameObject.AddComponent<BoxCollider> ();
			box.size = new Vector3 (0.844456f, 1.524742f, 1.023993f);
			box.center = new Vector3 (-0.07777202f, 0.7448943f, -0.01199627f);
		}

		if (gameObject.name.Equals("WK_priest(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("Mage") as RuntimeAnimatorController;
			BoxCollider box = gameObject.AddComponent<BoxCollider> ();
			box.size = new Vector3 (0.844456f, 1.524742f, 1.023993f);
			box.center = new Vector3 (-0.07777202f, 0.7448943f, -0.01199627f);
		}

		if (gameObject.name.Equals("WK_heavy_infantry_A(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("EvilKnight") as RuntimeAnimatorController;
			BoxCollider box = gameObject.AddComponent<BoxCollider> ();
			box.size = new Vector3 (0.844456f, 1.524742f, 1.023993f);
			box.center = new Vector3 (-0.07777202f, 0.7448943f, -0.01199627f);
		}

		if (gameObject.name.Equals("FreeLich(Clone)")){
			gameObject.AddComponent<Rigidbody> ();
			gameObject.AddComponent<Animator> ();
			Animator animator = gameObject.GetComponent<Animator>();
			animator.runtimeAnimatorController = Resources.Load("Lich") as RuntimeAnimatorController;
			BoxCollider box = gameObject.AddComponent<BoxCollider> ();
			box.size = new Vector3 (1.616549f, 2.151645f, 1f);
			box.center = new Vector3 (-0.03587463f, 1.028881f, -5.488276e-12f);
		}

		anim = GetComponent<Animator> ();
	}

  //On each frame update, checks if the user has touched the screen. Selects the object if it is struck
  //by the user's finger. If the object is selected and the user hit somewhere else on the screen,
  //start moving the unit to that position
	void Update () {
		if (isSelected && Input.touchCount > 0) {
			Touch myTouch = Input.touches [0];
			if (myTouch.phase == TouchPhase.Began) {
				SetTargetPosition ();
			}
		}

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			Ray ray = Camera.main.ScreenPointToRay( Input.GetTouch(0).position );
			RaycastHit hit;

			if ( Physics.Raycast(ray, out hit) && hit.transform.gameObject == gameObject) {
				isSelected = true;
			}
		}

		if (moving) {
			MoveObject ();
		}
	}

  //Halt the unit if it's about to collide into another unit.

	void OnCollisionEnter(Collision collision)
	{
		moving = false;
		anim.SetBool ("isWalking", false);
	}

  //Grabs the position where the user touched, then activate the walking animation of the unit,
  //then starts moving the unit toward the position after getting it to rotate and look
  //towards the point. Set the "moving" boolean to true.
	void SetTargetPosition () {
		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 1000)) {
			anim.SetBool ("isWalking", true);
			targetPosition = hit.point;
			lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y, targetPosition.z - transform.position.z);
			playerRot = Quaternion.LookRotation (lookAtTarget);
			moving = true;
			isSelected = false;
		}

	}


  //Keeps moving the unit toward the destination point. "Slerp" rotates it according the speed, the
  //position moves it closer. Once it reaches approximately that point, set the walking animation to false
  //and moving to false.
	void MoveObject() {
		transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotSpeed * Time.deltaTime);
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);
		if (Mathf.Round(transform.position.x * 100f) / 100f == Mathf.Round(targetPosition.x * 100f) / 100f &&
			Mathf.Round(transform.position.z * 100f) / 100f == Mathf.Round(targetPosition.z * 100f) / 100f) {
			moving = false;
			anim.SetBool ("isWalking", false);
		}
	}
}
