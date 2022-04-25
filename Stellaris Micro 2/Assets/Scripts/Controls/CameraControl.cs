using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class CameraControl : MonoBehaviour
{
    public GameObject btnsActionSSol;
    public float sensibliteDeplacement = 0.5f;

    //public Camera cameraExt;
    // Minfov = 15 si mode galaxie  - si mode ssol
    public float minFov = 5f;
    public float maxFov = 15f;
    float scrollSensitivity = 30f;

    public float posXBtnsCaches = -400;
    public float posYBtnsCaches = 1200;

    public float speed = 160f;
    //speed de l'avance/recule quand on zoom/dezoom
    public float orthographicSize = 1f;

    [SerializeField]
    [Tooltip("Sensitivity of mouse rotation")]
    private float _mouseSense = 0.1f;

    void Start()
    {
        orthographicSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        /*     
             // curseur follow
             // Pitch
             transform.rotation *= Quaternion.AngleAxis(
                 -Input.GetAxis("Mouse Y") * _mouseSense,
                 Vector3.right
             );
             // Paw
             transform.rotation = Quaternion.Euler(
                 transform.eulerAngles.x,
                 transform.eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
                 transform.eulerAngles.z
             );
        */

        // gauche droite
        if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.RightArrow))
        {
            // on fixe les limites a droite
            if (transform.position.z < -9)
            {
                transform.Translate(new Vector3(0, 0, sensibliteDeplacement * Time.deltaTime));
                GameStatic.clickDeplacement = false;
                btnsActionSSol.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);
            }

        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftArrow))
        {
            // on fixe les limites a gauche
            if (transform.position.z > -20)
            {
                transform.Translate(new Vector3(0, 0, -sensibliteDeplacement * Time.deltaTime));
                GameStatic.clickDeplacement = false;
                btnsActionSSol.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);
            }

        }
        // avant arriere
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.x < 5)
            {
                transform.Translate(new Vector3(sensibliteDeplacement * Time.deltaTime, 0, 0));
                GameStatic.clickDeplacement = false;
                btnsActionSSol.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);
            }

        }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.x > -2)
            {

                transform.Translate(new Vector3(-sensibliteDeplacement * Time.deltaTime, 0, 0));
                GameStatic.clickDeplacement = false;
                btnsActionSSol.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);
            }
        }



        /*
        // Haut
        if (Input.GetKey(KeyCode.T))
        {
            orthographicSize++;
            Camera.main.orthographicSize = orthographicSize;
        }
        // Bas
        if (Input.GetKey(KeyCode.G))
        {
            orthographicSize--;
            Camera.main.orthographicSize = orthographicSize;
        }
        */

        // zoom mouseWheel
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        orthographicSize = fov;
        Camera.main.fieldOfView = fov;

        /*
                // Rotation
                if (Input.GetKey(KeyCode.Z))
                {
                    transform.Rotate(0.0f, speed * Time.deltaTime, 0.0f, Space.World);
                }
                if (Input.GetKey(KeyCode.R))
                {
                    transform.Rotate(0, -speed * Time.deltaTime, 0.0f, Space.World);
                }
        */
        /*
                if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // haut
                {
                    transform.Translate(new Vector3(0,speed * Time.deltaTime,0));

                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // bas
                {
                    transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));

                }*/
        /*if(Input.GetKey(KeyCode.Escape))
        {
            transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
        }*/
    }
}
