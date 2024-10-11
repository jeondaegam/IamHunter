using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        // 카메라가 바라보는 방향을 현재 오브젝트도 같이 바라보게 된다.
        // 즉 오브젝트가 항상 카메라와 수평이 된다.  
    }
}
