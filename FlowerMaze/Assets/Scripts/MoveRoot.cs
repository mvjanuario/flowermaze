using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveRoot : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public Transform rootSprite;

    public LayerMask whatStopsMoviment;

    public bool up, down, left, right;
    private bool mudou = false;

    [SerializeField] private GameObject m_LModuleLeftUp;
    [SerializeField] private GameObject m_LModuleLeftDown;
    [SerializeField] private GameObject m_LModuleRightUp;
    [SerializeField] private GameObject m_LModuleRightDown;

    [SerializeField] private GameObject m_moduleVertical;
    [SerializeField] private GameObject m_moduleHorizontal;

    private Vector3 m_lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;   
        up = false;
        down = true;
        left = false;
        right = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        //if(transform.position == movePoint.position)

        if (Vector3.Distance(transform.position, movePoint.position) <= 0)
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && m_lastDirection != Vector3.down)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, 1, 0), 0.2f, whatStopsMoviment))
                {
                    up = true;
                    createModuleVertical();
                    m_lastDirection = Vector3.up;
                    movePoint.position += new Vector3(0, 1, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 180);
                }
            }
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && m_lastDirection != Vector3.up)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, -1, 0), 0.2f, whatStopsMoviment))
                {
                    down = true;
                    createModuleVertical();
                    m_lastDirection = Vector3.down;
                    movePoint.position += new Vector3(0, -1, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 0);
                }
            }
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && m_lastDirection != Vector3.right)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1, 0, 0), 0.2f, whatStopsMoviment))
                {
                    left = true;
                    createModuleHorizontal();
                    m_lastDirection = Vector3.left;
                    movePoint.position += new Vector3(-1, 0, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, -90);
                }
            }
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && m_lastDirection != Vector3.left)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(1, 0, 0), 0.2f, whatStopsMoviment))
                {
                    right = true;
                    createModuleHorizontal();
                    m_lastDirection = Vector3.right;
                    movePoint.position += new Vector3(1, 0, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 90);
                }
            }
        }
    }

    public void createModuleVertical()
    {
        if (up && left)
        {
            Instantiate(m_LModuleRightUp, transform.position, transform.rotation);
            left = false;
        }
        else if (up && right)
        {
            Instantiate(m_LModuleLeftUp, transform.position, transform.rotation);
            right = false;
        }
        else if (down && left)
        {
            Instantiate(m_LModuleRightDown, transform.position, transform.rotation);
            left = false;
        }
        else if (down && right)
        {
            Instantiate(m_LModuleLeftDown, transform.position, transform.rotation);
            right = false;
        }
        else
        {
            Instantiate(m_moduleVertical, transform.position, transform.rotation);
        }
    }

    public void createModuleHorizontal()
    {
        if (up && left)
        {
            Instantiate(m_LModuleLeftDown, transform.position, transform.rotation);
            up = false;
        }
        else if (up && right)
        {
            Instantiate(m_LModuleRightDown, transform.position, transform.rotation);
            up = false;
        }
        else if (down && left)
        {
            Instantiate(m_LModuleLeftUp, transform.position, transform.rotation);
            down = false;
        }
        else if (down && right)
        {
            Instantiate(m_LModuleRightUp, transform.position, transform.rotation);
            down = false;
        }
        else
        {
            Debug.Log("Dale");
            Instantiate(m_moduleHorizontal, transform.position, m_moduleHorizontal.transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("danger"))
        {
            Debug.Log("perigo");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (collision.CompareTag("final"))
        {
            Debug.Log("parabéns");
        }
    }
}
