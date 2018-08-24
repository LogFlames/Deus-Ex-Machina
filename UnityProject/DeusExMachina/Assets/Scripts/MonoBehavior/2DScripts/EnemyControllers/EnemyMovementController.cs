using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyMovementController : MonoBehaviour
{

    private Rigidbody2D rb;

    private Seeker seeker;

    [SerializeField] private Transform target;

    [SerializeField] private int updateRate;

    [SerializeField] private float movementSpeed;

    private Path path;

    [HideInInspector] public bool pathIsEnded;

    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private Vector3 positionFollowOffset;

    private bool following;

    private int currentWaypoint;

    private bool runOnEnable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        StartCoroutine(UpdatePath());
    }

    private IEnumerator UpdatePath()
    {
        following = true;
        WaitForSeconds timer = new WaitForSeconds(1f / updateRate);

        while (true)
        {
            if (target == null)
            {
                target = Player2D.instance.transform;
                if (target.gameObject.activeSelf)
                {
                    seeker.StartPath(transform.position, target.position + positionFollowOffset, OnPathComplete);
                }
            }
            else
            {
                seeker.StartPath(transform.position, target.position + positionFollowOffset, OnPathComplete);
            }

            yield return timer;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            target = Player2D.instance.transform;
            return;
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }

            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        Vector3 directionToNextWaypoint = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        directionToNextWaypoint *= movementSpeed * Time.fixedDeltaTime;
        rb.AddForce(directionToNextWaypoint, ForceMode2D.Force);

        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance <= nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    void OnEnable()
    {
        if (!runOnEnable)
        {
            runOnEnable = true;
            return;
        }

        if (!following)
        {
            StartCoroutine(UpdatePath());
        }
    }

    void OnDisable()
    {
        following = false;
    }
}
