using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TrexRunner.Entities
{
    public class ObstacleManager : IGameEntity
    {
        private const float MIN_SPAWN_DISTANCE = 10;
        private const int MIN_OBSTACLE_DISTANCE = 6;
        private const int MAX_OBSTACLE_DISTANCE = 28;

        private double _lastSpawnScore = -1;
        private double _currentTargetDistance;
       

        private readonly EntityManager _entityManager;
        private readonly Trex _trex;
        private readonly ScoreBoard _scoreBoard;
        private readonly Random _random;
        private Texture2D _spriteSheet;
        public bool IsEnabled { get; set; }
        public bool CanSpawnObstacles => IsEnabled && _scoreBoard.Score >= MIN_SPAWN_DISTANCE;
        public int DrawOrder => 0;

        public ObstacleManager(EntityManager entityManager, Trex trex, ScoreBoard scoreBoard, Texture2D spriteSheet)
        {
            _entityManager = entityManager;
            _trex = trex;
            _scoreBoard = scoreBoard;
            _random = new Random();
            _spriteSheet = spriteSheet;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
                
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;
            if  (CanSpawnObstacles && 
                (_lastSpawnScore <= 0 || (_scoreBoard.Score - _lastSpawnScore >= _currentTargetDistance)))
            {
                _currentTargetDistance = _random.NextDouble() * (MAX_OBSTACLE_DISTANCE - MIN_OBSTACLE_DISTANCE) + MIN_OBSTACLE_DISTANCE;
                _lastSpawnScore = _scoreBoard.Score;

                SpawnRandomObstacle();
            }

            foreach(Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                if (obstacle.Position.X < -200)
                    _entityManager.RemoveEntity(obstacle);

            }
        }

        private void SpawnRandomObstacle()
        {

            Obstacle obstacle = null;
            CactusGroup.GroupSize randomGroupSize = (CactusGroup.GroupSize)_random.Next((int)CactusGroup.GroupSize.Small, (int)CactusGroup.GroupSize.Large + 1);
            bool isLarge = _random.NextDouble() > 0.5f;
            float posY = isLarge ? 85 : 95;
            obstacle = new CactusGroup(_spriteSheet, isLarge, randomGroupSize, _trex, new Vector2(TrexRunnerGame.WINDOW_WIDTH, posY));
            _entityManager.AddEntity(obstacle);
        }
    }


}

