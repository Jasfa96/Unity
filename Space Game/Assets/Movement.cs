using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Transform playerModel;

    [SerializeField] float xySpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform shoot_point;

    void Start()
    {
        playerModel = transform.GetChild(0);
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        LocalMove(h, v, xySpeed);
        
        HorizontalLean(playerModel, h, 50);
        VerticalLean(playerModel, v, 50);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

    }
    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    
    void HorizontalLean(Transform target, float axis, float leanLimit)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, Time.deltaTime * xySpeed));
    }

    void VerticalLean(Transform target, float axis, float leanLimit)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEulerAngels.x, -axis * leanLimit, Time.deltaTime * xySpeed), targetEulerAngels.y, targetEulerAngels.z);
    }

    void Shoot(){
        /*
        RaycastHit hit;
        Ray lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(lastRay, out hit)){
            GameObject ej = Instantiate(bullet, shoot_point.position, Quaternion.LookRotation(hit.normal));
        }*/

        Instantiate(bullet, shoot_point.position, playerModel.rotation);

         
    }

    
}