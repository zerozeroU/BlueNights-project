using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class BackButton : MonoBehaviour
    , IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler

{
    private bool _pointerIn;
    private GameObject _parentObject;
    private void Awake()
    {
        GetParentObject();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector2(1.1f, 1.1f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = new Vector2(1, 1);
        GotoScene();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerIn = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerIn = false;
    }
    private void GotoScene()
    {
        if (_pointerIn)
        {
            switch (_parentObject.name)
            {
                case "SelectWindow":
                    _parentObject.SetActive(false);
                    break;
                case "Canvas":
                    SceneManager.LoadScene("TitleScene");
                    break;
                default:
                    Debug.Log("_parentObject Null");
                    break;
            }
        }
    }
    private void GetParentObject()
    {
        if (transform.parent.name == "SelectWindow")
        {
            _parentObject = GameObject.Find("SelectWindow");
        }
        else if (transform.parent.name == "Canvas")
        {
            _parentObject = GameObject.Find("Canvas");
        }

        if (_parentObject != null)
        {
            Debug.Log("_parentObject : " + _parentObject);
        }
        else
        {
            Debug.Log("_parentObject Null");
        }
    }
}
