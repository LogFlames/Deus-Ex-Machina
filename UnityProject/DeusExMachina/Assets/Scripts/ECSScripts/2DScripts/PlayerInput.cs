using UnityEngine;
using System.Collections;
using Unity.Entities;

[RequireComponent(typeof(Player2D))]
public class PlayerInput : MonoBehaviour
{

    private Player2D player;

    void Start()
    {
        player = GetComponent<Player2D>();
    }
}

class PlayerInputBehaviourECS : ComponentSystem
{
    struct Components
    {
        public Player2D player;
    }

    protected override void OnUpdate()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);
        bool spaceReleased = Input.GetKeyUp(KeyCode.Space);

        foreach (var e in GetEntities<Components>())
        {
            e.player.SetDirectionalInput(directionalInput);

            if (spacePressed)
            {
                e.player.OnJumpInputDown();
            }
            if (spaceReleased)
            {
                e.player.OnJumpInputUp();
            }
        }
    }
}