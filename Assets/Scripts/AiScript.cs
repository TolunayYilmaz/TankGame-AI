using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiScript : MonoBehaviour
{
    public enum AISTATE { PATROL, CHASE, ATTACK };
    private Transform target1;
    private Transform target2;
    public string AiTag;
    public string WayPointTag;
    private NavMeshAgent ThisAgent;
    private Transform Player;
  
    private Transform Tank1;
    private Transform Tank2;
    public bool atTrigger = false;
    // State prop!!!
    private NavMeshAgent nav1;
    private NavMeshAgent nav2;
    public Transform[] Tank1Pos;
    public Transform[] Tank2Pos;
    private int randomPosTank1;
    OffMeshLink Field1;
    OffMeshLink Field2;
    // private int randomPosTank2; 
    public AISTATE CurrentState
    {
        get { return _CurrentState; }
        set
        {
            StopAllCoroutines();
            _CurrentState = value;

            switch (CurrentState)
            {
                case AISTATE.PATROL:
                    StartCoroutine(StatePatrol());
                    break;

                case AISTATE.CHASE:
                    StartCoroutine(StateChase());
                    break;

                case AISTATE.ATTACK:
                    StartCoroutine(StateAttack());
                    break;
            }
        }
    }

    // field
    [SerializeField]
    private AISTATE _CurrentState = AISTATE.PATROL;

    private void Awake()
    {
        ThisAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag(AiTag).GetComponent<Transform>();
    }

    private void Start()
    {
        CurrentState = AISTATE.PATROL;
        Create();
        
  
    }


    private void Update()
    {
        TankMovement();
    }
    public IEnumerator StateChase()
    {
        float AttackDistance = 2f;

        while (CurrentState == AISTATE.CHASE)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < AttackDistance)
            {
                CurrentState = AISTATE.ATTACK;
                yield break;
            }


            ThisAgent.SetDestination(Player.transform.position);
            yield return null;
        }
    }

    public IEnumerator StateAttack()
    {
        float AttackDistance = 2f;

        while (CurrentState == AISTATE.ATTACK)
        {
            

            if (Vector3.Distance(transform.position, Player.transform.position) > AttackDistance)
            {
              
                CurrentState = AISTATE.CHASE;
                yield break;
            }

            print("Attack!");
            ThisAgent.SetDestination(Player.transform.position);
            StartCoroutine(BotWait());
            StartCoroutine(BridgeWait());


            yield return null;
        }
    }

    public IEnumerator StatePatrol()
    {
        GameObject[] Waypoints = GameObject.FindGameObjectsWithTag(WayPointTag);
        GameObject CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
        float TargetDistance = 2f;

        while (CurrentState == AISTATE.PATROL)
        {
            ThisAgent.SetDestination(CurrentWaypoint.transform.position);
           
            if (Vector3.Distance(transform.position, CurrentWaypoint.transform.position) < TargetDistance)
            {
                CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];

            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(AiTag)) return;

        CurrentState = AISTATE.CHASE;
    }
    void Bot()
    {
        atTrigger = true;
        if (atTrigger == true)
        {
            Debug.Log("Girdi");
            nav1.SetDestination(target1.position);
            nav2.SetDestination(target2.position);
        }
    }
    IEnumerator BotWait()
    {
        yield return new WaitForSecondsRealtime(2f);
        Bot();

    }
    void Create()
    {
     
        Tank1 = GameObject.FindGameObjectWithTag("Tank1").GetComponent<Transform>();
        Tank2 = GameObject.FindGameObjectWithTag("Tank2").GetComponent<Transform>();
        nav1 = GameObject.FindGameObjectWithTag("Tank1").GetComponent<NavMeshAgent>();
        nav2 = GameObject.FindGameObjectWithTag("Tank2").GetComponent<NavMeshAgent>();
        target1 = GameObject.FindGameObjectWithTag("Tower1").GetComponent<Transform>();
        target2 = GameObject.FindGameObjectWithTag("Tower2").GetComponent<Transform>();
        Field1 = GameObject.FindGameObjectWithTag("Field").GetComponent<OffMeshLink>();
        Field2 = GameObject.FindGameObjectWithTag("Field2").GetComponent<OffMeshLink>();
        randomPosTank1 = Random.Range(0, Tank1Pos.Length);
    }
    void TankMovement()
    {
        if (atTrigger == false)
        {
            
            Tank1.transform.position = Vector3.MoveTowards(Tank1.transform.position, Tank1Pos[randomPosTank1].position, 5 * Time.deltaTime);
            Tank2.transform.position = Vector3.MoveTowards(Tank2.transform.position, Tank2Pos[randomPosTank1].position, 5 * Time.deltaTime);
            if (Vector3.Distance(Tank1.transform.position, Tank1Pos[randomPosTank1].position) < 2f)
            {
                randomPosTank1 = Random.Range(0, Tank1Pos.Length);

            }
        }
    }
    void bridgeTime() {
      
        Field1.activated = true;
        Field2.activated = true;
        
       
    }

    IEnumerator BridgeWait()
    {
        yield return  new WaitForSecondsRealtime(6f);
        bridgeTime();
      
    }
}