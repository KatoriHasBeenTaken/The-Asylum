using NUnit.Framework;
using System.Runtime.InteropServices.WindowsRuntime;
using UniGLTF.SpringBoneJobs;
using UnityEngine;
using UnityEngine.AI;

public enum StateNode
{
    Sucess,
    Fail,
    Running
}
public abstract class btNode
{
    public abstract StateNode evaluate();
   
}
public class selector : btNode
{
    public override StateNode evaluate()
    {
        foreach(btNode node in nodes)
        {
            StateNode state = node.evaluate();
            if(state == StateNode.Sucess || state == StateNode.Running)
            {
                return state;
            }
            
        }
        return StateNode.Fail;
    }
    public btNode[] nodes;
    public selector(btNode[] nodes)
    {
        this.nodes = nodes;
    }   
}
    public class sequence:btNode
{

    public btNode[] nodes;
    public sequence(btNode[] nodes)
    {
        this.nodes = nodes;
    }

    public override StateNode evaluate()
    {
        foreach (btNode node in nodes)
        {
            StateNode states = node.evaluate();
            if(states == StateNode.Fail)
            {
                return StateNode.Fail;
            }
            if(states == StateNode.Running)
            {
                return StateNode.Running;
            }
        }
        return StateNode.Sucess;
        
    }
}
public class CheckPlayerInRange : btNode
{
    private Transform ai, player;
    private float range;

    public CheckPlayerInRange(Transform ai, Transform player, float range)
    {
        this.ai = ai;
        this.player = player;
        this.range = range;
    }

    public override StateNode evaluate()
    {
        float dist = Vector3.Distance(ai.position, player.position);
        return dist <= range ? StateNode.Sucess : StateNode.Fail;
    }
}
public class ChasePlayer : btNode
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private int runHash;
    private bool playedRunOnce = false;

    private float repathInterval = 0.2f;
    private float repathTimer = 0f;

    // runAnimName: tên state/trigger bạn muốn phát (tuỳ controller)
    public ChasePlayer(NavMeshAgent agent, Transform player, Animator animator = null, string runAnimName = "Run")
    {
        this.agent = agent;
        this.player = player;
        this.animator = animator;
        this.runHash = Animator.StringToHash(runAnimName);
    }

    public override StateNode evaluate()
    {
        if (agent == null || player == null || !agent.enabled) return StateNode.Fail;

        if (animator != null && !playedRunOnce)
        {
            // Nếu dùng Trigger: animator.SetTrigger(runHash);
            // Nếu dùng state:   animator.Play(runHash);
            animator.Play(runHash);
            playedRunOnce = true;
        }

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            if (NavMesh.SamplePosition(player.position, out var hit, 2.0f, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
            repathTimer = repathInterval;
        }

        if (agent.pathPending) return StateNode.Running;
        if (agent.remainingDistance > agent.stoppingDistance) return StateNode.Running;

        return StateNode.Sucess; // đã áp sát
    }
}

// PATROL bằng NavMeshAgent
public class Patrol : btNode
{
    private NavMeshAgent agent;
    private Transform[] points;
    private int index = -1;
    private bool started = false;

    public Patrol(NavMeshAgent agent, Transform[] points)
    {
        this.agent = agent;
        // Lọc null để tránh lỗi khi quên gán 1 waypoint
        if (points != null)
        {
            var list = new System.Collections.Generic.List<Transform>();
            foreach (var p in points) if (p != null) list.Add(p);
            this.points = list.ToArray();
        }
        else this.points = System.Array.Empty<Transform>();
    }

    public override StateNode evaluate()
    {
        if (agent == null || !agent.enabled || points == null || points.Length == 0)
            return StateNode.Fail;

        if (!started)
        {
            index = 0;
            SetNext();
            started = true;
            return StateNode.Running;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            index = (index + 1) % points.Length;
            SetNext();
        }

        return StateNode.Running;
    }

    private void SetNext()
    {
        var target = points[index].position;
        if (NavMesh.SamplePosition(target, out var hit, 2.0f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}
public class CloseToPlayer : btNode
{
    private Transform ai;
    private Transform player;
    private float closeRange;

    public CloseToPlayer(Transform ai, Transform player, float closeRange = 1.5f)
    {
        this.ai = ai;
        this.player = player;
        this.closeRange = closeRange;
    }

    public override StateNode evaluate()
    {
        float dist = Vector3.Distance(ai.position, player.position);
        return dist <= closeRange ? StateNode.Sucess : StateNode.Fail;
    }
}


    public class Jumpscare : btNode
    {
        private Animator animator;
        private string jumpscareTrigger;
        private bool started = false;
        private Transform playerCamera;
        private Transform ai;
        private float rotationTime = 0.3f; // thời gian xoay mượt
        private float rotationElapsed = 0f;

        public Jumpscare(Animator animator, Transform ai, Transform playerCamera, string jumpscareTrigger = "Jumpscare")
        {
            this.animator = animator;
            this.jumpscareTrigger = jumpscareTrigger;
            this.playerCamera = playerCamera;
            this.ai = ai;
        }

        public override StateNode evaluate()
        {
            if (!started)
            {
                // Bắt đầu jumpscare
                animator.Play(jumpscareTrigger);
                started = true;
                rotationElapsed = 0f;
                return StateNode.Running;

            }

            // Trong lúc xoay mượt camera
            if (rotationElapsed < rotationTime)
            {
                rotationElapsed += Time.deltaTime;
                Vector3 lookDir = (ai.position - playerCamera.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, rotationElapsed / rotationTime);
                return StateNode.Running;

            }

            // Kiểm tra xem animation jumpscare còn chạy không
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(jumpscareTrigger))
            {
                return StateNode.Running;
            }

            return StateNode.Sucess;
        }
    }
