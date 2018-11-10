using System;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace Assets.Scripts.Controllers
{
    public class BotController : MonoBehaviour
    {
        /// <summary>
        /// Цель для движения бота.
        /// </summary>
        [SerializeField] private Transform target;
        /// <summary>
        /// Угол обзора бота.
        /// </summary>
        [SerializeField] [Range(0, 180)] private float angle;
        /// <summary>
        /// Скорость поворота башни для бота.
        /// </summary>
        [SerializeField] private float speedOfTower;


        private static Random random = new Random();
        private TankController tankController;
        /// <summary>
        /// Время тикета для создания новой цели.
        /// </summary>
        private float timerToChangeTarget;
        /// <summary>
        /// Время тикета для движения.
        /// </summary>
        private float timerForMove;

        private void Start()
        {
            target = Instantiate(new GameObject(), Vector3.one, Quaternion.identity).transform;
            tankController = GetComponent<TankController>();
            tankController.SpeedTurn = 50;
        }

        private void Update()
        {
            { //Уменьшает время счетчиков каждый кадр.
                timerToChangeTarget -= Time.deltaTime;
                timerForMove -= Time.deltaTime;
            }

            if (timerForMove <= 0)
            {
                Move();
                timerForMove = 0.05f;
            }
            if (timerToChangeTarget <= 0)
            {
                CreateNewPositionTarget();
                tankController.ShootTank();  //todo создать нормальную функцию стрельбы
                timerToChangeTarget = 3f;
            }
        }

        /// <summary>
        /// Создает новую (рандомную) позицию для цели.
        /// </summary>
        private void CreateNewPositionTarget()
        {
            int randomX = random.Next((int) transform.position.x - 50, (int) transform.position.x + 50);
            int randomZ = random.Next((int) transform.position.z - 50, (int) transform.position.z + 50);
            Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);

            target.position = randomPosition;
        }

        /// <summary>
        /// Перемещает танк бота на новую позицию.
        /// </summary>
        private void Move()
        {
            TankController tc = tankController;
            float angleToTarget = Vector3.SignedAngle(transform.position - target.position, transform.forward, Vector3.up);

            if (angleToTarget > 1f) tc.MoveTank(DirectionMove.Right);
            else if (angleToTarget < 1f) tc.MoveTank(DirectionMove.Left);

            Transform tower = tc.Tower;
            if (FindObjectOfType<PlayerController>())
            {
                Transform player = FindObjectOfType<PlayerController>().transform;

                float angleToTagertTower = Vector3.SignedAngle(tower.position - player.position, tower.forward, Vector3.up);
                tc.RotateTowerToAngle(angleToTagertTower * speedOfTower);
            }
            

            tc.MoveTank(DirectionMove.Forward);  //todo Нужна проверка на достижение цели
        }
    }
}
