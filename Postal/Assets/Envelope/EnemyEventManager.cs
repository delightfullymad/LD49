using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventManager : MonoBehaviour
{
    public Enemy enemy;

    public void HitPlayer()
    {
        enemy.StartCoroutine("AttackHit");
    }

    public void Idle()
    {
        enemy.currentState = Enemy.State.Idle;
    }
}
