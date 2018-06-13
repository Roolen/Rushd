using UnityEngine;

namespace Assets.Scripts
{
    public delegate void KeyboardDown();

    public class PlayerController : MonoBehaviour
    {
        public event KeyboardDown OnKeyboardDown;
        private TankController tank;

        private void Start()
        {
            tank = GetComponent<TankController>();

            OnKeyboardDown += ForwardMove;
            OnKeyboardDown += BackMove;
            OnKeyboardDown += LeftTurn;
            OnKeyboardDown += RightTurn;
        }

        private void FixedUpdate()
        {
            if (OnKeyboardDown != null) OnKeyboardDown.Invoke();
        }

        private void ForwardMove()
        {
            if (Input.GetAxis("Vertical") > 0.1) tank.MoveForwardTank();
        }

        private void BackMove()
        {
            if (Input.GetAxis("Vertical") < -0.1) tank.MoveBackTank();
        }

        private void LeftTurn()
        {
            if (Input.GetAxis("Horizontal") <  -0.1) tank.TurnLeftTank();
        }

        private void RightTurn()
        {
            if (Input.GetAxis("Horizontal") > 0.1) tank.TurnRightTank();
        }

    }
}
