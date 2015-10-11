using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public GameObject rayPrefab;
	
	private bool destroy = false;

	void Start () 
	{
		
	}

	void Update () 
	{
		if(destroy)
		{
			StartCoroutine("collectCoin");
		}
	}
	
	void OnTriggerEnter(Collider collisionInfo)
	{
		if(collisionInfo.gameObject.tag.Equals("Player"))
		{
			Player player = collisionInfo.gameObject.GetComponent<Player>();
			player.addCoins();
			destroy = true;
		}
	}
	
	IEnumerator collectCoin()
	{
		Vector3 pos = transform.position;
		pos.x = 0.0f;
		Instantiate(rayPrefab, pos, transform.rotation);
		yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
	}
}
