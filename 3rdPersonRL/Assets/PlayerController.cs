using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 15;
    CharacterController myCC;
    // Use this for initialization
    void Start()
    {
        myCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");


        myCC.Move(transform.forward * speed * vAxis * Time.deltaTime);
        myCC.Move(transform.right * speed * hAxis * Time.deltaTime);

    }
}
