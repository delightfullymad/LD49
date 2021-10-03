using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Player player;
    public Slider healthSlider;
    public Text weapon;
    public Text wave;
    public Text wasted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = player.health;
        wave.text = "Wave: "+GameManager.GM.waveNum.ToString();
        if (player.holdingWeapon || player.holdingThrowable)
        {
            weapon.text = player.weapon.GetComponent<Weapon>().weaponName;
        }
        else if(weapon.text != "Fist")
        {
            weapon.text = "Fist";
        }
    }
}
