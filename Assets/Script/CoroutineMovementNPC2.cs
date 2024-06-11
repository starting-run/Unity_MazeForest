using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CoroutineMovementNPC2 : MonoBehaviour
{
    public enum Kind
    {
        ToTarget,
        Player
    }

    public Transform target;
    public string animationName;
    public Kind kind = Kind.ToTarget;

    public float aniSpeed = 0;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Coroutine moveToPlayerCoroutine;
    Vector3 old = Vector3.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            return;
        }

    }

    void Update()
    {
        float mag = (transform.position - old).magnitude;

        if (mag == 0) animator.SetFloat("Speed", 0);
        else if (mag < 0.02) animator.SetFloat("Speed", 0.2f);
        else animator.SetFloat("Speed", 0.5f);
        old = transform.position;
    }

    public void RemoteNPCStart()
    {
        StartCoroutine(DelayedStartCoroutine());
    }
    IEnumerator DelayedStartCoroutine()
    {
        yield return new WaitForSeconds(3);
        moveToPlayerCoroutine = StartCoroutine(MoveToPlayer());
    }

    IEnumerator MoveToPlayer()
    {
        if (target == null)
        {
            yield break;
        }

        while (true)
        {
            navMeshAgent.SetDestination(target.position);

            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                yield return null;
            }

            yield return null;
        }
    }

    public void StopMovementAndPlayAnimation()
    {
        if (moveToPlayerCoroutine != null)
        {
            StopCoroutine(moveToPlayerCoroutine);
            moveToPlayerCoroutine = null;
        }

        animator.Play(animationName);
    }
}
