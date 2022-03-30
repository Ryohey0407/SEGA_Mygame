using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    SpriteRenderer sp;    
    Animator animator;
    public PhysicsMaterial2D Slip;
    public PhysicsMaterial2D nonSlip;
    public GameObject bullet;
    public PlayerHP Life;
    public WingGauge WingHP;
    [SerializeField] LayerMask BlockLayer;
    [SerializeField] GameManager GameManager;
    [SerializeField] GameObject DeathEffect;
    [SerializeField] GameObject WaterEffect;
    AudioSource audioSource;
    [SerializeField] AudioClip JumpSE;
    [SerializeField] AudioClip Jump2SE;
    [SerializeField] AudioClip GetItemSE;
    [SerializeField] AudioClip PlayerAttackSE;
    [SerializeField] AudioClip DamageSE;

    //プレイヤーの状態
    public enum DIRECTION_TYPE
    {
        STOP,RIGHT,LEFT,
    }
    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;
    bool Nocontrol;
    public bool on_damage = false;//ダメージフラグ
    public bool blinking = false;//点滅フラグ
    public bool uncontrol = false;//移動不可
    public bool IsGround; //地面に着いているかどうか
    public string state;　//プレイヤーの状態
    float slopeAngle;　//（プレイヤーの角度）地面の角度

    //プレイヤーの変数
    public int MaxHP = 6;　//プレイヤーの最大HP
    public int HP;　//プレイヤーの今の体力
    public int MaxWing = 10;　//プレイヤーの最大羽ゲージ
    public float Wing;　//プレイヤーの今の羽ゲージ
    float xspeed;　//プレイヤーのスピード
    float walkspeed = 12f;　//プレイヤーの最大歩くスピード
    float acceleration = 0.125f;　//プレイヤーの加速量
    float jumppower = 900f;　//プレイヤーのジャンプ力


    // Start is called before the first frame update
    void Start()
    {
        state = "ENTER";
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = MaxHP;
        Wing = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP == 0)
        {
            Instantiate(DeathEffect, this.transform.position + transform.up * 1f, this.transform.rotation);
            Destroy(gameObject);
        }

        if(GameManager.WaitGame == true) //ゲームの開始、終了、ポーズ画面など
        {
            animator.Play("Player_Idle");
            rb.velocity = new Vector2(0,rb.velocity.y);
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        if (x == 0)
        {
            direction = DIRECTION_TYPE.STOP;     //止まっている
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            direction = DIRECTION_TYPE.RIGHT;    //右方向
        }
        else if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            direction = DIRECTION_TYPE.LEFT;    //左方向
        }

        if (blinking) //点滅時
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            sp.color = new Color(1f, 1f, 1f, level);
        }

        GroundCheck();

        if (uncontrol)　//ダメージを受けている時
        {
            return;
        }

        if (!WallCheck())　//壁に触れていない
        {
            boxCollider2D.sharedMaterial = nonSlip; //摩擦あり
        }
        else //壁に接触しているとき
        {
            state = "IDLE";
            boxCollider2D.sharedMaterial = Slip;　//摩擦なし
        }
        InputGet();        
        ChangeAnimation();
        Sensor(); //センサーを使い、地面の角度を検出し、プレイヤーの向きを変えます。

    }

    void InputGet()
    {
        if (uncontrol)
        {
            return;
        }
        
        //ジャンプする
        if (IsGround)
        {
            if (Input.GetKeyDown("space"))
            {
                JumpUp();
            }
        }
        else if (!IsGround && rb.velocity.y < 0 && Wing >= 1)
        {
            if (Input.GetKeyDown("space"))
            {
                MoreJumpUp();
            }
        }
        if(!IsGround && rb.velocity.y > 0)
        {
            state = "JUMP";
        }
        //ジャンプボタン離して降下
        if (Input.GetKeyUp("space") && rb.velocity.y > 0)
        {
            JumpDown();
        }
        //ジャンプが頂点になったとき
        if(!IsGround && rb.velocity.y <= 0)
        {
            state = "FALL";
        }
        //ショット
        if (Input.GetKeyDown("left ctrl"))
        {
            audioSource.PlayOneShot(PlayerAttackSE);
            Instantiate(bullet, transform.position + new Vector3(transform.localScale.x* 1f, 0f, 0f), transform.rotation);
        }
    }


    void ChangeAnimation()
    {
        if (state == "HURT")
        {
            animator.Play("Player_Hurt");
        }
        if (state == "IDLE")
        {
            animator.Play("Player_Idle");
            //animator.SetBool("Walking", false);
            //animator.SetBool("IsJumping", false);
            //animator.SetBool("IsFalling", false);
        }
        if (state == "WALK")
        {
            animator.Play("Player_Walk");
            //animator.SetBool("Walking", true);
        }
        if (state == "JUMP")
        {
            animator.Play("Player_Jump");
            //animator.SetBool("IsJumping", true);
        }
        if (state == "FALL")
        {
            animator.Play("Player_Fall");
            //animator.SetBool("IsJumping", false);
            //animator.SetBool("IsFalling", true);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.WaitGame)
        {
            return;
        }
        Move();
    }
       
    void Move()
    {
        if (uncontrol)
        {
            return;
        }


        if (IsGround) //地面に触れている
        {
            switch (direction)
            {
                case DIRECTION_TYPE.STOP:
                    state = "IDLE";
                    xspeed = Mathf.Lerp(xspeed, 0, 0.1f); //歩くスピードを0に近づける
                    break;
                case DIRECTION_TYPE.RIGHT:
                    state = "WALK";
                    if (xspeed >= 0 && xspeed < walkspeed) //だんだん速く
                    {
                        xspeed += acceleration;
                    }
                    else if (xspeed < 0)
                    {
                        xspeed += acceleration * 5; //急な方向転換
                    }
                    else
                    {
                        xspeed = walkspeed; //最大スピードで一定
                    }
                    break;
                case DIRECTION_TYPE.LEFT:
                    state = "WALK";
                    if (xspeed <= 0 && xspeed > -walkspeed) //だんだん速く
                    {
                        xspeed -= acceleration;
                    }
                    else if (xspeed > 0)
                    {
                        xspeed -= acceleration * 5; //急な方向転換
                    }
                    else
                    {
                        xspeed = -walkspeed; //最大スピードで一定
                    }
                    break;
            }
            rb.velocity = (transform.right * xspeed); //プレイヤーの向いている方向に走る
        }
        else
        {
            switch (direction)
            {
                case DIRECTION_TYPE.STOP:
                    xspeed = Mathf.Lerp(xspeed, 0, 0.1f);
                    break;
                case DIRECTION_TYPE.RIGHT:
                    if (xspeed < walkspeed)
                    {
                        xspeed += acceleration;
                    }
                    else
                    {
                        xspeed = walkspeed;
                    }
                    break;
                case DIRECTION_TYPE.LEFT:
                    if (xspeed > -walkspeed)
                    {
                        xspeed -= acceleration;
                    }
                    else
                    {
                        xspeed = -walkspeed;
                    }
                    break;
            }
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, xspeed, 0.1f), rb.velocity.y); //ジャンプ動作と競合を防ぐため、x速度をxspeedにだんだん近づける
        }
    }


    void JumpUp()
    {
        state = "JUMP";
        audioSource.PlayOneShot(JumpSE);
        rb.AddForce(transform.up * jumppower);　//プレイヤーの頭上に向かってジャンプ
    }

    void JumpDown()
    {
        state = "FALL";
        rb.velocity = new Vector2(rb.velocity.x, 0f); //y方向速度を0にしてジャンプを止める
    }

    void MoreJumpUp()
    {
        state = "MoreJUMP";
        audioSource.PlayOneShot(Jump2SE);
        Wing -= 1f;　//羽ゲージを消費
        rb.AddForce(transform.up * 1.5f * jumppower);
    }

    void Sensor()
    {
        Vector2 temp= transform.localScale;
        //角度センサー右
        Vector3 GNDrightstartVec = transform.position + transform.right * 0.5f * temp.x;
        Vector3 GNDrightendVec = GNDrightstartVec - transform.up * 1.5f;
        //Debug.DrawLine(GNDrightstartVec, GNDrightendVec, Color.yellow);
        //角度センサー左
        Vector3 GNDleftstartVec = transform.position + transform.right * -0.5f * temp.x;
        Vector3 GNDleftendVec = GNDleftstartVec - transform.up * 1.5f;
        //Debug.DrawLine(GNDleftstartVec, GNDleftendVec, Color.red);

        //RaycastHit2Dで地面との法線ベクトルを取得
        RaycastHit2D groundrightHit = Physics2D.Linecast(GNDrightstartVec, GNDrightendVec, BlockLayer);
        RaycastHit2D groundleftHit = Physics2D.Linecast(GNDleftstartVec, GNDleftendVec, BlockLayer);

        //法線ベクトルを角度に変換
        Vector3 angle1 = Quaternion.FromToRotation(new Vector2(0, 1), groundrightHit.normal).eulerAngles;
        Vector3 angle2 = Quaternion.FromToRotation(new Vector2(0, 1), groundleftHit.normal).eulerAngles;

        float s = Mathf.Round(angle1.z);
        float t = Mathf.Round(angle2.z);

        //プレイヤーの向きを当てはめるための計算
        if(groundleftHit && groundrightHit)
        {
            if (t < 90 && s > 270 || s < 90 && t > 270)
            {
                slopeAngle = 180 + (s + t) / 2;
            }
            else
            {
                slopeAngle = (s + t) / 2;
            }
        }
        else if(groundrightHit)
        {
            slopeAngle = s;
        }
        else if(groundleftHit)
        {
            slopeAngle = t;
        }

        //プレイヤーの向きを地面の角度に合わせる
        if (groundleftHit || groundrightHit)
        {
            this.transform.localRotation = Quaternion.Euler(0f, 0f, slopeAngle);
        }
        else if(CeilCheck())
        {
            this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            this.transform.Rotate(0f, 0f, Mathf.MoveTowardsAngle(-transform.localRotation.z, 0, 0f));
        }
    }

    void GroundCheck()
    {
        //ジャンプ（上昇中）はセンサーを出さない
        if (state == "JUMP")
        {
            return;
        }

        //地面センサー
        Vector3 startpoint1 = transform.position + transform.right * 0.5f;
        Vector3 startpoint2 = transform.position + transform.right * -0.5f;
        Vector3 endpoint1 = startpoint1 + transform.up * -1.2f;
        Vector3 endpoint2 = startpoint2 + transform.up * -1.2f;
        //Debug.DrawLine(startpoint1, endpoint1, Color.green);
        //Debug.DrawLine(startpoint2, endpoint2, Color.green);

        //地面を検出
        RaycastHit2D groundhit1 = Physics2D.Linecast(startpoint1, endpoint1, BlockLayer);
        RaycastHit2D groundhit2 = Physics2D.Linecast(startpoint2, endpoint2, BlockLayer);
        if(groundhit1 || groundhit2)
        {
            IsGround = true;
            if (groundhit1)
            {
                if(slopeAngle == 0)
                {
                    transform.position = new Vector2(transform.position.x, groundhit1.point.y + 1f); //地面に密着
                }
                else if(slopeAngle == 90 || slopeAngle == 270)
                {
                    //transform.position = new Vector2(groundhit1.point.x, transform.position.y); //地面に密着(壁を走るとき)
                }
            }
            else if (groundhit2)
            {
                if (slopeAngle == 0)
                {
                    //transform.position = new Vector2(transform.position.x, groundhit2.point.y + 1f); //地面に密着
                }
                else if (slopeAngle == 90 || slopeAngle == 270)
                {
                    //transform.position = new Vector2(groundhit2.point.x, transform.position.y); //地面に密着(壁を走るとき)
                }
            }
        }
        else
        {
            IsGround = false;            
        }

    }
    bool WallCheck()
    {
        //壁センサー
        Vector3 startpoint = transform.position - transform.up * 0.5f;
        Vector3 endpoint = startpoint + transform.right * 0.6f * transform.localScale.x;
        Debug.DrawLine(startpoint, endpoint, Color.red);
        return Physics2D.Linecast(startpoint, endpoint, BlockLayer);
    }

    bool CeilCheck()
    {
        //天井センサー
        Vector3 startpoint1 = transform.position + transform.right * 0.5f;
        Vector3 startpoint2 = transform.position + transform.right * -0.5f;
        Vector3 endpoint1 = startpoint1 + transform.up * 1.5f;
        Vector3 endpoint2 = startpoint2 + transform.up * 1.5f;
        Debug.DrawLine(startpoint1, endpoint1, Color.red);
        Debug.DrawLine(startpoint2, endpoint2, Color.red);
        return Physics2D.Linecast(startpoint1, endpoint1, BlockLayer)|| Physics2D.Linecast(startpoint2, endpoint2, BlockLayer);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (on_damage)
            {
                return;
            }
            else
            {
                Damage();
            }
        }
        if (other.gameObject.tag == "Water")
        {
            rb.gravityScale = 2.5f;
            xspeed *= 0.98f;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (on_damage)
            {
                return;
            }
            else
            {
                Damage();
            }
        }
        if (other.gameObject.tag == "Heart")
        {
            audioSource.PlayOneShot(GetItemSE);
            if (MaxHP > HP)
            {
                HP += 1;
            }
        }
        if (other.gameObject.tag == "Wing")
        {
            audioSource.PlayOneShot(GetItemSE);
            if (MaxWing > Wing)
            {
                Wing += 2;
            }
            else
            {
                Wing = MaxWing;
            }
        }
        if (other.gameObject.tag == "Water")
        {
            Instantiate(WaterEffect, this.transform.position, Quaternion.Euler(0, 0, 0));
        }
        if ((other.gameObject.tag == "GAME CLEAR"))
        {
            GameManager.GameClear();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Instantiate(WaterEffect, this.transform.position, Quaternion.Euler(0, 0, 0));
            rb.gravityScale = 3.8f;
        }
    }


    void Damage()
    {
        animator.Play("Player_Hurt");
        audioSource.PlayOneShot(DamageSE);
        state = "HURT";
        on_damage = true;
        uncontrol = true;
        HP = HP - 1;
        xspeed = 0;
        rb.velocity = new Vector2(-4f * transform.localScale.x,6f);
        Invoke("Hurtoff", 0.5f);
    }

    void Hurtoff()
    {
        StartCoroutine("MUTEKI");
        blinking = true;
        uncontrol = false;
    }
    IEnumerator MUTEKI()
    {
        yield return new WaitForSeconds(2);
        blinking = false;
        on_damage = false;
        sp.color = new Color(1f, 1f, 1f, 1f);
    }

    

}
