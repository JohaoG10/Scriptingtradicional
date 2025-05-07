using UnityEngine;

public class Recolector : MonoBehaviour
{
    public int contador = 0;

    public void RecolectarObjeto()
    {
        contador++;
        Debug.Log("Objeto recolectado. Total: " + contador);
    }
}

