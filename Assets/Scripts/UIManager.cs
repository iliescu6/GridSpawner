using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text fps;
    [SerializeField] Camera camera;
    [SerializeField] Grid grid;
    [SerializeField] Slider slider;
    [SerializeField] Button spawnButton;
    [SerializeField] Button clearButton;
    [SerializeField] EventTrigger spawnTrigger;
    Vector3 distance;
    Vector3 origin;
    bool drag;
    bool overUI;
    float maxZoomOut;
    float currentZoom;

    private void Start()
    {
        maxZoomOut = grid.GridSizeX < grid.GridSizeY ? (float)grid.GridSizeY / 2 : (float)grid.GridSizeX / 2;
        slider.onValueChanged.AddListener(delegate { CameraZoom(); });

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { grid.Spawner.Spawn(); });

        spawnTrigger.triggers.Add(entry);

        //spawnButton.onClick.AddListener(grid.Spawner.Spawn);
        clearButton.onClick.AddListener(grid.Spawner.Clear);
    }

    private void Update()
    {
        fps.text = string.Format("FPS: \n {0}", Mathf.Round(1f / Time.deltaTime));
    }

    private void LateUpdate()
    {
        DragCamera();
    }

    void CameraZoom()
    {
        currentZoom = slider.value * maxZoomOut;
        currentZoom = Mathf.Clamp(currentZoom, 1, maxZoomOut);
        Camera.main.orthographicSize = currentZoom;
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer.ToString() == "UI")
                Debug.Log("xx");
            return true;
        }
        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    //Drags camera if not over ui element or dragging spawner
    void DragCamera()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.tag == "Spawner")
            {
                return;
            }
            else if (IsPointerOverUIElement())
            {
                overUI = true;
                return;
            }

            if (!overUI)
            {
                distance = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;

                if (drag == false)
                {
                    drag = transform;
                    origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }
        else
        {
            drag = false;
            overUI = false;
        }

        if (drag)
        {
            Camera.main.transform.position = origin - distance;
        }
    }
}
