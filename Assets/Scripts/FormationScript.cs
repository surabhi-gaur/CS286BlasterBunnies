using UnityEngine;
using System.Collections;

public class FormationScript : MonoBehaviour {
	public GameObject[] formations; // array that holds the formations of a scene
	public int currentForm; // the index of the current formation
	public bool done = false; // have all formations finished
	// Use this for initialization
	void Start () {
		//formations = GameObject.FindGameObjectsWithTag("Formation"); // get all formations in a scene
		foreach(GameObject formation in formations) {
			formation.gameObject.SetActive(false); // set all formation to inactive
		}

		currentForm = 0; //set the current formation to the first one

		Invoke("startFormation", 3);//brief delay before starting the formations
	}
	
	// Update is called once per frame
	void Update () {
		if(formations[currentForm].transform.childCount <= 0) { // if current formation has completed
			if(currentForm == formations.Length - 1) { // if the current formation was the last formation
				done = true; // set the formations to done
			} else {
				currentForm++; // current formation is now the next formation
				startFormation(); // activate the current formation
			}
		}
	
	}
	// sets the current formation to active
	public void startFormation() {
		formations[currentForm].gameObject.SetActive(true);
	}
}
