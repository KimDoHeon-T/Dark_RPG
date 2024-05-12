
using UnityEngine;

public class DemoScript : MonoBehaviour {

    public Transform target;
    public Animator ani;
    public float smoothSpeed = 15f;
    public Vector3 offset;

    private bool _aniReady = true;

    private void OnGUI()
    {
        if (_aniReady)
        {
            if (GUI.Button(new Rect(50, 50, 100, 30), "Attack"))
                SetTrigger("Attack1");

            if (GUI.Button(new Rect(160, 50, 100, 30), "Attack2"))
            {
                SetTrigger("Attack1");
                SetTrigger("Attack2");
            }

            if (GUI.Button(new Rect(270, 50, 100, 30), "Attack3"))
            {
                SetTrigger("Attack1");
                SetTrigger("Attack2");
                SetTrigger("Attack3");
            }


            if (GUI.Button(new Rect(380, 50, 100, 30), "Attack4"))
            {
                SetTrigger("Attack1");
                SetTrigger("Attack2");
                SetTrigger("Attack3");
                SetTrigger("Attack4");
            }

            if (GUI.Button(new Rect(50, 90, 100, 30), "Skill_A_1"))
                SetTrigger("Skill_A_1");

            if (GUI.Button(new Rect(160, 90, 100, 30), "Skill_A_2"))
            { 
               SetTrigger("Skill_A_1");
               SetTrigger("Skill_A_2");
            }

            if (GUI.Button(new Rect(50, 130, 100, 30), "Skill_B"))
                SetTrigger("Skill_B");

            if (GUI.Button(new Rect(50, 170, 100, 30), "Skill_C"))
                SetTrigger("Skill_C");

            if (GUI.Button(new Rect(50, 210, 100, 30), "Skill_D"))
                SetTrigger("Skill_D");

            if (GUI.Button(new Rect(50, 250, 100, 30), "Skill_E"))
                SetTrigger("Skill_E");

            if (GUI.Button(new Rect(50, 290, 100, 30), "Reaction"))
                SetTrigger("Reaction");

            if (GUI.Button(new Rect(50, 330, 100, 30), "Sturn"))
                SetTrigger("Sturn");

            if (GUI.Button(new Rect(50, 370, 100, 30), "KnockDown"))
                SetTrigger("KnockDown");

            if (GUI.Button(new Rect(50, 410, 100, 30), "Move"))
                SetTrigger("Move");

            if (GUI.Button(new Rect(50, 450, 100, 30), "Die"))
                SetTrigger("Die");

        }
        else
        {
            if (ani.GetNextAnimatorStateInfo(0).IsName("Base Layer.2HS_Idle"))
            {
                _aniReady = true;
            }
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    void SetTrigger(string param)
    {
        _aniReady = false;
        ani.SetTrigger(param); 
    }
}
