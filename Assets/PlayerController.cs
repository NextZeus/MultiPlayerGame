using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	void Update () {
		if(!isLocalPlayer){
			return;
		}

		var x = Input.GetAxis("Horizontal")	* Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		if(Input.GetKeyDown(KeyCode.Space)){
			CmdFire();
		}

	}
	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	// 注意[Command]可以声明一个函数可以本客户端调用，但是会在服务端（主机）执行
	[Command]
	void CmdFire()
	{
		// create the bullet from the bullet prefab
		var bullet = (GameObject) Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation
		);

		// add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
		
		// spawn the bullet on the clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
