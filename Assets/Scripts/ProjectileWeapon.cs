using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    // 던지는 각도 
    public float projectileAngle = 30;
    // 던지는 힘 
    public float projectileForce = 10;
    // n초 후 폭발
    public float projectileTime = 5; 

    protected override void Fire()
    {
        projectileFire();
    }

    private void projectileFire()
    {
        // 카메라가 바라보고 있는 방향을 가져온다 .
        Camera cam = Camera.main;

        // 던질 각도 설정 
        Vector3 direction = cam.transform.forward; // <- 카메라가 바라보고 있는 방향

        // 위쪽으로 30도 앵글 올리기 (위쪽을 바라보도록)
        // 그러려면 x축을 회전
        direction = Quaternion.AngleAxis(-projectileAngle, transform.right) * direction;
        // projectileAngle만큼, transform.right를 중심으로) 위를 바라보려면 - 를 붙여야함
        direction.Normalize();
        // 힘을 곱한다 
        direction *= projectileForce;

        // 수류탄 인스턴스 생성 
        GameObject obj = Instantiate(projectilePrefab);
        obj.transform.position = firingPosition.position;
        obj.GetComponent<Bomb>().time = projectileTime;
        obj.GetComponent<Bomb>().damage = damage;
        // 던진다 
        obj.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }
}
