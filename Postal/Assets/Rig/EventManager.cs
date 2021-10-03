using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Player player;

    public void HitEnemy()
    {
        player.StartCoroutine("Swing");
    }

    public void Throw()
    {
        player.Throw();
    }
}
