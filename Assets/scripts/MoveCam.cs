using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCam : MonoBehaviour
{

    int cameraMoveSpeed = 500;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveThisCamera();
    }

    void moveThisCamera()
    {
        //Debug.Log(Input.mousePosition.x);
        //Debug.Log(Input.mousePosition.y);
        //Debug.Log(Input.mousePosition.z);

        if (Input.mousePosition.x <= 10 && transform.position.x > -200)
        {
            float xNew = this.transform.position.x;
            xNew -= cameraMoveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(xNew, this.transform.position.y, this.transform.position.z);
        }
        if (Input.mousePosition.x >= (Screen.width - 10) && transform.position.x < 800)
        {
            float xNew = this.transform.position.x;
            xNew += cameraMoveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(xNew, this.transform.position.y, this.transform.position.z);
        }

        if (Input.mousePosition.y <= 10 && transform.position.y > -300)
        {
            float yNew = this.transform.position.y;
            yNew -= cameraMoveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(this.transform.position.x, yNew, this.transform.position.z);
        }
        if (Input.mousePosition.y >= (Screen.height - 10) && transform.position.y < 800)
        {
            float yNew = this.transform.position.y;
            yNew += cameraMoveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(this.transform.position.x, yNew, this.transform.position.z);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.z < 0)
        {
            float zNew = this.transform.position.z;
            zNew += cameraMoveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, zNew);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.z > -900)
        {
            float zNew = this.transform.position.z;
            zNew -= cameraMoveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, zNew);
        }
    }

    public void onBackButton()
    {
        SceneManager.LoadScene("ProjectScene");
    }
}
