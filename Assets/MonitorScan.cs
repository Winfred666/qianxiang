using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorScan : MonoBehaviour
{

    [SerializeField] private float scanAngularSpeed;
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;

    [SerializeField] private Vector2 lensOffset;
    
    private Vector3 rotateDirection = Vector3.forward;


    private LayerMask playerLayer;
    private float radius;
    private Vector2 lensPosition;
    private PolygonCollider2D polygon;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        polygon = GetComponent<PolygonCollider2D>();
        
        Vector2 size = transform.GetComponent<Renderer>().bounds.size;

        lensPosition = transform.position;
        lensPosition.x+=lensOffset.x*size.x;
        lensPosition.y+=lensOffset.y*size.y;

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

    //检测已经在playerHealth中完成，只需要将预设加入trapss图层
    // private void OnTriggerEnter2D(Collider2D colli){
    //     if(colli.gameObject.layer == playerLayer){
    //         GameManager.playerDied(transform);
    //     }
    // }

}
