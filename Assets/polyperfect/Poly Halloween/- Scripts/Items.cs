using UnityEngine;
using TMPro;
using System.Collections;

public class Items : MonoBehaviour
{
    private int puntos = 0;
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoLlave;
    public GameObject Puerta1;
    public GameObject Puerta2;
    public GameObject CameraPlayer;
    public GameObject CameraDoor;
    public float tiempoCambio = 3f;
    public float velocidadRotacion = 2f;
    private bool llaveObtenida = false;

    void Start()
    {
        ActualizarTexto();
        CameraDoor.SetActive(false);

        if (textoLlave != null)
        {
            textoLlave.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Calabaza"))
        {
            puntos++;
            Destroy(other.gameObject);
            ActualizarTexto();
            Debug.Log("Puntos: " + puntos);
        }

        if (other.CompareTag("Key1") && !llaveObtenida)
        {
            llaveObtenida = true;
            Destroy(other.gameObject);
            MostrarMensajeLlave();

            if (Puerta1 != null && Puerta2 != null)
            {
                // Iniciar la corrutina para el cambio de cámaras
                StartCoroutine(SecuenciaCamaraYPuertas());
            }
        }
    }

    IEnumerator SecuenciaCamaraYPuertas()
    {
        // 1. Cambiar a la cámara de la puerta
        CameraPlayer.SetActive(false);
        CameraDoor.SetActive(true);

        // 2. Rotar las puertas gradualmente
        Quaternion rotacionInicial1 = Puerta1.transform.rotation;
        Quaternion rotacionInicial2 = Puerta2.transform.rotation;
        Quaternion rotacionFinal = Quaternion.Euler(0, 180, 0);

        float tiempoTranscurrido = 0f;
        float duracionRotacion = 2f; // Duración de la animación de las puertas

        while (tiempoTranscurrido < duracionRotacion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duracionRotacion;

            Puerta1.transform.rotation = Quaternion.Lerp(rotacionInicial1, rotacionFinal, t);
            Puerta2.transform.rotation = Quaternion.Lerp(rotacionInicial2, rotacionFinal, t);

            yield return null; // Esperar al siguiente frame
        }

        // Asegurar que lleguen a la rotación final
        Puerta1.transform.rotation = rotacionFinal;
        Puerta2.transform.rotation = rotacionFinal;

        // 3. Esperar el tiempo adicional antes de volver a la cámara del jugador
        yield return new WaitForSeconds(tiempoCambio - duracionRotacion);

        // 4. Volver a la cámara del jugador
        CameraPlayer.SetActive(true);
        CameraDoor.SetActive(false);
    }

    void ActualizarTexto()
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = "Calabazas: " + puntos;
        }
    }

    void MostrarMensajeLlave()
    {
        if (textoLlave != null)
        {
            textoLlave.gameObject.SetActive(true);
            textoLlave.text = "Llave Obtenida";
            Invoke("OcultarMensajeLlave", 3f);
        }
    }

    void OcultarMensajeLlave()
    {
        if (textoLlave != null)
        {
            textoLlave.gameObject.SetActive(false);
        }
    }
}