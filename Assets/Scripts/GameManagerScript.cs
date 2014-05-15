﻿using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	public PlayerScript playerPrefab; // prefab to create the player
	private PlayerScript player; // the player object
	private float prevLife = 4;
	private Vector3 prevPos = new Vector3(0,0,0);
	
	public FormationScript formationManager; // the formation manager
	
	public ItemScript[] itemPrefabs = new ItemScript[3];
	
	private int blueLoyalty; // loyalty score
	private int greenLoyalty;
	private int redLoyalty;
	public int MIN_LOYALTY; // minimum loyalty for friendly to join player
	
	private float lives; // player current lives 
	private int score;
	public bool playing; // is the game playing or paused
	public bool win; // if the player wins or not
	
	public enum GameState { // the states of the game
		Menu,
		Story,
		Pause,
		Phase1,
		Phase2,
		Friends,
		Phase3,
		BossMain,
		GameOver
	}
	public GameState currentState; // the current state of the game
	public GameState prevState; // previous state
	
	public enum Type {  // the types an enemy or item can be
		Aggressive = 0,
		Defensive = 1,
		Balanced = 2
	};
	
	public float sidebarWidth;
	
	void Awake(){
		DontDestroyOnLoad(transform.gameObject); // keeps the gamemanager from being destroyed when a scene changes
	}
	// Use this for initialization
	void Start () {
		win = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		switch(currentState) {
		case GameState.Menu: //main menu
			playing = false;
			if(Input.GetKeyDown("1")) {
				prevState = currentState;
				currentState = GameState.Story;
				Application.LoadLevel(5);
			}
			break;
		case GameState.Pause: // paused
			playing = false;
			break;
		case GameState.Phase1: // phase1
			playing = true;
			if(formationManager != null){
				if(formationManager.done) { // check to see if all formations have finished
					prevLife = player.life;
					prevPos = player.transform.position;
					prevState = currentState;
					currentState = GameState.Phase2; // change state to next level
					Application.LoadLevel(2); // load the next scene
				}
			}
			break;
		case GameState.Phase2: // phase2
			playing = true;
			if(formationManager != null){
				if(formationManager.done) { // check to see if all formations have finished
					prevLife = player.life;
					prevPos = player.transform.position;
					prevState = currentState;
					currentState = GameState.Friends; // change state to next level
					Application.LoadLevel(6); // load the next scene
				}
			}
			break;
		case GameState.Friends:
			playing = false;
			if(Input.GetKeyDown("1")) {
				prevState = currentState;
				currentState = GameState.Phase3; // change state to next level
				redLoyalty = 0;
				blueLoyalty = 0;
				greenLoyalty = 0;
				Application.LoadLevel(3);
			}
			break;
		case GameState.Phase3: // phase3
			playing = true;
			if(formationManager != null){
				if(formationManager.done) { // check to see if all formations have finished
					prevLife = player.life;
					prevPos = player.transform.position;
					prevState = currentState;
					currentState = GameState.BossMain; // end of the game
					Application.LoadLevel(4); // load the next scene
				}
			}
			// if any loyalty is above the minimum activate the ally
			if(redLoyalty >= MIN_LOYALTY) {
				if(player.allies[0] != null) {
					player.allies[0].gameObject.SetActive(true);
				}
			} else {
				if(player.allies[0] != null) {
					player.allies[0].gameObject.SetActive(false);
				}
			}
			if(greenLoyalty >= MIN_LOYALTY) {
				if(player.allies[1] != null) {
					player.allies[1].gameObject.SetActive(true);
				}
			} else {
				if(player.allies[1] != null) {
					player.allies[1].gameObject.SetActive(false);
				}
			}
			if(blueLoyalty >= MIN_LOYALTY) {
				if(player.allies[2] != null) {
					player.allies[2].gameObject.SetActive(true);
				}
			} else {
				if(player.allies[2] != null) {
					player.allies[2].gameObject.SetActive(false);
				}
			}
			break;
		case GameState.BossMain: //Boss phase
			playing = true;
			if(formationManager.done) {
				if(player.life > 0) {
					win = true;
				}
				prevState = currentState;
				currentState = GameState.GameOver; //end of game
			}
			// if any loyalty is above the minimum activate the ally
			if(redLoyalty >= MIN_LOYALTY) {
				if(player.allies[0] != null) {
					player.allies[0].gameObject.SetActive(true);
				}
			} else {
				if(player.allies[0] != null) {
					player.allies[0].gameObject.SetActive(false);
				}
			}
			if(greenLoyalty >= MIN_LOYALTY) {
				if(player.allies[1] != null) {
					player.allies[1].gameObject.SetActive(true);
				}
			} else {
				if(player.allies[1] != null) {
					player.allies[1].gameObject.SetActive(false);
				}
			}
			if(blueLoyalty >= MIN_LOYALTY) {
				if(player.allies[2] != null) {
					player.allies[2].gameObject.SetActive(true);
				}
			} else {
				if(player.allies[2] != null) {
					player.allies[2].gameObject.SetActive(false);
				}
			}
			
			break;
		case GameState.GameOver: // gameover
			playing = false;
			if(Input.GetKeyDown("1")) {
				prevState = currentState;
				currentState = GameState.Menu;
				Application.LoadLevel(0);
				Destroy(gameObject);
			}
			if(Input.GetButton("Fire1")) {
				if (prevState == GameState.Phase1) {
					currentState = prevState;
					prevState = GameState.GameOver;
					score = 0;
					prevLife = 4;
					prevPos = new Vector3(0,0,0);
					Application.LoadLevel(1);
				} else if(prevState == GameState.Phase2) {
					currentState = prevState;
					prevState = GameState.GameOver;
					score = 0;
					prevLife = 4;
					prevPos = new Vector3(0,0,0);
					Application.LoadLevel(2);
				} else if (prevState == GameState.Phase3) {
					currentState = prevState;
					prevState = GameState.GameOver;
					redLoyalty = 0;
					blueLoyalty = 0;
					greenLoyalty = 0;
					score = 0;
					prevLife = 4;
					prevPos = new Vector3(0,0,0);
					Application.LoadLevel(3);
				} else if (prevState == GameState.BossMain) {
					currentState = prevState;
					prevState = GameState.GameOver;
					redLoyalty = 0;
					blueLoyalty = 0;
					greenLoyalty = 0;
					score = 0;
					prevLife = 4;
					prevPos = new Vector3(0,0,0);
					Application.LoadLevel(4);
				} 
			}
			break;
		case GameState.Story:
			playing = false;
			if(Input.GetKeyDown("1")) {
				prevState = currentState;
				currentState = GameState.Phase1;
				redLoyalty = 0;
				blueLoyalty = 0;
				greenLoyalty = 0;
				prevLife = 4;
				prevPos = new Vector3(0,0,0);
				Application.LoadLevel(1);
			}
			break;
		}
		
		if(playing && player != null){
			
			if(Input.GetKeyDown("escape")) {
				prevState = currentState;
				currentState = GameState.Pause;
			}
			if(player.life <= 0) { // if player has no more lives then gameover
				prevState = currentState;
				currentState = GameState.GameOver;
			}
			
			lives = player.life; // update lives with players current life
		}
		
		
		
	}
	
	void OnGUI() {
		
		float width = 140f;
		float xpos = Screen.width / 2f - width / 2f;
		float ypos = Screen.height / 2 - 100f;
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(-3.5f,0,0));
		Vector2 GUIPoint = GUIUtility.ScreenToGUIPoint(new Vector2(screenPoint.x,screenPoint.y));
		sidebarWidth = GUIPoint.x;
		
		switch(currentState) {
		case GameState.Menu:
			float controlsX = Screen.width/4 * 3 - 10;
			float controlsY = Screen.height/4 * 3 - 30;
			GUI.Box(new Rect(controlsX, controlsY, width, 120f), "Main Menu");
			GUI.Label(new Rect(controlsX + 10,controlsY + 15, width, 20), "Controls");
			GUI.Label(new Rect(controlsX + 10,controlsY + 30, width, 20), "Shoot: Button 1");
			GUI.Label(new Rect(controlsX + 10,controlsY + 45, width, 20), "Use Item: Button 2");
			GUI.Label(new Rect(controlsX + 10,controlsY + 60, width, 20), "Gift Item: Start Button");
			GUI.Label(new Rect(controlsX + 10,controlsY + 75, width, 20), "Movement: JoyStick");
			GUI.Label(new Rect(controlsX + 10,controlsY + 90, width, 20), "Press Button 1 to play");
			
			break;
		case GameState.Pause:
			GUI.Box(new Rect(xpos, ypos, width, 160f), "Pause");
			
			
			if (GUI.Button(new Rect(xpos + 20, ypos + 30, width - 40, 38f), "Menu"))
			{
				prevState = currentState;
				currentState = GameState.Menu;
				Application.LoadLevel(0);
				Destroy(gameObject);
			}
			if (GUI.Button(new Rect(xpos + 20, ypos + 73, width - 40, 38f), "Resume"))
			{
				currentState = prevState;
				prevState = GameState.Pause;
				
			}
			if (GUI.Button(new Rect(xpos + 20, ypos + 116, width - 40, 38f), "Exit"))
			{
				Application.Quit();
			}
			break;
		case GameState.Phase1:
		case GameState.Phase2:
		case GameState.Phase3:
		case GameState.BossMain:
			GUI.Box(new Rect(0, 0, sidebarWidth, Screen.height), "Score");
			GUI.Box(new Rect(Screen.width - sidebarWidth, 0, sidebarWidth, Screen.height), "Items");
			GUI.Label(new Rect(sidebarWidth/3,10, sidebarWidth - 20, 20), "Lives: " + lives);
			
			if(player != null && player.shield > 0) {
				GUI.Label(new Rect(sidebarWidth/3,20, width - 20, 20), "Shield: " + player.shield);
			}
			GUI.Label(new Rect(sidebarWidth/3,Screen.height - 20, width - 20, 20), "Points: " + score);
			
			if(currentState == GameState.Phase3 || currentState == GameState.BossMain) {
				GUI.Label(new Rect(sidebarWidth/3,40, width - 20, 20), "Blue Loyalty: " + blueLoyalty);
				GUI.Label(new Rect(sidebarWidth/3,50, width - 20, 20), "Green Loyalty: " + greenLoyalty);
				GUI.Label(new Rect(sidebarWidth/3,60, width - 20, 20), "Red Loyalty: " + redLoyalty);
			}
			
			break;
			
		case GameState.GameOver:
			if(win) {
				GUI.Box(new Rect(xpos, ypos, width, 160f), "You Won");
				GUI.Label(new Rect(xpos + 20, ypos + 30, width - 20, 20f), "Points: " + score);
				GUI.Label(new Rect(xpos + 20,ypos + 45, width, 20), "Menu: Press Start");
				
			} else {
				GUI.Box(new Rect(xpos, ypos, width, 80f), "Game Over");
				GUI.Label(new Rect(xpos + 10,ypos + 25, width, 20), "Menu: Press Start");
				GUI.Label(new Rect(xpos + 10,ypos + 45, width, 20), "Retry: Press Button 1");
				
			}
			break;
		}
	}
	
	void OnLevelWasLoaded(int level) {
		
		if( level > 0 && level < 5) {
			formationManager = GameObject.FindWithTag("FormationManager").GetComponent<FormationScript>(); // get the current scenes formationManager
			if(player == null) { // if no player then create one
				player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as PlayerScript;
				player.gameManager = gameObject.GetComponent<GameManagerScript>();
				player.life = prevLife;
				player.transform.position = prevPos;
			}
		}
	}
	
	public void dropItem(Type t, Transform trans) {
		if(currentState == GameState.Phase2 || currentState == GameState.Phase3) {
			Instantiate(itemPrefabs[(int)t], trans.position, Quaternion.identity);
		}
		
	}
	
	public void increaseScore(int i) {
		score += i;
	}
	
	public void changeLoyalty(Type t, int amount) {
		switch(t) {
		case Type.Aggressive:
			redLoyalty += amount;
			break;
		case Type.Defensive:
			blueLoyalty += amount;
			break;
		case Type.Balanced:
			greenLoyalty += amount;
			break;
		}
	}
}