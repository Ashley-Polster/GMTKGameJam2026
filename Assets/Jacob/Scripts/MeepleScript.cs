using System.Collections;
using TMPro;
using UnityEngine;

public class MeepleScript : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sr;
    Rigidbody2D rb;
    Vector2 velocity;
    bool coroutineIsDone;

    public bool isResenter = false;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(run());
    }

    void FixedUpdate()
    {
        rb.linearVelocity = velocity;
        

        if (coroutineIsDone)
        {
            StartCoroutine(run());
        }
    }

    public void changeMeepleType()
    {
        if (isResenter)
        {
            isResenter = false;
            animator.SetBool("isMad", false);
        }
        else
        {
            isResenter = true;
            animator.SetBool("isMad", true);
        }
    }

    public void apocalypseAnimation()
    {
        animator.SetBool("isScreaming", true);
    }

    IEnumerator run()
    {
        coroutineIsDone = false;
        int randomTime = Random.Range(2,4);
        if (randomTime % 2 == 0)
        {
            velocity = new Vector2(1,0);
            sr.flipX = false;
        }
        else
        {
            velocity = new Vector2(-1,0);
            sr.flipX = true;
        }
        animator.SetBool("isRunning", true);
        yield return new WaitForSeconds(randomTime);
        velocity = Vector2.zero;
        animator.SetBool("isRunning", false);
        yield return new WaitForSeconds(1f);
        coroutineIsDone = true;
        yield return null;
    }
}
