using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float speedRotation;

    [SerializeField] private AudioClip _idleSound;
    [SerializeField] private AudioClip _driveSound;

    private AudioSource _motorSounds;
    Rigidbody2D _rb;

    public bool isFrozen = false;

    float x;
    float y;

    Vector2 lastMovementDirection = Vector2.up;

  
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _motorSounds = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

       

        if (x != 0)
        {
            y = 0;
        }

        // Запоминаем последнее направление движения, пока игрок жмет клавиши
        if (x != 0 || y != 0)
        {
            lastMovementDirection = new Vector2(x, y);
            Rotation(x, y);
        }
        // Включается строго один раз в моменты старта или остановки
        if ((x != 0 || y != 0) && _motorSounds.clip != _driveSound)
        {
            _motorSounds.clip = _driveSound;
            _motorSounds.loop = true;
            _motorSounds.Play();
        }
        else if ((x == 0 && y == 0) && _motorSounds.clip != _idleSound) // Явное условие остановки
        {
            _motorSounds.clip = _idleSound;
            _motorSounds.loop = true;
            _motorSounds.Play();
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(x, y).normalized * speed;
        if (x > 0 && y > 0 && -x > 0 && -y > 0) { return; }
        _rb.linearVelocity = new Vector2(x,y) * speed;

        if (isFrozen)
        {
            _rb.linearVelocity = new Vector2(0,0);
            return;
        }
    }

    void Rotation(float moveX, float moveY)
    {
        if(isFrozen)
        {
            moveX = 0;
            moveY = 0;
        }
        if (moveX < 0) // Влево
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (moveX > 0) // Вправо
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        // Поворот по вертикали (срабатывает, только если x == 0)
        else if (moveY < 0) // Вниз
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (moveY > 0) // Вверх
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
