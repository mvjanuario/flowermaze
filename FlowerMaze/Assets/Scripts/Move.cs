using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    [SerializeField] private GameObject m_module;
    private Vector3 m_originPosition, m_targetPosition, m_direction;

    private float timeToMove = 0.5f;
    private float speed = 10;
    private bool isMoving = false;
    private Vector3 m_lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveEdge();
    }

    public void MoveEdge()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && !isMoving && m_lastDirection != Vector3.down)
        {
            m_direction = Vector3.up;
            m_lastDirection = m_direction;
            StartCoroutine(Movement(Vector3.up));
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && !isMoving && m_lastDirection != Vector3.up)
        {
            m_direction = Vector3.down;
            m_lastDirection = m_direction;
            StartCoroutine(Movement(Vector3.down));
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving && m_lastDirection != Vector3.right)
        {
            m_direction = Vector3.left;
            m_lastDirection = m_direction;
            StartCoroutine(Movement(Vector3.left));
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && !isMoving && m_lastDirection != Vector3.left)
        {
            m_direction = Vector3.right;
            m_lastDirection = m_direction;
            StartCoroutine(Movement(Vector3.right));
        }
    }

    public void createModule()
    {
        Instantiate(m_module, transform.position, transform.rotation);
    }

    private IEnumerator Movement(Vector3 direction)
    {
        isMoving = true;

        //float elapsedTime = 0;

        m_originPosition = transform.position;
        for (int i = 0; i < 1; i++)
        {
            m_targetPosition = m_originPosition + direction;
            createModule();
            transform.position = m_targetPosition;
            //transform.position = Vector2.MoveTowards(transform.position, m_targetPosition, speed * Time.deltaTime);
            m_originPosition = transform.position;
        }
        yield return null;
        /*
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(m_originPosition, m_targetPosition, (elapsedTime / timeToMove));
            createModule();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        */

        transform.position = m_targetPosition;

        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("danger"))
        {
            Debug.Log("perigo");
        }
        if (collision.CompareTag("final"))
        {
            Debug.Log("parabéns");
        }
    }
}
