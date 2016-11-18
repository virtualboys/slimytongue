using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AIState
{
    Idle,
    Moving,
    Chasing,
    Stuck
}

public abstract class AIStateController : MonoBehaviour {

    private const float c_distanceThreshold = .005f;

    private AIState m_state;

    private Transform m_chaseTarget;
    private float m_timeToStartChase;
    private float m_chaseStartTimer;

    private Vector3 m_destination;

    private float m_idleTimer;
    private float m_timeToIdle;

    protected abstract void DoIdle();
    protected abstract void DoMove(Vector3 destination);
    protected abstract void DoChase(Transform target);
    protected abstract void DoStuck();
    protected abstract void ReachedChaseTarget(GameObject target);

    protected abstract float GetChaseRange();
    protected abstract float GetTimeToStartChase();

    /// <summary>
    /// Start moving to destination
    /// </summary>
    /// <returns>The destination</returns>
    protected abstract Vector3 StartMove();
    /// <summary>
    /// Start idling
    /// </summary>
    /// <returns>The amount of time to idle</returns>
    protected abstract float StartIdle();
    /// <summary>
    /// Start chasing target
    /// </summary>
    protected abstract void StartChase();
    
	protected void StartAIController () {
        TransitionToIdle();
	}
	
	protected void UpdateAIController () {
        float dt = Time.deltaTime;

	    switch(m_state)
        {
            case AIState.Idle:
                DoIdle();
                CheckIdleState(dt);
                break;
            case AIState.Moving:
                DoMove(m_destination);
                CheckMovingState(dt);
                break;
            case AIState.Chasing:
                DoChase(m_chaseTarget);
                CheckChasingState();
                break;
            case AIState.Stuck:
                DoStuck();
                CheckStuckState();
                break;
        }
	}

    private void CheckIdleState(float dt)
    {
        m_idleTimer += dt;
        if(m_idleTimer >= m_timeToIdle)
        {
            TransitionToMoving();
        }
    }

    private void CheckMovingState(float dt)
    {
        // reached destination
        if(TargetInRange(m_destination, c_distanceThreshold))
        {
            TransitionToIdle();
        }
    }

    private void CheckChasingState()
    {
        // out of chase range
        if(!TargetInRange(m_chaseTarget.position, GetChaseRange()))
        {
            TransitionToIdle();
        }
        // reached chase target
        else if(TargetInRange(m_chaseTarget.position, c_distanceThreshold))
        {
            ReachedChaseTarget(m_chaseTarget.gameObject);
            TransitionToIdle();
        }
    }
    
    private void LookForChaseTarget(float dt)
    {
        // we have a target, see if it stays in range for m_timeToStartChase
        if(m_chaseTarget != null)
        {
            // target still in range
            if(TargetInRange(m_chaseTarget.position, GetChaseRange()))
            {
                m_chaseStartTimer += dt;
                if(m_chaseStartTimer > m_timeToStartChase)
                {
                    TransitionToChase();
                }
            }
            // target left range
            else
            {
                m_chaseTarget = null;
            }
        }
        // look for players in range
        else
        {
            List<Transform> playersInRange = new List<Transform>();
            List<GameObject> players = PlayerInput.players;
            for(int i = 0; i < players.Count; i++)
            {
                if(TargetInRange(players[i].transform.position, GetChaseRange()))
                {
                    playersInRange.Add(players[i].transform);
                }
            }

            if(playersInRange.Count != 0)
            {
                m_chaseTarget = playersInRange[Random.Range(0, playersInRange.Count)];
                m_timeToStartChase = GetTimeToStartChase();
                m_chaseStartTimer = 0;
            }
        }
    }

    private void TransitionToIdle()
    {
        Debug.Log("Idle");
        m_chaseTarget = null;
        m_state = AIState.Idle;
        m_idleTimer = 0;
        m_timeToIdle = StartIdle();
    }

    private void TransitionToMoving()
    {
        Debug.Log("Move");
        m_state = AIState.Moving;
        m_destination = StartMove();
    }

    private void TransitionToChase()
    {
        Debug.Log("Chase");
        m_state = AIState.Chasing;
        StartChase();
    }

    private bool TargetInRange(Vector3 target, float range)
    {
        Debug.Log((target - transform.position).magnitude);
        return (target - transform.position).magnitude <= range;
    }

    private void CheckStuckState()
    {

    }
}
