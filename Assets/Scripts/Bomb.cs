 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;
    public float damage;

    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if(!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(explosionSound);
            }

            // 애니메이션 실행 
            GetComponent<Animator>().SetTrigger("Explosion");
            Invoke("DestroyThis", 3f); // 폭발 사운드 재생시간이 약 3초정도 됨 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().Damage(damage);
        }
    }


    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
