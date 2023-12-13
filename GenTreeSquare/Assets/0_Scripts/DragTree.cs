using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTree : MonoBehaviour
{
    private bool arrastrando = false;
    private Vector3 posicionInicial;
    private Vector3 offset;

    void Update()
    {
        if (arrastrando)
        {
            Vector3 posicionMouse = Input.mousePosition;
            Vector3 nuevaPosicion = posicionMouse + offset;

            // Limita el movimiento según tus necesidades (puedes ajustar los valores)
            nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, 0f, Screen.width);
            nuevaPosicion.y = Mathf.Clamp(nuevaPosicion.y, 0f, Screen.height);

            transform.position = nuevaPosicion;
        }
    }

    public void EmpezarArrastre()
    {
        if (Input.GetMouseButton(0))
        {
            arrastrando = true;
            posicionInicial = transform.position;
            offset = posicionInicial - Input.mousePosition;
        }
    }

    public void TerminarArrastre()
    {
        arrastrando = false;
    }
}
