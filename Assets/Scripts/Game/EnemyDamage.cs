using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.kbFromRight = (collision.transform.position.x <= transform.position.x)
                ? true : false;

            StartCoroutine(player.DelayLose());
        }
    }
}
