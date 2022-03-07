using Invector.vCharacterController;
using UnityEngine;
using Keys = GameData.Keys;

public class PlayerAnimatorHandler : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayMeleeAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayGetHit()
    {
        animator.Play("GetHit");
        animator.SetBool("OnGround", true);
    }

    void StopAttack()
    {
        if (!Input.GetMouseButtonDown(Keys.ATTACK))
        {
            animator.SetBool("Attack", false);
        }
    }

    public void PlayPickUp()
    {
        animator.SetTrigger("PickUp");
    }

    private void StopInputWhenPickingUpItem()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
        {
            gameObject.GetComponent<vThirdPersonInput>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<vThirdPersonInput>().enabled = true;
        }
    }

    void Update()
    {
        StopAttack();
    }
}
