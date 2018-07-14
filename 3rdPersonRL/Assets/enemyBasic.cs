using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBasic : MonoBehaviour
{
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
}
