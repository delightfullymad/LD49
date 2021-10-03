using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Player player;
    public string weaponName = "Supplies";
    public Material baseMat;
    public Material selectedMat;
    public bool pickedUp;
    public float damage;
    public int durability;
    public Transform model;
    bool hover;
    public bool throwable;
    public GameObject explodeParticle;
    public bool thrown;

    private void Start()
    {
        player = Player.player;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Mouse")
        {
            model.GetComponent<Renderer>().material = selectedMat;
            hover = true;
        }
    }

    private void Update()
    {
        if (hover && Input.GetMouseButtonDown(0) && !pickedUp && !player.holdingWeapon && !player.holdingThrowable)
        {
            print("pickup");
            Pickup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mouse")
        {
            model.GetComponent<Renderer>().material = baseMat;
            hover = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > 1f && thrown && collision.transform.tag == "Enemy" && collision.transform.GetComponent<Enemy>().currentState != Enemy.State.Dead)
        {
            collision.transform.GetComponent<Enemy>().Damage(damage, transform.position);
            collision.transform.GetComponent<Enemy>().rb.velocity = Vector3.zero;
            Instantiate(explodeParticle, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Pickup()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        print(dist);
        if (dist < 4f)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = 12;
            pickedUp = true;
            thrown = false;
            transform.parent = player.handSlot.transform;
            transform.position = player.handSlot.position;
            transform.rotation = player.handSlot.rotation;
            player.weapon = gameObject;
            if (!throwable)
            {
                player.Equip(damage, durability);
            }
            else
            {
                player.Holding();
            }
        }
    }

}
