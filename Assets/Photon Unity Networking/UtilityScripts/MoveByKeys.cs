/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;

/// <summary>
/// Very basic component to move a GameObject by WASD and Space.
/// </summary>
/// <remarks>
/// Requires a PhotonView. 
/// Disables itself on GameObjects that are not owned on Start.
/// 
/// Speed affects movement-speed. 
/// JumpForce defines how high the object "jumps". 
/// JumpTimeout defines after how many seconds you can jump again.
/// </remarks>
[RequireComponent(typeof (PhotonView))]
public class MoveByKeys : Photon.MonoBehaviour
{
    public float Speed = 10f;
    public float JumpForce = 200f;
    public float JumpTimeout = 0.5f;

    private bool isSprite;
    private float jumpingTime;
    private Rigidbody body;
    private Rigidbody2D body2d;

    public void Start()
    {
        //enabled = photonView.isMine;
        this.isSprite = (GetComponent<SpriteRenderer>() != null);

        this.body2d = GetComponent<Rigidbody2D>();
        this.body = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            return;
        }

        if ((Input.GetAxisRaw("Horizontal") < -0.1f) || (Input.GetAxisRaw("Horizontal") > 0.1f))
        {
            transform.position += Vector3.right * (Speed * Time.deltaTime) * Input.GetAxisRaw("Horizontal");
        }

        // jumping has a simple "cooldown" time but you could also jump in the air
        if (this.jumpingTime <= 0.0f)
        {
            if (this.body != null || this.body2d != null)
            {
                // obj has a Rigidbody and can jump (AddForce)
                if (Input.GetKey(KeyCode.Space))
                {
                    this.jumpingTime = this.JumpTimeout;

                    Vector2 jump = Vector2.up*this.JumpForce;
                    if (this.body2d != null)
                    {
                        this.body2d.AddForce(jump);
                    }
                    else if (this.body != null)
                    {
                        this.body.AddForce(jump);
                    }
                }
            }
        }
        else
        {
            this.jumpingTime -= Time.deltaTime;
        }

        // 2d objects can't be moved in 3d "forward"
        if (!this.isSprite)
        {
            if ((Input.GetAxisRaw("Vertical") < -0.1f) || (Input.GetAxisRaw("Vertical") > 0.1f))
            {
                transform.position += Vector3.forward * (Speed * Time.deltaTime) * Input.GetAxisRaw("Vertical");
            }
        }
    }
}
