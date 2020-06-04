using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToObject : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;

    private float speedrotate = 4f;

    private GameObject spwanedObject;
    private ARRaycastManager _arRaycastManager;

    private Vector3 _rotation;
    private Vector2 touchPosition;

    private bool isLocked = false;
    private bool onTouchHold = false;

    public Button lockButton;
    public Sprite lockSprite;
    public Sprite unlockSprite;
    public Slider slider;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    //Allowed Touch Position Function
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;

    }

    //Scale Function
    public void ScaleChanged(float newValue)
    {
        spwanedObject.transform.localScale = new Vector3(newValue, newValue, newValue);
    }

    //Lock Button Function
    private void Lock()
    {
        isLocked = !isLocked;

        lockButton.GetComponent<Image>().sprite = isLocked ? lockSprite : unlockSprite;

        //Debug.Log(lockButton.GetComponentInChildren<Text>().text + "After call");
    }

    bool IsPointOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        PointerEventData eventPosition = new PointerEventData(EventSystem.current);
        eventPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);

        return results.Count > 0;

    }

    void Update()
    {
        //Debug.Log(Input.mousePosition);
        //Debug.Log(slider.value);
        /*if (spwanedObject != null)
		{
			slider.onValueChanged.AddListener(ScaleChanged);
			lockButton.onClick.AddListener(Lock);
		}*/

        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        bool isOverUI = IsPointOverUIObject(touchPosition);


        if (isOverUI && spwanedObject != null)
        {
            lockButton.onClick.AddListener(Lock);
        }

        else if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (spwanedObject == null)
            {
                spwanedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                slider.onValueChanged.AddListener(ScaleChanged);
            }
            else
            {
                onTouchHold = isLocked ? false : true;

                if (onTouchHold)
                {

                    spwanedObject = null;
                    spwanedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);

                    spwanedObject.transform.localScale = Vector3.one;
                    slider.value = 1;

                    // spwanedObject.transform.position = hitPose.position;

                }
                slider.onValueChanged.AddListener(ScaleChanged);

                // Rotation
                var dif = hitPose.position - spwanedObject.transform.position;

                _rotation = new Vector3(-dif.y * speedrotate, -dif.x * speedrotate, dif.z * speedrotate);

                spwanedObject.transform.Rotate(_rotation);
                //
            }
        }
    }
}
