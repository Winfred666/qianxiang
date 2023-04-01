using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{

    [SerializeField] private float scanAngularSpeed;
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;

    private Vector3 rotateDirection = Vector3.forward;

    public static RedZone redZone;

    private LayerMask playerLayer;
    private float radius;
    private Vector2 lensPosition;
    private PolygonCollider2D polygon;

    void Start()
    {
        redZone = this;

        playerLayer = LayerMask.NameToLayer("Player");
        polygon = GetComponent<PolygonCollider2D>();
        
        Vector2 size = transform.GetComponent<Renderer>().bounds.size;

        lensPosition = transform.position;

        startAngle+=360f;
    }

    void Update()
    {
        if(transform.localEulerAngles.z > 180f && transform.localEulerAngles.z < startAngle){
            rotateDirection = Vector3.forward;
        }
        else if(transform.localEulerAngles.z < 180f && transform.localEulerAngles.z > endAngle)
            rotateDirection = Vector3.back;

        transform.RotateAround(lensPosition,rotateDirection, scanAngularSpeed*Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D colli){
        if(colli.gameObject.layer == playerLayer){
            
            //GameManager.playerDied();
        }
    }

    public float getRotateAngle(){
        float ret = transform.localEulerAngles.z;
        if(ret>180) ret-=360f;
        return ret;
    }

    public bool getIsMirror(){
        if(transform.localEulerAngles.z<180 && transform.localEulerAngles.z>30){
            return true;
        }
        return false;
    }
}
