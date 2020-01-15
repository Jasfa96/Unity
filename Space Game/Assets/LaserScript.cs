using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] float speed;
    GameObject player;
    //[SerializeField] GameObject Player;


    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //transform.rotation = Player.transform.rotation;
        //transform.rotation = GetComponentInParent<Transform>().rotation;
        Movement();
    }

    private void Movement(){
        /*Vector3 pos= transform.position;
        
        pos.z +=  speed * Time.deltaTime;

        transform.position = pos;*/



        transform.position += player.transform.forward *(speed *Time.deltaTime);
        
        

    }
}
