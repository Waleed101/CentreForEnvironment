using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.EventSystems;


public class SelectRouteButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject gameControl;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        gameControl.GetComponent<SelectRoute>().SetPickedRoute(this.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<DisplayRoutes>().GetCurrentPage() - 1);
        gameControl.GetComponent<ManageScene>().SetActiveComponent(gameControl.GetComponent<ManageScene>().GetActiveComponent() + 1);
    }
}
