using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{
    [SerializeField] List<Color> posibleColors = new List<Color>();
    [SerializeField] SpriteRenderer renderer;
    MergeObjectType mergeObjectType;
    int mergeObjectLevel;
    SimpleObject overlappingObject;

    public MergeObjectType MergeObjectType { get { return mergeObjectType; } set { mergeObjectType = value; } }
    public int MergeObjectLevel { get { return mergeObjectLevel; } set { mergeObjectLevel = value; } }

    void Awake()
    {
        int index = Random.Range(0, posibleColors.Count);
        renderer.color = posibleColors[index];
    }

    public SimpleObject(MergeObjectType _type, int _level)
    {
        mergeObjectType = _type;
        mergeObjectLevel = _level;
    }

    public void CheckMatchingType()
    {
        if (overlappingObject.MergeObjectLevel == this.mergeObjectLevel && overlappingObject.MergeObjectType == this.mergeObjectType)
        {
            overlappingObject.UpdateLevel();
            Destroy(gameObject);//TODO make this use object pooling
        }
    }

    public void UpdateLevel()
    {
        mergeObjectLevel++;
    }

    public Color GetColor()
    {
        return renderer.color;
    }

    public IEnumerator MoveToTarget(Vector3 destination)
    {
        while (Vector2.Distance(transform.position, destination) > .01f)
        {
            transform.position = Vector3.Lerp(transform.position, destination, .05f);
            yield return null;
        }

    }
}

public enum MergeObjectType
{
    Fruit,
    Vegie
}

