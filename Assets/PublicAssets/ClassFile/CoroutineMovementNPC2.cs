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

    public Kind kind = Kind.ToTarget;

    public float aniSpeed = 0;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    Vector3 old = Vector3.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            return;
        }

        // 인트로로 인해 11초뒤 따라오는 npc 동작
        if (kind == Kind.Player) StartCoroutine(DelayedStartCoroutine());
    }

    void Update()
    {
        float mag = (transform.position - old).magnitude;

        if (mag == 0) animator.SetFloat("Speed", 0);
        else if (mag < 0.02) animator.SetFloat("Speed", 0.2f);
        else animator.SetFloat("Speed", 0.5f);
        old = transform.position;
    }

    IEnumerator DelayedStartCoroutine()
    {
        yield return new WaitForSeconds(11);
        StartCoroutine(MoveToPlayer());
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
}
