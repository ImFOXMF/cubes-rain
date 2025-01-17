using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        GameObject otherObject = other.gameObject;
        Cube cube = otherObject.GetComponent<Cube>();
        
        if (cube != null && !cube.GetIsChangedColor())
        {
            Renderer otherRenderer = otherObject.GetComponent<Renderer>();
            SetRandomColor(otherRenderer);
            cube.ChangeColor();
        }
    }

    private void SetRandomColor(Renderer objectRenderer)
    {
        objectRenderer.material.color = Random.ColorHSV();
    }
}
