using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteFruits : MonoBehaviour
{
    public List<Sprite> SpriteFruits;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        RandomSprite();
    }
    private void RandomSprite()
    {
        _spriteRenderer.sprite = SpriteFruits[Random.Range(0, SpriteFruits.Count)];  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RandomSprite();
    }
}


