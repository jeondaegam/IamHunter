using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float hp;
    public IHealthListener healthListener;
    // 무적시간 추가
    // (연속으로 닿아 한번에 두대 맞는 것을 방지하기 위해 무적 시간을 둔다) 
    float invincibleTime = 0.5f;
    float lastAttacTime;

    // Start is called before the first frame update
    void Start()
    {
        healthListener = GetComponent<IHealthListener>();
        // 이 인터페이스를 따르고 있는 컴포넌트를 그대로 가져온다 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        // 마지막으로 맞은 시간으로부터 쿨타임이 지난 뒤면 데미지를 입는다. 
        if (hp > 0 && (lastAttacTime + invincibleTime < Time.time))
        {
            hp -= damage;
            lastAttacTime = Time.time;
            Debug.Log($"hp:{hp}, damage:{damage}");

            if (hp <= 0)
            {
                if (healthListener != null)
                {
                    healthListener.Die();
                }
            }
        }
    }


    public interface IHealthListener
    {
        void Die();
    }
}
