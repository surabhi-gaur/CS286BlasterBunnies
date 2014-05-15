using UnityEngine;
using System.Collections;

public class ShieldScript :  MonoBehaviour {
	PlayerScript player;
	private Animator animator;

	void Awake()
	{
		// Get the animator
		animator = GetComponent<Animator>();
	}
	
	// Use this for initialization
	void Start () {
		player = transform.parent.gameObject.GetComponent<PlayerScript>();
		gameObject.SetActive (false);
	}

	void Update () {
		int shieldStatus = player.shield;
		if (player.activeItem.type == GameManagerScript.Type.Defensive && shieldStatus > 0) {
			animator.SetTrigger ("ShieldUp"); //shield animation on		
		} 
		else {
			animator.SetInteger("ShieldDown",0);		
		}
	}
}

