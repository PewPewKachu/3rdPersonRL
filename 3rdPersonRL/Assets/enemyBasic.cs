using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBasic : MonoBehaviour
{
    enum logicState { roam, followTarget, hostile, attack }
    [SerializeField] logicState logic;

    [SerializeField] float health, speed, attentionSpan, attackDistance;

    GameObject player;
    FieldOfView fov;

    bool agro;

    private void Start()
    {
        fov = GetComponent<FieldOfView>();

    }

    private void FixedUpdate()
    {
        if (!fov.FindTarget())
        {
            if (agro == true)
                Invoke("loseTarget", attentionSpan);
            agro = false;
        }
        else
        {
            if (agro == false)
            {
                logic = logicState.followTarget;
                player = fov.visibleTarget;
            }
            agro = true;
        }

        switch (logic)
        {
            case logicState.roam:
                break;

            case logicState.followTarget:
                if (player != null && Vector3.Distance(transform.position, player.transform.position) < attackDistance)
                    logic = logicState.attack;
                break;

            case logicState.hostile:
                break;

            case logicState.attack:
                if (player != null && Vector3.Distance(transform.position, player.transform.position) > attackDistance)
                    logic = logicState.followTarget;
                break;

            default:
                break;
        }

    }
    /*
    public logicState GetState()
    {
        return logic;
    }
    */
    public void loseTarget()
    {
        if (agro == false)
        {
            player = null;
            fov.setTargetNull();

            logic = logicState.roam;
        }
    }

    public void SetState(int _num)
    {
        if (_num == 1)
            logic = logicState.roam;
        else if (_num == 2)
            logic = logicState.followTarget;
        else if (_num == 3)
            logic = logicState.hostile;
        else if (_num == 4)
            logic = logicState.attack;
        else
            Debug.Log("stateSet_Incorrect");
    }

    //GROSS
    /*
    enum logicState { roam, followTarget }
    [SerializeField] logicState enemyState;

    public LayerMask mask;
    public UnityEngine.AI.NavMeshAgent agent;

    public float speed, attentionSpan;
    float counter;

    bool agro;

    RaycastHit hit;
    GameObject player;
    Vector3 origin, upAlil;

    private void Start()
    {
        origin = transform.position;
        upAlil = new Vector3(0,0.25f, 0);
        player = GameObject.FindGameObjectWithTag("Player");
        speed = Random.Range(speed - 2, speed + 2);
        agent.speed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!agro && Physics.Raycast(transform.position + upAlil, ((player.transform.position + upAlil) - (transform.position + upAlil)).normalized, out hit, Mathf.Infinity, mask))
        {
            if (hit.collider.tag == "Player")
                agro = true;
        }

        if (agro && Physics.Raycast(transform.position + upAlil, ((player.transform.position + upAlil) - (transform.position + upAlil)).normalized, out hit, Mathf.Infinity, mask))
        {
            Debug.DrawLine(transform.position + upAlil, new Vector3(hit.point.x, hit.point.y, hit.point.z), Color.red);
            if (hit.collider.tag == "Player")
            {
                enemyState = logicState.followTarget;
                counter = 0;
            }
            else
                counter++;
        }

        if (counter >= attentionSpan
            && counter != 0)
        {
            counter = 0;
            enemyState = logicState.roam;
            agro = false;
        }

        switch (enemyState)
        {
            case logicState.roam:
                if(agent.isOnNavMesh && Random.Range(0,25) == 0)
                    agent.SetDestination(origin + new Vector3(Random.Range(-4,5), 0, Random.Range(-4,5)));
                break;

            case logicState.followTarget:
                if(agent.isOnNavMesh)
                    agent.SetDestination(player.transform.position);
                break;
        }
    }
    */

}
