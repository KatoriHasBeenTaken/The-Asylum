using NUnit.Framework;
using System.Runtime.InteropServices.WindowsRuntime;
using UniGLTF.SpringBoneJobs;
using UnityEngine;

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
    private Transform ai, player;
    private float speed;
    private Animator animator;
    private string runTrigger;
    private bool started = false;
    public ChasePlayer(Transform ai, Transform player, float speed, string runTrigger = "run")
    {
        this.animator = animator;
        this.runTrigger = runTrigger;
        this.ai = ai;
        this.player = player;
        this.speed = speed;
    }

    public override StateNode evaluate()
    {
        ai.position = Vector3.MoveTowards(ai.position, player.position, speed * Time.deltaTime);
        return StateNode.Running;
    }
}
public class Patrol : btNode
{
    private Transform ai;
    private Transform[] points;
    private int index;
    private float speed;

    public Patrol(Transform ai, Transform[] points, float speed)
    {
        this.ai = ai;
        this.points = points;
        this.speed = speed;
        index = 0;
    }

    public override StateNode evaluate()
    {
        Vector3[] positions = new Vector3[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }
        if (Vector3.Distance(ai.position, positions[index]) < 0.1f)
            index = (index + 1) % positions.Length;

        ai.position = Vector3.MoveTowards(ai.position, positions[index], speed * Time.deltaTime);
        return StateNode.Running;
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
