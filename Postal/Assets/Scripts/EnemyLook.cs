using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && (enemy.currentState != Enemy.State.Dead && enemy.currentState != Enemy.State.RunAway && enemy.currentState != Enemy.State.Hit))
        {
            enemy.currentState = Enemy.State.RunTowards;
            enemy.target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && (enemy.currentState != Enemy.State.Dead && enemy.currentState != Enemy.State.RunAway))
        {
            enemy.currentState = Enemy.State.Idle;
            enemy.rb.velocity = Vector3.zero;
            enemy.target = null;
        }
    }
}

