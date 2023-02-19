using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveRoot : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform movePoint;
    public Transform rootSprite;
    public BoxCollider2D collider;

    public LayerMask whatStopsMovement;

    private bool _up, _down, _left, _right;

    public GameObject moduleLeftUp;
    public GameObject moduleLeftUpDead;
    public GameObject moduleLeftDown;
    public GameObject moduleLeftDownDead;
    public GameObject moduleRightUp;
    public GameObject moduleRightUpDead;
    public GameObject moduleRightDown;
    public GameObject moduleRightDownDead;
    public GameObject moduleVertical;
    public GameObject moduleVerticalDead;
    public GameObject moduleHorizontal;
    public GameObject moduleHorizontalDead;

    public GameObject roots;
    public GameObject sprite;

    private Vector3 _lastDirection;
    private Vector3 _direction;
    private Vector3 _rootDirection1, _rootDirection2;
    private bool _changeDirection;

    private List<GameObject> _modulesList;
    private List<GameObject> _modulesListAntigo;
    private List<int> _directionsList;
    
    public AudioSource music, gameOver, rootSound;

    public bool isInGameOver = false;
    public int tentativas = 0;

    // Start is called before the first frame update
    void Start(){
        movePoint.parent = null;   
        _up = false;
        _down = true;
        _left = false;
        _right = false;
        _rootDirection1 = new Vector3(1, 1, 1);
        _rootDirection2 = new Vector3(-1, 1, 1);

        _modulesList = new List<GameObject>();
        _modulesListAntigo = new List<GameObject>();
        _directionsList = new List<int>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update(){
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0 && !isInGameOver){
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && _lastDirection != Vector3.down){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, 1, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(0, 1, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 180);
                    _lastDirection = Vector3.up;
                    _up = true;
                    RootMoviment(8);
                }
            } else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && _lastDirection != Vector3.up){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, -1, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(0, -1, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 0);
                    _lastDirection = Vector3.down;
                    _down = true;
                    RootMoviment(2);
                }
            } else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && _lastDirection != Vector3.right){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1, 0, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(-1, 0, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, -90);
                    _lastDirection = Vector3.left;
                    _left = true;
                    RootMoviment(4);
                }
            } else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && _lastDirection != Vector3.left){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1, 0, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(1, 0, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 90);
                    _lastDirection = Vector3.right;
                    _right = true;
                    RootMoviment(6);
                }
            }
        }
    }

    private void RootMoviment(int direction){
        if (!isInGameOver)
        {
            _directionsList.Add(direction);
            _modulesList.Add(CreateModule());
        }
         
         if (_modulesList.Count >= 10){
             _modulesList[^10].GetComponent<BoxCollider2D>().enabled = true;
         }
         ChangeDirection();
     }

     private void ChangeDirection(){
        rootSound.Play();
        
        if (_changeDirection){
            sprite.transform.localScale = _rootDirection1;
        } else {
            sprite.transform.localScale = _rootDirection2;
        }

        _changeDirection = !_changeDirection;
    }

    private GameObject CreateModule(){
        if (_lastDirection == Vector3.up || _lastDirection == Vector3.down){
            return CreateModuleVertical();
        } else{
            return CreateModuleHorizontal();
        }
    }

    private GameObject CreateModuleVertical(){
        GameObject module;
        if (_up && _left){
            module = InstantiateModule(moduleRightUp);
            _left = false;
        } else if (_up && _right){
            module =InstantiateModule(moduleLeftUp);
            _right = false;
        } else if (_down && _left){
            module =InstantiateModule(moduleRightDown);
            _left = false;
        } else if (_down && _right){
            module = InstantiateModule(moduleLeftDown);
            _right = false;
        } else{
            module = InstantiateModule(moduleVertical);
        }

        return module;
    }

    private GameObject CreateModuleHorizontal(){
        GameObject module;
        if (_up && _left){
            module = InstantiateModule(moduleLeftDown);
            _up = false;
        } else if (_up && _right){
            module = InstantiateModule(moduleRightDown);
            _up = false;
        } else if (_down && _left){
            module = InstantiateModule(moduleLeftUp);
            _down = false;
        } else if (_down && _right){
            module = InstantiateModule(moduleRightUp);
            _down = false;
        } else{
            module = Instantiate(moduleHorizontal, transform.position, moduleHorizontal.transform.rotation, roots.transform);
        }

        return module;
    }

    private GameObject InstantiateModule(GameObject module){
        var transformAtual = transform;
        GameObject moduleAtual;
        moduleAtual = Instantiate(module, transformAtual.position, transformAtual.rotation, roots.transform);
        return moduleAtual;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("danger") || collision.gameObject.CompareTag("modulo")){
            if (!isInGameOver)
            {
                music.Stop();
                StartCoroutine(PlayGameover());
            }
        }
        if (collision.gameObject.CompareTag("final")){
            SceneManager.LoadScene("End Game");
        }
    }

    private IEnumerator PlayGameover() {
        tentativas++;
        isInGameOver = true;
        gameOver.volume = 0.25f;
        gameOver.Play();

        if (tentativas > 0)
        {
            for (int i = 0; i < _modulesListAntigo.Count; i++)
            {
                Destroy(_modulesListAntigo[i]);
            }
            _modulesListAntigo.Clear();
        }
        GameObject module;
        Destroy(_modulesList[_modulesList.Count-1].gameObject);
        yield return new WaitForSeconds(0.05f);
        for (int i = _modulesList.Count - 2; i >= 0; i--)
        {
            _modulesList[i].GetComponent<Collider2D>().enabled = false;

            if (_directionsList[i] == 2)
            {
                rootSprite.eulerAngles = new Vector3(0, 0, 0);
                if (_directionsList[i + 1] == 4)
                {
                    module = Instantiate(moduleLeftUpDead, transform.position, moduleLeftUpDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else if (_directionsList[i + 1] == 6)
                {
                    module = Instantiate(moduleRightUpDead, transform.position, moduleRightUpDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else
                {
                    module = Instantiate(moduleVerticalDead, transform.position, moduleVerticalDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
            }else if (_directionsList[i] == 4)
            {
                rootSprite.eulerAngles = new Vector3(0, 0, -90);
                if (_directionsList[i + 1] == 8)
                {
                    module = Instantiate(moduleRightUpDead, transform.position, moduleRightUpDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else if (_directionsList[i + 1] == 2)
                {
                    module = Instantiate(moduleRightDownDead, transform.position, moduleRightDownDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else
                {
                    module = Instantiate(moduleHorizontalDead, transform.position, moduleHorizontalDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
            }
            else if (_directionsList[i] == 8)
            {
                rootSprite.eulerAngles = new Vector3(0, 0, 180);
                if (_directionsList[i + 1] == 4)
                {
                    module = Instantiate(moduleLeftUpDead, transform.position, moduleLeftUpDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else if (_directionsList[i + 1] == 6)
                {
                    module = Instantiate(moduleRightDownDead, transform.position, moduleRightDownDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else
                {
                    module = Instantiate(moduleVerticalDead, transform.position, moduleVerticalDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
            }
            else
            {
                rootSprite.eulerAngles = new Vector3(0, 0, 90);
                if (_directionsList[i + 1] == 2)
                {
                    module = Instantiate(moduleLeftDownDead, transform.position, moduleLeftDownDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else if (_directionsList[i + 1] == 8)
                {
                    module = Instantiate(moduleLeftUpDead, transform.position, moduleLeftUpDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
                else
                {
                    module = Instantiate(moduleHorizontalDead, transform.position, moduleHorizontalDead.transform.rotation, roots.transform);
                    _modulesListAntigo.Add(module);
                }
            }

            movePoint.position = _modulesList[i].transform.position;
            yield return new WaitForSeconds(0.05f);
            Destroy(_modulesList[i].gameObject);
            yield return new WaitForSeconds(0.25f);
        }
        _modulesList.Clear();

        Debug.Log(_modulesList.Count);

        while (gameOver.isPlaying){
            yield return null;
        }

        if (_modulesList.Count == 0)
        {
            movePoint.position = new Vector3(0.5f, -0.5f, 0);
        }
        //collider.enabled = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        movePoint.parent = null;
        _up = false;
        _down = true;
        _left = false;
        _right = false;
        _rootDirection1 = new Vector3(1, 1, 1);
        _rootDirection2 = new Vector3(-1, 1, 1);
        _lastDirection = Vector3.down;

        _modulesList = new List<GameObject>();
        _directionsList = new List<int>();

        isInGameOver = false;
        music.Play();
    }
}
