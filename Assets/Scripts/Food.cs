using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D BoxColider2D;
    private Transform _transform;
    private Snake _snake;


    private void Start()
    {
        _transform = GetComponent<Transform>();
        _snake = FindObjectOfType<Snake>();

        RandomizePosition();
    }
    public void RandomizePosition()
    {
        Bounds bounds = BoxColider2D.bounds;
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
        while (_snake.Occupies(x, y))
        {
            x++;
            if (x > bounds.max.x)
            {
                x = Mathf.RoundToInt(bounds.min.x);
                y++;
                if (y > bounds.max.y)
                {
                    y = Mathf.RoundToInt(bounds.min.y);
                }
            }
        }
        _transform.position = new Vector2(x, y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RandomizePosition();
    }
}
