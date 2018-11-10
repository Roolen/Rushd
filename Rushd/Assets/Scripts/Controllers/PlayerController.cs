using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

namespace Assets.Scripts.Controllers
{

    /// <summary>
    /// Класс для управления танком, предназначеный для игрока.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool isMove;
        [SerializeField] private GameObject menuPanel;

        private TankController tank;
        private ICommandController[] commandsMoveTank;  // accessory, see the DirectionMove enumeration.
        private Transform cameraPlayer;
        private Text textHealth;

        public void SetCommand(int numberCommand, ICommandController command)
        {
            if (command != null)
                commandsMoveTank[numberCommand] = command;
        }

        private void Start()
        {
            tank = GetComponent<TankController>();
            textHealth = GameObject.Find("TextHealth").GetComponent<Text>();
            menuPanel = GameObject.Find("PanelMenu");
            menuPanel.SetActive(false);
            isMove = true;

            commandsMoveTank = new ICommandController[4];

            for (int i = 0; i < commandsMoveTank.Length; i++)
            {
                commandsMoveTank[i] = new TankMoveCommand(tank, (DirectionMove) i);
            }

            cameraPlayer = GameObject.Find("Camera").GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            tank.RotateTower(cameraPlayer.eulerAngles.y);
            textHealth.text = tank.Health.ToString();

            if (isMove)
            {
                ForwardMove();
                BackMove();
                LeftTurn();
                RightTurn();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                tank.ShootTank();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuPanel.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    menuPanel.SetActive(false);
                }
                else if (!menuPanel.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    menuPanel.SetActive(true);
                }
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
