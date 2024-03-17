using UnityEngine;

public class ComboAnimCtrl : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private AnimationClip jumpAtk;
    private int _1stAtk;

    private void Awake()
    {
        _1stAtk = Animator.StringToHash("Attack1");
    }

}
