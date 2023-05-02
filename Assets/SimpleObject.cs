using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{
    [SerializeField] List<Color> posibleColors = new List<Color>();
    [SerializeField] SpriteRenderer renderer;

    void Awake()
    {
        int index = Random.Range(0, posibleColors.Count);
        renderer.color = posibleColors[index];
    }

    public Color GetColor()
    {
        return renderer.color;
    }

    public IEnumerator MoveToTarget(Vector3 destination)
    {
        while (Vector2.Distance(transform.position, destination) > .01f)
        {
            Debug.Log("nope");
            transform.position = Vector3.Lerp(transform.position, destination, .05f);
            yield return null;
        }

    }
}

