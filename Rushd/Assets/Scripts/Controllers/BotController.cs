using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Controllers
{
    public class BotController : MonoBehaviour
    {
        public GameObject defenseTarget;
        public Transform target;
        public float angle;

        private static System.Random random = new System.Random();
        private TankController tankController;
        private float timer = 0;
        private float timerForMove = 0;

        private void Start()
        {
            target = Instantiate(new GameObject(), Vector3.one, Quaternion.identity).transform;
            tankController = GetComponent<TankController>();
            tankController.SpeedTurn = 50;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            timerForMove -= Time.deltaTime;

            if (timerForMove <= 0)
            {
                Move();
                timerForMove = 0.05f;
            }
            if (timer <= 0)
            {
                MoveToRandomPositionTarget();
                tankController.ShootTank();
                timer = 3f;
            }
        }

        private void MoveToRandomPositionTarget()
        {
            int randomX = random.Next((int) transform.position.x - 50, (int) transform.position.x + 50);
            int randomZ = random.Next((int) transform.position.z - 50, (int) transform.position.z + 50);
            Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);
            target.position = randomPosition;
        }

        private void Move()
        {
            TankController tc = tankController;

            if (Vector3.SignedAngle(transform.position - target.position, transform.forward, Vector3.up) > 1f) tc.MoveTank(DirectionMove.Right);
            else if (Vector3.SignedAngle(transform.position - target.position, transform.forward, Vector3.up) < 1f) tc.MoveTank(DirectionMove.Left);

            Transform tower = tc.Tower;
            Transform player = FindObjectOfType<PlayerController>().transform;
            float angleToTagerTower = Vector3.SignedAngle(tower.position - player.position, tower.forward, Vector3.up);
            tc.RotateTowerToAngle(angleToTagerTower * 4f);
            

            tc.MoveTank(DirectionMove.Forward);
        }
    }
}
