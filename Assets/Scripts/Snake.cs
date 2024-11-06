
using System.Collections.Generic;
using UnityEngine;


public class Snake : MonoBehaviour
{
    public GameManager GameManager;
    public float Speed;
    [Range(0.5f, 3f)] public float SpeedMultiplier;

    public Transform SegmentPrefab;
    public Vector2 Direction;
    [Range(2, 10)] public int InitialSize;
    public bool CanCrossWall = true;

    private Vector2 _input;
    private Transform _transform;
    private float _nextUpdate;
    private List<Transform> _segments = new List<Transform>();
    private int _currentSize;

    private void Start()
    {
        _currentSize = InitialSize;
        _transform = GetComponent<Transform>();
        UpdateState();
        Direction = new Vector2(Mathf.RoundToInt(Random.Range(Vector2.left.x, Vector2.right.x)), Mathf.RoundToInt(Random.Range(Vector2.down.y, Vector2.up.y)));
    }
    private void Update()
    {
        if (Direction.x != 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _input = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _input = Vector2.down;
            }

        }
        else if (Direction.y != 0)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _input = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _input = Vector2.right;
            }
        }
        RotateHeadSnake();
    }

    private void FixedUpdate()
    {
        if (Time.time < _nextUpdate)
        {
            return;
        }
        if (_input != Vector2.zero)
        {
            Direction = _input;
        }
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        var newPosition = (Vector2)_transform.position + Direction;
        _transform.position = newPosition;
        _nextUpdate = Time.time + 1 / (Speed * SpeedMultiplier);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            if (CanCrossWall)
            {
                CrossWall(collision.transform);
            }
            else
            {
                GameManager.ActiveEndMenu();
            }
        }
        else if (collision.CompareTag("Food"))
        {
            // змейка кушает! 
            InitialSize++;
            Grow();
        }
        else if (collision.CompareTag("segment"))
        {
            GameManager.ActiveEndMenu();
        }
    }
    private void UpdateState()
    {
        InitialSize = _currentSize;
        Direction = new Vector2(Mathf.RoundToInt(Random.Range(Vector2.left.x, Vector2.right.x)), Mathf.RoundToInt(Random.Range(Vector2.down.y, Vector2.up.y)));
        _transform.position = Vector3.zero;
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(transform);
        for (int i = 1; i < InitialSize; i++)
        {
            Grow();
        }

    }
    private void Grow()
    {
        Transform segment = Instantiate(SegmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }
    public bool Occupies(int x, int y)
    {
        Vector2 position = new Vector2(x, y);
        foreach (Transform segment in _segments)
        {
            if ((Vector2)segment.position == position)
            {
                return true;
            }
        }
        return false;
    }
    private void CrossWall(Transform wall)
    {
        Vector2 newPosition = _transform.position;

        if (Direction.x != 0f)
        {
            newPosition.x = -wall.position.x + Direction.x;
        }
        else if (Direction.y != 0f)
        {
            newPosition.y = -wall.position.y + Direction.y;
        }
        _transform.position = newPosition;
    }

    private void RotateHeadSnake()
    {
        if (Direction == Vector2.up)
        {
            _transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (Direction == Vector2.down)
        {
            _transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (Direction == Vector2.left)
        {
            _transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (Direction == Vector2.right)
        {
            _transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }

}