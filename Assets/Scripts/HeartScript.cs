using UnityEngine;
using System.Collections;

public class HeartScript : MonoBehaviour {
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
	
	// Update is called once per frame
	void Update () {
		if (player.activeItem.type == GameManagerScript.Type.Balanced) {
			animator.SetTrigger("Heart"); //heart animation on		
		}
	}
}
