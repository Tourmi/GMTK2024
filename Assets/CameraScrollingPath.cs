using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class CameraScrollingPath : MonoBehaviour
{
    private Camera _camera;
    private float leftedge;
    private float rightedge;
    private Vector3 basePos;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        leftedge = _camera.transform.localPosition.x;
        rightedge = _camera.transform.localPosition.x + 16;
        basePos = _camera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _camera.transform.localPosition;

        pos.x += Input.mouseScrollDelta.y;
        pos.z += Input.mouseScrollDelta.y;

        if (pos.x < rightedge & pos.x > leftedge)
        {
            _camera.transform.localPosition = pos;
        }
        else if(pos.x < leftedge)
        {
            _camera.transform.localPosition = basePos;
        }
        else if(pos.x > rightedge)
        {
            _camera.transform.localPosition = basePos + new Vector3(16, 0, 16);
        }
        
    }

}
