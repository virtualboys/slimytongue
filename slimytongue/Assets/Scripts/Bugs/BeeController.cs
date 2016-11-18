using UnityEngine;
using System.Collections;

public class BeeController : AIStateController {

    public float moveSpeed;
    public float chaseSpeed;

    public float chaseRange;

    public float timeToIdle;
    public float timeToStartChase;
    public float moveDist;

    void Start()
    {
        StartAIController();
    }

    void Update()
    {
        UpdateAIController();
    }

    private void MoveTowardTarget(Vector3 target, float speed)
    {
        Vector3 d = target - transform.position;
        d.Normalize();

        float s = speed * Time.deltaTime;
        
        transform.position += d * s;
        transform.rotation = Quaternion.LookRotation(d);
    }

    protected override void DoChase(Transform target)
    {
        MoveTowardTarget(target.position, chaseSpeed);
    }

    protected override void DoMove(Vector3 destination)
    {
        MoveTowardTarget(destination, moveSpeed);
    }

    protected override void DoIdle()
    {
        
    }

    protected override void DoStuck()
    {
        
    }

    protected override float GetChaseRange()
    {
        return chaseRange;
    }

    protected override float GetTimeToStartChase()
    {
        return timeToStartChase;
    }

    protected override void ReachedChaseTarget(GameObject target)
    {
        TongueController tc = target.GetComponent<TongueController>();
        tc.Strike();

        Rigidbody r = target.GetComponent<Rigidbody>();
        Vector3 f = transform.forward * 400;
        r.AddForce(f);
    }

    protected override void StartChase()
    {
        
    }

    protected override float StartIdle()
    {
        return timeToIdle;
    }

    protected override Vector3 StartMove()
    {
        Vector3 destOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        destOffset.Normalize();
        destOffset *= moveDist + Random.Range(-.5f, .5f) * moveDist;
        return transform.position + destOffset;
    }
}
