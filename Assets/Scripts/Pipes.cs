using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pipes : MonoBehaviour
{
    public float speed = 5f;
    public float leftEdge;
    public string equation;
    public int correct;
    public int incorrect;
    public TextMeshPro ans1;
    public TextMeshPro ans2;

    public GameObject gap1;
    public GameObject gap2;


    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
        GenerateEquation();

        var n = Random.Range(0, 2);
        if (n == 0)
        {
            gap2.tag = "Obstacle";
            ans1.text = correct.ToString();
            ans2.text = incorrect.ToString();
        }
        else if (n == 1)
        {
            gap1.tag = "Obstacle";

            ans1.text = incorrect.ToString();
            ans2.text = correct.ToString();
        }


        GameManager manager = FindObjectOfType<GameManager>();
        manager.addEquation(equation);
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }

    public void GenerateEquation()
    {
        string[] signs = { "+", "-", "/", "x" };
        int a = Random.Range(1, 20);
        int b = Random.Range(1, 20);
        string sign = signs[Random.Range(0, 3)];

        int result = 0;
        int notresult = 0;
        if (sign == "+") { result = a + b; notresult = a * b; }
        else if (sign == "-")
        {
            if (a < b) { int temp = a; a = b; b = temp; }
            result = a - b;
            notresult = a + b;
        }
        else if (sign == "/")
        {
            result = Random.Range(1, 5);
            a = b * result; notresult = a - b;
        }
        else if (sign == "x") { result = a * b; notresult = a + b; }

        correct = result;
        incorrect = notresult;
        equation = a + " " + sign + " " + b;
    }

}
