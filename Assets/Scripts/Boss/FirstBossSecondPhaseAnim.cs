using UnityEngine;

public class FirstBossSecondPhaseAnim : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }


}
