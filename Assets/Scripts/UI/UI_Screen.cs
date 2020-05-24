using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class UI_Screen : MonoBehaviour
{

    [Header("Main Properties")]
    public Selectable startSelectable;
    private Animator animator;

    [Header("Screen Events")]
    public UnityEvent onScreenStart = new UnityEvent();
    public UnityEvent onScreenClose = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (startSelectable)
        {
            EventSystem.current.SetSelectedGameObject(startSelectable.gameObject);
        }
    }

    public virtual void StartScreen()
    {
        if (onScreenStart != null)
        {
            onScreenStart.Invoke();
        }

        handleAnimator("show");

    }

    public virtual void CloseScreen()
    {
        if (onScreenClose != null)
        {
            onScreenClose.Invoke();
        }

        handleAnimator("hide");
    }

    void handleAnimator(string trigger)
    {
        if (animator)
        {
            animator.SetTrigger(trigger);
        }
    }

}
