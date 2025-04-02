using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(target.position.x, 0, -10f);
        Camera.main.transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
    }
}
