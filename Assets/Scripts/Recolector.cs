using UnityEngine;

public class Recolector : MonoBehaviour
{
    public int objetosRecolectados = 0;

    public void RecolectarObjeto()
    {
        objetosRecolectados++;
        Debug.Log("Objeto recolectado. Total: " + objetosRecolectados);
    }
}
}
}
