using UnityEngine;

public class AnimationBoolSetter : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string boolName;

    public void SetBool(bool value)
    {      
        animator.SetBool(boolName, value);
        Debug.Log("Bool set");
    }
}
