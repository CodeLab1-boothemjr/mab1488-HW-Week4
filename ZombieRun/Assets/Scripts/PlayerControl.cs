using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 2.0f;

    private void Awake()
    {
        transform.position = GameObject.Find("StartPos").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step =  speed * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * step;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * step;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * step;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * step;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EndPos")
        {
            //TODO why does this have to be static?
            GameManager.AdvanceCurrentLevel();
        }
    }
}
