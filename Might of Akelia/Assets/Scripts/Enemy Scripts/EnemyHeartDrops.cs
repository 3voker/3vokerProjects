using UnityEngine;
using System.Collections;

public class EnemyHeartDrops : MonoBehaviour
{
    private Enemy enemy;
    public GameObject heartPrefab;
    public bool willAlwaysDropHearts = false;
    private bool droppedHeart;

    void Awake()
    {
        enemy = this.GetComponent<Enemy>();
    }

	void Update ()
    {
	    if(enemy.CurrentCombatState == CombatState.DieState && !droppedHeart)
        {
            if (willAlwaysDropHearts)
            {
                HeartDrop();
            }
            else
            {
                RandomHeartDrop();
            }
        }
	}

    public void RandomHeartDrop()
    {
        int rnd = Random.Range(0,1);
        switch (rnd)
        {
            case 0:
                HeartDrop();
                break;
            case 1:
                //will not drop hearts
                break;
        }
    }

    public void HeartDrop()
    {
        GameObject hP = Instantiate(heartPrefab);
        hP.transform.position = this.transform.position;
        droppedHeart = true;
    }
}
