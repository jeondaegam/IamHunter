using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Health.IHealthListener
{
    public Transform player;
    NavMeshAgent agent;
    float timeForNextState = 2f;
    Animator animator;

    enum State
    {
        Idle,
        Walk,
        Attack,
        Dying
    }

    State state;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
        agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 시간에 의지해 상태가 변하는게 아니라
        // 특정 조건에 따라 상태가 변하도록 수정하자 (많이 사용하는 방식) 

        // 1. 상태 추가 : Dying, Attack
        // 2. 휴식중일때
        //플레이어와의 거리를 구한다
        //    거리가 1m이내이면 공격
        //    im 밖이면 휴식시간이 지나기를 기다리며 시간을 잰다
        //    만약 시간이 다되면 걷는다

        //2. 걷는중일때
        //플레이어에게 도달했거나, 가는 길이 없으면 휴식한다 // TODO 도달하면 Attack 아닌가 ?
        // 플레이어에게 도착하면 일단 멈추고 -> 휴식한뒤 공격하는구나 

        // 3. 공격중일때
        // 일정 시간이 지나면 Idle로 돌아간다 


        switch (state)
        {
            case State.Idle:
                // TODO - 적과 플레이어의 실시간 위치값을 계산 
                float distance = (player.position -
                    (transform.position + GetComponent<CapsuleCollider>().center)).magnitude; // magnitude를 붙이면 Vector의 길이를 float으로 받아온다 . 
                if (distance < 1.0f)
                {
                    Attack();
                } else
                {
                    timeForNextState -= Time.deltaTime;
                    if (timeForNextState < 0)
                    {
                         StartWalk();
                    }
                }
                break;
            case State.Walk:
                // TODO - agent.remainingDistance: agent가 기억하는 목표지점과의 남은 거리
                // 목표지점: 지금은 플레이어가 다른곳으로 이동했을 수도 있기때문에 그 위치에 플레이어가 없을수도 있음 
                if (agent.remainingDistance < 1.0f || !agent.hasPath)
                {
                    StartIdle();
                }
                break;
            case State.Attack:
                timeForNextState -= Time.deltaTime;
                if (timeForNextState < 0)
                {
                    StartIdle();
                }
                break;

        }



    }

    private void StartIdle()
    {
        state = State.Idle;
        timeForNextState = Random.Range(1f, 2f);
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }


    private void StartWalk()
    {
        state = State.Walk;
        // 목표 지점을 설정한다 . 
        agent.destination = player.position;
        agent.isStopped = false;
        animator.SetTrigger("Walk");
    }


    private void Attack()
    {
        state = State.Attack;
        timeForNextState = 1.5f;
        animator.SetTrigger("Attack");
    }


    public void Die()
    {
        // 적이 죽었을 때의 Action을 구현
        state = State.Dying;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        // 잠시동안 쓰러져있다 사라지도록 2.5초의 텀을 설정 
        Invoke("DestroyThis", 2.5f);

        Debug.Log("Dying..");

    }


    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
