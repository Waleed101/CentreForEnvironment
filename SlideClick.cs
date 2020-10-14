using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.EventSystems;

public class SlideClick : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public bool changeOnHover = true, left = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {}

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (left)
            gameObject.GetComponentInParent<FinalManager>().NextPage();
        else
            gameObject.GetComponentInParent<FinalManager>().PreviousPage();
    }
}
