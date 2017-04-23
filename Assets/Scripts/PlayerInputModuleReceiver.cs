using UnityEngine;

public interface PlayerInputModuleReceiver
{
    void onJump(float input);
    void onMovement(Vector2 input);
    void onMouseInput(Vector2 input);
}