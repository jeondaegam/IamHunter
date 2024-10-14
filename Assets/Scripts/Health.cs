using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp; // 현재 체력
    public Image hpGauge; // 체력바
    float maxHp; // 최대 체력 

    public IHealthListener healthListener;
    // 무적시간 추가
    // (연속으로 닿아 한번에 두대 맞는 것을 방지하기 위해 무적 시간을 둔다) 
    float invincibleTime = 0.5f;
    float lastAttacTime;

    // sound
    public AudioClip dieSound;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
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
            // 체력 감소 
            hp -= damage;
            // 체력바 게이지 감소
            if (hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHp;
            }

            lastAttacTime = Time.time;
            Debug.Log($"hp:{hp}, damage:{damage}");

            // 사망 
            if (hp <= 0)
            {
                GetComponent<AudioSource>().PlayOneShot(dieSound);
                if (healthListener != null)
                {
                    healthListener.Die();
                }
            } else
            {
                GetComponent<AudioSource>().PlayOneShot(hitSound);
            }
        }
    }


    public interface IHealthListener
    {
        void Die();
    }
}
