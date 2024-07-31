using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject[] targets;
    private NavMeshModifierVolume modifierVolume;
    private Animator animator;
    private int currentTargetIndex = 0;
    private bool isMoving =  true;
    private float timer = 0f;
    private float waitTime = 3f;

void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        modifierVolume = GameObject.Find("DanceVolume").GetComponent<NavMeshModifierVolume>();
        SetNextTarget();
    }

    void Update()
    {
        if (isMoving)
        {
            if (!agent.pathPending && agent.remainingDistance < 3f)
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(agent.destination, path);
                bool isInModifierVolume = false;
                animator.SetFloat("Move", 0f);
                foreach (Vector3 corner in path.corners)
                {
                    if (modifierVolume.GetComponent<Collider>().bounds.Contains(corner))
                    {
                        isInModifierVolume = true;
                        break;
                    }
                }

                if (isInModifierVolume)
                {
                    animator.SetTrigger("Dance");
                }
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    SetNextTarget();
                }
            }
        }
    }

    void SetNextTarget()
    {
        isMoving = false;
        timer = 0f;
        currentTargetIndex = Random.Range(0, targets.Length);
        agent.destination = targets[currentTargetIndex].transform.position;
        animator.SetFloat("Move", 0.2f);
        isMoving = true;
    }
}