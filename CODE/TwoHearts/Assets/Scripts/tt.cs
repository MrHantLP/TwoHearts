using UnityEngine;
using System.Collections;

public class tt : MonoBehaviour
{
    private Rigidbody2D player;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "jump")
        {
            player.velocity = new Vector2(player.velocity.x, 5f);
        }

    }
}