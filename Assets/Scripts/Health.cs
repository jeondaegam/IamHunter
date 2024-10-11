using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float hp;
    public IHealthListener healthListener;

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
        if (hp > 0)
        {
            hp -= damage;
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
