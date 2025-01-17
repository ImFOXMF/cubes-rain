using UnityEngine;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        GameObject otherObject = other.gameObject;
        
        if (otherObject.GetComponent<Cube>().GetIsChangedColor() == false)
        {
            Renderer otherRenderer = otherObject.GetComponent<Renderer>();
            SetRandomColor(otherRenderer);
            otherObject.GetComponent<Cube>().ChangeColor();
        }
    }

    private void SetRandomColor(Renderer objectRenderer)
    {
        objectRenderer.material.color = Random.ColorHSV();
    }
}