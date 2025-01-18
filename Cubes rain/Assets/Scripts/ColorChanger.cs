using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        GameObject cubeObject = gameObject;
        Cube cube = cubeObject.GetComponent<Cube>();
        
        if (cube != null && !cube.GetIsChangedColor())
        {
            Renderer otherRenderer = cubeObject.GetComponent<Renderer>();
            SetRandomColor(otherRenderer);
            cube.ChangeColor();
        }
    }

    private void SetRandomColor(Renderer objectRenderer)
    {
        objectRenderer.material.color = Random.ColorHSV();
    }
}
