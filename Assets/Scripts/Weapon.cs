using System;
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

    // 발사 궤적 그리기
    public GameObject trailPrefab;
    // 총구 위치
    public Transform firingPosition;
    // 파티클
    public GameObject shotSparkParticle;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && bullet > 0)
        {
            if (animator != null)
            {
            animator.SetTrigger("Shot");
            }
            bullet--; 
            Fire();
        }
        else if (Input.GetButtonDown("Reload"))
        {
            if (animator != null)
            {
                animator.SetTrigger("Reload");
            }

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

    virtual protected void Fire()
    {
        RacastFire();
    }


    // 목표물까지 선을 그린다 .
    private void RacastFire()
    {
        // 1. 현재 비추고 있는 화면 정보를 가져온다 .
        Camera cam = Camera.main;

        // 2. 빛을 쏜 결과값을 받을 변수
        RaycastHit hit;

        // 3. 발사할 빛과 초점을 설정한다 . (viewport)
        Ray r = cam.ViewportPointToRay(Vector3.one / 2);

        // 4. 부딪힌 지점 정보를 받을 변수
        // TODO - 어차피 뒤에서 값이 재설정 되는데 왜 초기화 하는거지 ?
        // 빛이 어딘가에 부딪히지 않았을 경우 기본값을 설정하는 건가 ? 
        Vector3 hitPosition = r.origin + r.direction * 200;

        // 5. 빛을 쏜다 .
        if (Physics.Raycast(r, out hit, 1000))
        {
            hitPosition = hit.point;

            // 적중시 먼지 파티클
            GameObject particle = Instantiate(shotSparkParticle);
            particle.transform.position = hitPosition;
            particle.transform.forward = hit.normal;

        }

        // ------------- 위에 까지는 , 빛이 부딪힐 지점을 설정하는 과정

        // 선을 그린다 .
        if (trailPrefab != null)
        {
            GameObject trail = Instantiate(trailPrefab);
            Vector3[] pos = new Vector3[] { firingPosition.transform.position, hitPosition };
            // pos 배열에 들어있는 각 점들 사이에 선을 그린다. 
            trail.GetComponent<LineRenderer>().SetPositions(pos);

            // 선을 지운다
            // RemoveTrail 함수를 Coroutine 형식으로 호출하겠다 .
            // StartCoroutine은 함수의 종료를 기다리지 않고 다음으로 넘어간다 . 
            StartCoroutine(RemoveTrail(trail));
        }

    }

    // Couroutine은 IEnumerator 타입이어야 하고, 함수 안에서 마음대로 시간을 끌 수 있다 . 
    private IEnumerator RemoveTrail(GameObject trail)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(trail);
    }
}
