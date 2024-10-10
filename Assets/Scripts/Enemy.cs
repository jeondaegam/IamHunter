using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    NavMeshAgent agent;
    float timeForNextState = 2f;
    Animator animator;

    enum State
    {
        Idle,
        Walk
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
        agent.destination = player.position;
        // 애니메이션 분리 Idle <-> Walk

        //1. 걷는 상태인지 쉬는 상태인지 체크
        //2. 걸을 땐 > SetTrigger " Walk"
        // 3. 쉴 땐 > SetTrigger "Idle"
        // 4. 두 애니메이션 사이에 텀을 추가하기
        // 시간을 셀 변수 필요

        timeForNextState -= Time.deltaTime;
        if (timeForNextState <= 0)
        {
            switch (state)
            {
                case State.Idle:
                    state = State.Walk;
                    animator.SetTrigger("Walk");
                    // 쉬는 시간과 이동 시간을 매번 다르게 설정한다 .
                    // 동일할 경우 적 캐릭터가 모두 일정한 간격으로 쉬게 되겠지 ? 
                    timeForNextState = Random.Range(5f, 7f);
                    agent.isStopped = false;
                    break;

                case State.Walk:
                    state = State.Idle;
                    animator.SetTrigger("Idle");
                    timeForNextState = Random.Range(1f, 2f);
                    // TODO Idle 일때도 쉬는 상태로 따라오는데 ?
                    agent.isStopped = true;
                    break;

                default:
                    state = State.Idle;
                    animator.SetTrigger("Idle");
                    break;

            }
        }

    }
}
