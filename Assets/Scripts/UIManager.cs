using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]TMP_Text fps;

    private void Update()
    {
        fps.text = string.Format("FPS: \n {0}",Mathf.Round(1f/Time.deltaTime));
    }
}
