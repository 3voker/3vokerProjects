using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    private PlayerInteraction player;
    public List<EnemyHeartDrops> enemyHeartDrops = new List<EnemyHeartDrops>();
    public int forceHeartDropPerXEnemies = 3;

    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerInteraction>();
    }

    void Update()
    {
        //Fix later, spawns health from defeated enemies
        //if(player.targetsAttackingThis.Count > 0)
        //{
        //    foreach(Enemy enemy in player.targetsAttackingThis)
        //    {
        //        enemyHeartDrops.Add(enemy.gameObject.GetComponent<EnemyHeartDrops>());
        //    }
        //}
    }

    private void HeartBadLuckProtection()
    {
        if (enemyHeartDrops.Count > (forceHeartDropPerXEnemies - 1))
        {
            List<EnemyHeartDrops> forceHeartDrops = (List<EnemyHeartDrops>)enemyHeartDrops.Where((x, i) => (i + 1) % forceHeartDropPerXEnemies == 0);
            foreach (EnemyHeartDrops drop in forceHeartDrops)
            {
                drop.willAlwaysDropHearts = true;
            }
        }
    }
}
