using UnityEngine;

namespace Assets.Scripts.Controllers
{

    /// <summary>
    /// Класс для управления танком, предназначеный для игрока.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool isMove;

        private TankController tank;
        private ICommandController[] commandsMoveTank;  // accessory, see the DirectionMove enumeration.

        public void SetCommand(int numberCommand, ICommandController command)
        {
            if (command != null)
                commandsMoveTank[numberCommand] = command;
        }

        private void Start()
        {
            tank = GetComponent<TankController>();
            isMove = true;

            commandsMoveTank = new ICommandController[4];

            for (int i = 0; i < commandsMoveTank.Length; i++)
            {
                commandsMoveTank[i] = new TankMoveCommand(tank, (DirectionMove) i);
            }
        }

        private void FixedUpdate()
        {
            if (isMove)
            {
                ForwardMove();
                BackMove();
                LeftTurn();
                RightTurn();
            }

            if (Input.GetMouseButton(0))
            {
                tank.ShootTank();
            }
        }

        public void IsMove(bool flag)
        {
            isMove = flag;
        }

        private void ForwardMove()
        {
            if (Input.GetAxis("Vertical") > 0.1) commandsMoveTank[0].Execute(); 
        }

        private void BackMove()
        {
            if (Input.GetAxis("Vertical") < -0.1) commandsMoveTank[1].Execute();
        }

        private void LeftTurn()
        {
            if (Input.GetAxis("Horizontal") <  -0.1) commandsMoveTank[2].Execute();
        }

        private void RightTurn()
        {
            if (Input.GetAxis("Horizontal") > 0.1) commandsMoveTank[3].Execute();
        }

    }

    /// <summary>
    /// Комманда для движения танка.
    /// </summary>
    public class TankMoveCommand : ICommandController
    {
        private readonly TankController tankController;
        private readonly DirectionMove directionMove;

        public TankMoveCommand(TankController tank, DirectionMove directionMove)
        {
            tankController = tank;
            this.directionMove = directionMove;
        }

        void ICommandController.Execute()
        {
            tankController.MoveTank(directionMove);
        }

        void ICommandController.Undo()
        {

        }
    }
}
