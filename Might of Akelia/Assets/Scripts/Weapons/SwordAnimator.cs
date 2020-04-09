using UnityEngine;
using System.Collections;

public class SwordAnimator : MonoBehaviour
{
    private Animator swordAnimator;

    void Awake()
    {
        swordAnimator = this.GetComponent<Animator>();
    }

    //Key - Nothing (0), Attack (1), Block (2), Unblock (3)

    public void PlaySwordAttack()
    {
        swordAnimator.SetInteger("State", 1);
    }

    public void PlaySwordBlock()
    {
        swordAnimator.SetInteger("State", 2);
    }

    public void PlaySwordUnBlock()
    {
        swordAnimator.SetInteger("State", 3);
    }

    public void PlayNothing()
    {
        swordAnimator.SetInteger("State", 0);
    }
}
