using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFade : MonoBehaviour
{
    LineRenderer lr;

    [SerializeField] private Color color;

    [SerializeField] private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);

        lr.startColor = color;
        lr.endColor = color;
    }
}
