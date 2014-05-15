using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
	public float spawnRate = 5f; //Time between each row of enemies spawnned
	public float spawnTime = 3f; //Time to wait before you start spawnning
	public EnemyScript[] enemies; //enemy prefab array
	public float maxX = 6.5f;
	public float minX = -6.5f;
	public int amount;
	public bool straight;
	// Use this for initialization
	void Start () {
	}
	void SpawnEnemy (){
		int enemyIndex = Random.Range(0, enemies.Length);
		Vector3 pos = new Vector3(Random.Range(maxX,minX), transform.position.y, 0);
		EnemyScript newEnemy = Instantiate(enemies[enemyIndex], pos, Quaternion.identity) as EnemyScript;
		newEnemy.moveStraight = straight;
		newEnemy.transform.parent = transform.parent;
		amount--;
		}
	// Update is called once per frame
	void Update () {
		if(spawnTime > 0) {
			spawnTime -= Time.deltaTime;
		}
		if(spawnTime <= 0) {
			SpawnEnemy();
			spawnTime = spawnRate;
		}
		if( amount <= 0) {
			Destroy(gameObject);
		}
	}
}
