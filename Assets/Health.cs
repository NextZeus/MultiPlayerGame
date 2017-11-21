﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;

	[SyncVar(hook="OnChangeHealth")]
	public int currentHealth = maxHealth;
	public RectTransform healthBar;

	public void TakeDamage(int damage){
		currentHealth -= damage;

		if(currentHealth <= 0){
			currentHealth = maxHealth;
			RpcRespawn();
		}
	}

	void OnChangeHealth(int health){
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn(){
		if(isLocalPlayer){
			transform.position = Vector3.zero;
		}
	}
}
