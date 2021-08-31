using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Gesture { tap, up, down, left, right };
//제스처 열거
public class PlayerCtrl : MonoBehaviour
{
    // public TextMesh InputText;
    public GameObject GameOverUI;
    private Vector2 Input_Start;
    private Vector2 Input_End;
    private Gesture gesture;
    public float Position_x;
    public float Position_y;
    public float Speed;
    public AudioSource overSource;

    public TextMesh timeText;
    private float surviveTime;
    private float bestTime = 0;
    private bool isGameover; // 게임 오버 상태

    void Start()
    {
        Position_x = 0.0f;
        Position_y = 0.0f;
        Speed = 10.0f;
        StartCoroutine(SpeedUP());
        GameOverUI.SetActive(false);
        overSource = GetComponent<AudioSource>();


        surviveTime = 0;
        isGameover = false;
    }

    IEnumerator GameOver()
    {
        GameOverUI.SetActive(true);
        Speed = 0.0f;
        StopCoroutine(SpeedUP());
        yield return new WaitForSeconds(1.5f);

        Application.LoadLevel(0);

    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Trap"))
        {
            StartCoroutine(GameOver());
            isGameover = true;

            overSource.Play();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time:" + (int)surviveTime;
        }

        else
        {
            bestTime = PlayerPrefs.GetFloat("BestTime");
            // 이전까지의 최고 기록보다 현재 생존 시간이 더 크다면
            if (surviveTime > bestTime)
            {
                // 최고 기록의 값을 현재 생존 시간의 값으로 변경 
                bestTime = surviveTime;
                // 변경된 최고 기록을 BestTime 키로 저장
                PlayerPrefs.SetFloat("BestTime", bestTime);
            }
            // 최고 기록을 recordText 텍스트 컴포넌트를 통해 표시
            timeText.text = "Best Time: " + (int)bestTime;
        }

        GestureInput();
        Run();
    }

    void GestureInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Input_Start = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Input_End = Input.mousePosition;
            Vector2 CurrentInput = (Input_End - Input_Start);
            CurrentInput.Normalize();

            if (CurrentInput.x > 0 && CurrentInput.y < 0.5f && CurrentInput.y > -0.5f)
            {
                // InputText.text = "Right";
                gesture = Gesture.right;

            }
            else if (CurrentInput.x < 0 && CurrentInput.y < 0.5f && CurrentInput.y > -0.5f)
            {
                // InputText.text = "Left";
                gesture = Gesture.left;
            }
            else if (CurrentInput.y < 0 && CurrentInput.x < 0.5f && CurrentInput.x > -0.5f)
            {
                // InputText.text = "Down";
                gesture = Gesture.down;
            }


            else if (CurrentInput.y > 0 && CurrentInput.x < 0.5f && CurrentInput.y > -0.5f)
            {
                // InputText.text = "Up";
                gesture = Gesture.up;
            }
            else
            {
                // InputText.text = "Tap";
                gesture = Gesture.tap;
            }
            Move_x();
            Move_y();
        }
    }

    void Run()
    {

        transform.position += transform.forward * Speed * Time.deltaTime;
        Vector3 MoveDir = new Vector3(Position_x, Position_y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, MoveDir, 10.0f * Time.deltaTime);


    }

    void Move_x()
    {
        if (gesture == Gesture.left)
        {
            if (Position_x < 2.5f)
            {
                Position_x += 2.5f;
            }
        }

        if (gesture == Gesture.right)
        {
            if (Position_x > -2.5f)
            {
                Position_x -= 2.5f;
            }
        }
    }
    void Move_y()
    {
        if (gesture == Gesture.down)
        {
            if (Position_y < 2.5f)
            {
                Position_y += 2.5f;
            }
        }

        if (gesture == Gesture.up)
        {
            if (Position_y > -2.5f)
            {
                Position_y -= 2.5f;
            }
        }
    }


    IEnumerator SpeedUP()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            Speed = Speed + 7.0f;
        }
        yield return null;
    }
}
