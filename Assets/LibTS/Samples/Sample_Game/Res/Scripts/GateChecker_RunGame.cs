using LibTS;
using UnityEngine;

public class GateChecker_RunGame : CollisionChecker
{
    private void OnTriggerStay(Collider other)
    {
        if (CollisionCheck(other))
        {
            PlayerController_RunGame.PlayerControllerStatic.AllowOperation = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CollisionCheck(other))
        {
            PlayerController_RunGame.PlayerControllerStatic.AllowOperation = true;
        }
    }
}
