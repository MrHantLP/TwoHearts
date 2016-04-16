using UnityEngine;
using System.Collections;

public class Move_for_test : MonoBehaviour
{
    //переменная для установки макс. скорости персонажа
    public float maxSpeed = 10f;
    //переменная для определения направления персонажа вправо/влево
    private bool isFacingRight = true;
    //ссылка на компонент анимаций
    private Animator anim;
    private Rigidbody2D player;
    //находится ли персонаж на земле или в прыжке?
    private bool isGrounded = false;
    private int score = 4;
    private string GUIBOX = "";
    private bool PickedSocks = false;
    private bool PickedPants = false;
    private bool PickedShirts = false;
    private bool PickedRose = false;
    //ссылка на компонент Transform объекта
    //для определения соприкосновения с землей
    public Transform groundCheck;
    //радиус определения соприкосновения с землей
    private float groundRadius = 0.2f;
    //ссылка на слой, представляющий землю
    public LayerMask whatIsGround;
    /// <summary>
    /// Начальная инициализация
    /// </summary>
	private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхронизируется с расчетами физики
    /// </summary>
    private void FixedUpdate()
    {
        //определяем, на земле ли персонаж
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        //устанавливаем соответствующую переменную в аниматоре
       anim.SetBool("Ground", isGrounded);
        //устанавливаем в аниматоре значение скорости взлета/падения
        anim.SetFloat("vSpeed", player.velocity.y);
        //если персонаж в прыжке - выход из метода, чтобы не выполнялись действия, связанные с бегом
        if (!isGrounded)
            return;
        //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.
        //при стандартных настройках проекта 
        //-1 возвращается при нажатии на клавиатуре стрелки влево (или клавиши А),
        //1 возвращается при нажатии на клавиатуре стрелки вправо (или клавиши D)
        float move = Input.GetAxisRaw("Horizontal");

        //в компоненте анимаций изменяем значение параметра Speed на значение оси Х.
        //приэтом нам нужен модуль значения
        anim.SetFloat("Speed", Mathf.Abs(move));

        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
        //равную значению оси Х умноженное на значение макс. скорости
        player.velocity = new Vector2(move * maxSpeed, player.velocity.y);

        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if (move > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (move < 0 && isFacingRight)
            Flip();
    }

    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    /// 


    private void Update()
    {


        if (score != 0)
        {
            GUIBOX = "Нужно собрать еще " + score + " вещей";
        }
        else
        {
            GUIBOX = "Теперь можно идти";
        }
        
        float move = Input.GetAxisRaw("Horizontal");
        player.velocity = new Vector2(move * maxSpeed, player.velocity.y);
        if (move > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (move < 0 && isFacingRight)
            Flip();
        //если персонаж на земле и нажат пробел...
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            //устанавливаем в аниматоре переменную в false
            anim.SetBool("Ground", false);
            //прикладываем силу вверх, чтобы персонаж подпрыгнул
            player.AddForce(new Vector2(0, 300));
        }
    }


    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Die")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        if (col.gameObject.tag == "jump")
        {
            player.velocity = new Vector2(player.velocity.x, 5f);
        }


    }


    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "clothes")
        {

            if (col.gameObject.name == "pants")
            {

                PickedPants = true;
                Destroy(col.gameObject);
                score--;
            
            }
            if (col.gameObject.name == "rose")
            {
                PickedRose = true;
                Destroy(col.gameObject);
                score--;
            }
            if (col.gameObject.name == "shirt")
            {
                PickedShirts = true;
                Destroy(col.gameObject);
                score--;
            }
            if (col.gameObject.name == "socks")
            {
                PickedSocks = true;
                Destroy(col.gameObject);
                score--;
            }
        }

        if (col.gameObject.name == "End_level_room_1" && PickedPants && PickedRose && PickedShirts & PickedSocks)
        {
            Application.LoadLevel("Room_2");
        }

        if (col.gameObject.name == "End_level_room_2")
        {
            Application.LoadLevel("ToBeContinue");
        }

    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 50), GUIBOX );
    }
}