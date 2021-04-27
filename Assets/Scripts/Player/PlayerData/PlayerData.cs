using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script placed on player prefab
//Holds values of the player, and performs functiosn related to those values
public class PlayerData : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public List<string> buffs;
    public bool isDead;

    private Vector3 playerPos;

    private void Awake()
    {
        playerPos = this.transform.position;
        isDead = false;
    }

    private void Update()
    {
        checkPlayerDeath();
    }

    void checkPlayerDeath()
    {
        if(isDead)
        {
            //Make player see death hud.
            //Make sure player object is not destroyed
            //Restore default values and respawn player.

        }
    }

    public void resetPlayerHP() {
        health = maxHealth;
        buffs.Clear();
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if(health < 0) {
            isDead = true;
        }
    }

}
