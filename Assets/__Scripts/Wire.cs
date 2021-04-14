using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour,
         IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool IsLeftWire;
    public Color CustomColor;

    private Image image;
    private LineRenderer lineRenderer;

    private Canvas canvas;
    private bool dragStarted = false;
    private WireGame wireGame;
    public bool isSuccess = false;

    //Initialize the image, line renderer, cancas, and wireGame object
    private void Awake()
    {
        image = GetComponent<Image>();
        lineRenderer = GetComponent<LineRenderer>();
        canvas = GetComponentInParent<Canvas>();
        wireGame = GetComponentInParent<WireGame>();
    }

    private void Update()
    {
        //If dragging has begun
        if (dragStarted)
        {
            //Display the movement of the wire according to the mouse position
            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform,
                        Input.mousePosition,
                        canvas.worldCamera,
                        out movePos);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1,
                 canvas.transform.TransformPoint(movePos));
        }
        else
        {
            //Hide the line if not dragging.
            //If the line is not connected reset it 
            if (!isSuccess)
            {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
        }
        bool isHovered =
          RectTransformUtility.RectangleContainsScreenPoint(
              transform as RectTransform, Input.mousePosition,
                                      canvas.worldCamera);
        if (isHovered)
        {
            wireGame.CurrentHoveredWire = this;
        }
    }

    //Set the colour of the wire
    public void SetColor(Color color)
    {
        image.color = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        CustomColor = color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // needed for drag but not used
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Cannot drag the right wire
        if (!IsLeftWire)
        {
            return;
        }
        // Is is successful, don't draw more lines
        if (isSuccess)
        {
            return;
        }
        dragStarted = true;
        wireGame.CurrentDraggedWire = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (wireGame.CurrentHoveredWire != null)
        {
            //If the destination point is the same colour as the dragged point
            if (wireGame.CurrentHoveredWire.CustomColor ==
                                                   CustomColor &&
                !wireGame.CurrentHoveredWire.IsLeftWire)
            {
                isSuccess = true;

                // Set Successful on the right wire too
                wireGame.CurrentHoveredWire.isSuccess = true;
            }
        }
        dragStarted = false;
        wireGame.CurrentDraggedWire = null;
    }
}