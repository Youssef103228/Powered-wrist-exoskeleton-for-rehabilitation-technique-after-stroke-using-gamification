using JetBrains.Annotations;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ControleurJoueur : MonoBehaviour
{

    public float vitesse;
    public Text countText;
    public Text winText;
    private Rigidbody rb;
    private int count;
    public float speed = 5.0f;

    SerialPort stream = new SerialPort("COM4", 115200);
    public string strReceived;

    public string[] strData = new string[4];
    public string[] strData_received = new string[4];
    public float qw, qx, qy, qz;
    // Start is called before the first frame update
    void Start()
    {
      stream.Open(); //Open the Serial Stream.
      rb = GetComponent<Rigidbody>();  
      count = 0;
      SetCountText ();
      winText.text = "";

    }
    void Update()
    {
        strReceived = stream.ReadLine();

        strData = strReceived.Split(',');
        if (strData.Length >= 4 && strData[0] != "" && strData[1] != "" && strData[2] != "" && strData[3] != "")
        {
            strData_received[0] = strData[0];
            strData_received[1] = strData[1];
            strData_received[2] = strData[2];
            strData_received[3] = strData[3];

            qw = float.Parse(strData_received[0]);
            qx = float.Parse(strData_received[1]);
            qy = float.Parse(strData_received[2]);
            qz = float.Parse(strData_received[3]);

            transform.rotation = new Quaternion(-qy, -qz, qx, qw);
            if (transform.position.z < 8)
            {
                // Move forward only if the condition is met
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                rb.AddForce(Vector3.forward * vitesse);
            }

        }
    }

    // Update is called once per frame

    public void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag ("Cible")) {
              other.gameObject.SetActive (false) ;
              count = count + 1;
              SetCountText ();
        }
    }
    
    void SetCountText (){
        countText.text = "Count: " + count.ToString ();
        if (count >= 46)
        {
            winText.text = "You Win!";
        }
    }


}
