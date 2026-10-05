using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewVisionDetector : MonoBehaviour
{
    EnemyMove enemy;

    [SerializeField] float distance;
    [SerializeField] LayerMask layerMask;

    BoxCollider2D boxCollider;
    Vector2[] _raycastPosition = new Vector2[3];
    Rigidbody2D _rb;
    RaycastHit2D[] _raycastHits = new RaycastHit2D[3];
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyMove>();
        boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
    }
    private void CheckGround()
    {
        Vector2 _raycastOrigin = _rb.position + new Vector2(0,boxCollider.size.y * 10f);
        _raycastPosition[0] = _raycastOrigin + (Vector2.left * boxCollider.size.x * 10f + Vector2.down * 0.1f);
        _raycastPosition[1] = _raycastOrigin;
        _raycastPosition[2] = _raycastOrigin + (Vector2.right * boxCollider.size.x * 10f + Vector2.down * 0.1f);

        int numberOfRays = 0;

        for (int i = 0; i < _raycastPosition.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_raycastPosition[i], Vector2.down, distance, layerMask);
            if (hit)
            {
                _raycastHits[i] = hit;
                numberOfRays++;
                enemy.transform.Rotate(0, 0, UnityEngine.Random.Range(1, 4) * 90);
            }
        }
    }


    private void OnDrawGizmos()
    {
        
        for (int i = 0; i < _raycastPosition.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_raycastPosition[i], Vector2.down * 35f);
        }
    }
}
