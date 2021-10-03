using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitDetector : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.GetComponent<Enemy>().currentState != Enemy.State.Dead)
        {
            Debug.Log("Hit enemy");
            other.GetComponent<Enemy>().Damage(player.currentDamage, transform.position);
            if (player.holdingWeapon)
            {
                player.weaponDurability -= 1;
                if (player.weaponDurability <= 0)
                {
                    player.WeaponBreak();
                }
            }
        }
    }
}
