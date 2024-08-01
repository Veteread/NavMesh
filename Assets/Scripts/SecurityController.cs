using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SecurityController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject[] visitors;
    [SerializeField] private BoxCollider boxCollider;
    private Animator animator;
    private int currentTargetIndex = 0;
    private bool isMoving =  true;
    private float timer = 0f;
    private float waitTime = 3f;

void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        visitors = GameObject.FindGameObjectsWithTag("visitor");
        SetNextVisitor();
    }

    void Update()
    {
        if (isMoving)
        {
            if (!agent.pathPending && agent.remainingDistance < 2.1f)
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(agent.destination, path);                
                animator.SetFloat("Move", 0f);
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    SetNextVisitor();
                }
            }
        }
    }

    void SetNextVisitor()
    {
        isMoving = false;
        timer = 0f;

        while (true)
        {
            currentTargetIndex = Random.Range(0, visitors.Length);
            Vector3 visitorPosition = visitors[currentTargetIndex].transform.position;

            if (boxCollider.bounds.Contains(visitorPosition))
            {
                agent.destination = visitorPosition;
                animator.SetFloat("Move", 0.2f);
                isMoving = true;
                break;
            }
            else
            {
                if (currentTargetIndex == visitors.Length - 1)
                {
                    InvokeRepeating("SetNextVisitor", 5f, 5f);
                    break;
                }
            }
        }
    }
}