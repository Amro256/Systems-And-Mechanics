using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; } //Singleton Pattern

    //General Variables
    [SerializeField] private Animator[] animators; //Array to hold the animator components attached to different game objects


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }



    //Methods to start and stop the animation
    public void StartAnimation(string aniParam)
    {
        foreach (Animator am in animators)
        {
            am.SetBool(aniParam, true);
        }
    }

    public void StopAnimation(string aniParam)
    {
        foreach (Animator am in animators)
        {
            am.SetBool(aniParam, false );
        }
    }
}
