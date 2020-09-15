using System;
using UnityEngine;
using System.Collections;

public class PopupBase : MonoBehaviour
{

    public Animator anim;
    public AnimationClip hidingAnimation;


    public Action<PopupBase> onOpened;
    public Action<PopupBase> onClosed;

    public Action assentCall;
    public Action cancelCall;



    public PopupType popupType;


    private AnimatorStateInfo info;
    private bool isShowing;

    protected virtual void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {

    }

    public virtual void OnShow()
    {
        // gameObject.SetActive(true);
        //动画效果
        //if (anim != null && IsIdle())
        {
            isShowing = true;
            //    anim.SetTrigger("show");
            onOpened(this);
        }


    }

    public virtual void OnClose()
    {

        if (isShowing == false) return;
        isShowing = false;

        //动画效果
        //if (anim != null && IsIdle() && hidingAnimation != null)
        //{
        //    anim.SetTrigger("hide");
        //    //Timer.Schedule(this, hidingAnimation.length, DoClose);

        //    float timeCount = 0.1f;
        //    DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        //        {
        //            //延时后的操作
        //        }));
        //}
        //else
        //{
        DoClose();
        //}


    }

    private void DoClose()
    {
        onClosed(this);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
        isShowing = false;
    }

    public bool IsIdle()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        return info.IsName("Idle");
    }

    public bool IsShowing()
    {
        return isShowing;
    }
}
