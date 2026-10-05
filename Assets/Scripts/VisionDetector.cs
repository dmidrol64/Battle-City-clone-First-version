using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class VisionDetector : MonoBehaviour
{

    EnemyMove enemy;
    [SerializeField] float distance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float xSize;
    [SerializeField] float ySize;
     
    
    // Start is called before the first frame update
    void Start()
    {
        
        enemy = GetComponentInParent<EnemyMove>();
        
    }

   
    // Update is called once per frame
    void Update()
    {
        Detect();
    }
    private void OnDrawGizmos()
    {
        Vector3 size = new Vector3(xSize, ySize,0);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, size );

    }
    void Detect()
    {
        Vector2 size = new Vector2(xSize, ySize);
        float angleTankRotation = transform.rotation.eulerAngles.z;
        RaycastHit2D cast = Physics2D.BoxCast(transform.position, size, angleTankRotation, transform.position + transform.up,distance,layerMask.value);
        if (cast)
        {

            enemy.transform.Rotate(0, 0,  UnityEngine.Random.Range(0, 4) * 90);

        }  
        
    }
    
    
}
