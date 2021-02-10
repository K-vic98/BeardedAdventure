using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _followObject;
    [SerializeField] private Vector2 _followOffset;
    [SerializeField] private float _speed = 3f;

    private Vector2 _threshold;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _threshold = CalculateThreshold();
        _rigidbody = _followObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 follow = _followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= _threshold.x)
            newPosition.x = follow.x;
        if (Mathf.Abs(xDifference) >= _threshold.y)
            newPosition.y = follow.y;
        float moveSpeed = _rigidbody.velocity.magnitude > _speed ? _rigidbody.velocity.magnitude : _speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    private Vector3 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= _followOffset.x;
        t.y -= _followOffset.y;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}