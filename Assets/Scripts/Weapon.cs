using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update

    // 탄창
    // 현재 탄창안의 총알 개수 
    public int bullet;
    // 남은 총알 개수 
    public int totalBullet;
    // 탄창에 들어갈 수 있는 최대 총알 개수 
    public int maxBulletInMagazine;
    public TextMeshProUGUI bulletNumberLabel;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && bullet > 0)
        {
            bullet--;
            animator.SetTrigger("Shot");

        }
        else if (Input.GetButtonDown("Reload"))
        {
            // 재장전 해야할 총알 개수 
            int reloadBullet = maxBulletInMagazine - bullet;

            // 남은 전체 총알 개수가 재장전 해야할 개수보다 많으면 
            if (totalBullet >= reloadBullet)
            {
                // 전체 총알에서 재장전할 총알 개수를 빼고 
                totalBullet -= reloadBullet;

                // 총알을 채워준다
                bullet = maxBulletInMagazine;
            }
            else
            {
                // 전체 총알 개수가 부족하면
                // 남은 총알 모두를 재장전 해주고
                bullet += totalBullet;
                totalBullet = 0;
            }

        }
        bulletNumberLabel.text = $"{bullet}/{totalBullet}";
    }
}
