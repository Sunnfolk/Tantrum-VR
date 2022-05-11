using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    //This script deals with the Radial Selection menu
    //Most of this script is manipulated by the MenuInputManager script if something is unclear or not commented check the video linked in there

    [Header("Scene")]
    public Transform selectionTransform = null;
    public Transform cursorTransform = null;

    [Header("Events")]
    public RadialSection top = null;
    public RadialSection right = null;
    public RadialSection bottom = null;
    public RadialSection left = null;

    private Vector2 _touchPosition = Vector2.zero;
    private List<RadialSection> _radialSections = null;
    private RadialSection _highlightedSection = null;

    private readonly float _degreeIncrement = 90.0f;

    private void Awake()
    {
        CreateAndSetupSections();
    }

    private void CreateAndSetupSections()
    {
        _radialSections = new List<RadialSection>()
        {
            top,
            right,
            bottom,
            left
        };

        foreach (RadialSection section in _radialSections)
            section.iconRenderer.sprite = section.icon;
    }

    private void Start()
    {
        Show(false);
    }

    public void Show(bool value) //Activates the object which contains the menu
    {
        gameObject.SetActive(value);
    }

    private void Update() //Keeps the Selection circle up to date on where your finger is and what it is hovering over
    {
        Vector2 direction = Vector2.zero + _touchPosition;
        float rotation = GetDegree(direction);

        SetCursorPosition();
        SetSelectionRotation(rotation);
        SetSelectedEvent(rotation);

    }

    private float GetDegree(Vector2 direction)
    {
        float value = Mathf.Atan2(direction.x, direction.y);
        value *= Mathf.Rad2Deg;

        if(value < 0)
        {
            value += 360.0f;
        }

        return value;
    }

    private void SetCursorPosition() //Sets the black dot which indicates your finger to a corresponding position on the selection wheel
    {
        cursorTransform.localPosition = _touchPosition;
    }

    private void SetSelectionRotation(float newRotation) //Sets the rotation of the object which indicates which portion of the selection wheel you are in
    {
        float snappedRotation = SnapRotation(newRotation);
        selectionTransform.localEulerAngles = new Vector3(0, 0, -snappedRotation);
    }

    private float SnapRotation(float rotation)
    {
        return GetNearestIncrement(rotation) * _degreeIncrement;
    }

    private int GetNearestIncrement(float rotation)
    {
        return Mathf.RoundToInt(rotation / _degreeIncrement);
    }

    private void SetSelectedEvent(float currentRotation)
    {
        int index = GetNearestIncrement(currentRotation);

        if (index == 4)
            index = 0;

        _highlightedSection = _radialSections[index];
    }

    public void SetTouchPosition(Vector2 newValue)
    {
        _touchPosition = newValue;
    }

    public void ActivateHighlightedSection()
    {
        if (_highlightedSection != null)
        {
            _highlightedSection.onPress.Invoke();
        }
    }
}
