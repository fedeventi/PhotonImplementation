using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject character;   //personaje para que la camara siga
    public Transform cameraPrediction; //ubicacion desde donde la camara va a estar mirando al personaje, la camara sigue este objeto 
    [Range(0.5f,5)]
    public float speed;    //velocidad con la que la camara sigue al personaje
    [Range(0.5f, 5)]
    public float followMouseSpeed; //velocidad con la que la camara sigue el mouse
    [Range(3, 10)]
    public float radius;  // distancia a la que se puede alejar la camara
    Vector3 offset;  // desplazamiento de la posicion de la camara con respecto al personaje
    bool _offsetChanged;
    public bool teleportCamera; //indica si la camara aparece directamente en la posicion de cameraPrediction

    bool ac = false;
    public CameraController SetValues()
    {
        ac = true;
        if(character != null)
        {
            offset = cameraPrediction.position - character.transform.position;
            _offsetChanged = true;
        }
        if (teleportCamera) transform.position = cameraPrediction.position;
        return this;
    }
    private void Start()
    {
        if (character != null)
        {
            offset = cameraPrediction.position - character.transform.position;
            _offsetChanged = true;
        }
        ac = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (character != null && ac)
        {
            if (!_offsetChanged)
            {
                offset = cameraPrediction.position - character.transform.position;
                _offsetChanged = true;
            }
            CameraFutureLocation(); //modifica la posicion donde luego sera direccionada la camara
            CameraControl();
            //Tcamera();
        }


    }
    
     public static Vector3 CheckMousePosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 20;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        return mouse;
    }
   
    public void CameraControl()
    {
        float distance = Vector3.Distance(transform.position, cameraPrediction.position);
        float vDistance = Mathf.Abs(transform.position.z - cameraPrediction.position.z);
        
        Vector3 followCharacter = (cameraPrediction.position - transform.position) * speed * Mathf.Clamp(distance, 0.5f, 1) * Time.deltaTime;
        var mouse = CheckMousePosition();
        Vector3 followMouse = Vector3.ClampMagnitude((new Vector3(mouse.x, transform.position.y, mouse.z) - transform.position),radius) * followMouseSpeed * Time.deltaTime;

        transform.position += followCharacter+followMouse;

    }
    public void CameraFutureLocation()
    {
        cameraPrediction.position = character.transform.position + offset;
    }

    public Vector3 offset2;

    public void Tcamera()
    {
        transform.position = character.transform.position + offset2;
    }
   



}

