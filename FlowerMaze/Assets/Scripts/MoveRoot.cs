using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveRoot : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform movePoint;
    public Transform rootSprite;

    public LayerMask whatStopsMovement;

    private bool _up, _down, _left, _right;

    public GameObject moduleLeftUp;
    public GameObject moduleLeftDown;
    public GameObject moduleRightUp;
    public GameObject moduleRightDown;
    public GameObject moduleVertical;
    public GameObject moduleHorizontal;

    public GameObject roots;
    public GameObject sprite;

    private Vector3 _lastDirection;
    private Vector3 _direction;
    private Vector3 _rootDirection1, _rootDirection2;
    private bool _changeDirection;
    
    public AudioSource music, gameOver, rootSound;

    // Start is called before the first frame update
    void Start(){
        movePoint.parent = null;   
        _up = false;
        _down = true;
        _left = false;
        _right = false;
        _rootDirection1 = new Vector3(1, 1, 1);
        _rootDirection2 = new Vector3(-1, 1, 1);
    }

    private void Update(){
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0){
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && _lastDirection != Vector3.down){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, 1, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(0, 1, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 180);
                    _lastDirection = Vector3.up;
                    _up = true;
                    RootMoviment();
                }
            } else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && _lastDirection != Vector3.up){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, -1, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(0, -1, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 0);
                    _lastDirection = Vector3.down;
                    _down = true;
                    RootMoviment();
                }
            } else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && _lastDirection != Vector3.right){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1, 0, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(-1, 0, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, -90);
                    _lastDirection = Vector3.left;
                    _left = true;
                    RootMoviment();
                }
            } else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && _lastDirection != Vector3.left){
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1, 0, 0), 0.2f, whatStopsMovement)){
                    movePoint.position += new Vector3(1, 0, 0);
                    rootSprite.eulerAngles = new Vector3(0, 0, 90);
                    _lastDirection = Vector3.right;
                    _right = true;
                    RootMoviment();
                }
            }
        }
    }

    private void RootMoviment(){
         CreateModule();
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

    private void CreateModule(){
        if (_lastDirection == Vector3.up || _lastDirection == Vector3.down){
            CreateModuleVertical();
        } else{
            CreateModuleHorizontal();
        }
    }

    private void CreateModuleVertical(){
        if (_up && _left){
            InstantiateModule(moduleRightUp);
            _left = false;
        } else if (_up && _right){
            InstantiateModule(moduleLeftUp);
            _right = false;
        } else if (_down && _left){
            InstantiateModule(moduleRightDown);
            _left = false;
        } else if (_down && _right){
            InstantiateModule(moduleLeftDown);
            _right = false;
        } else{
            InstantiateModule(moduleVertical);
        }
    }

    private void CreateModuleHorizontal(){
        if (_up && _left){
            InstantiateModule(moduleLeftDown);
            _up = false;
        } else if (_up && _right){
            InstantiateModule(moduleRightDown);
            _up = false;
        } else if (_down && _left){
            InstantiateModule(moduleLeftUp);
            _down = false;
        } else if (_down && _right){
            InstantiateModule(moduleRightUp);
            _down = false;
        } else{
            Instantiate(moduleHorizontal, transform.position, moduleHorizontal.transform.rotation, roots.transform);
        }
    }

    private void InstantiateModule(GameObject module){
        var transformAtual = transform;
        Instantiate(module, transformAtual.position, transformAtual.rotation, roots.transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("danger") || collision.gameObject.CompareTag("modulo")){
            music.Stop();
            StartCoroutine(PlayGameover());
        }
        if (collision.gameObject.CompareTag("final")){
            SceneManager.LoadScene("End Game");
        }
    }

    private IEnumerator PlayGameover() {
        gameOver.volume = 0.25f;
        gameOver.Play();

        while (gameOver.isPlaying){
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
