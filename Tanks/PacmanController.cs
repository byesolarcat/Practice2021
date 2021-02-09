using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tanks
{
	public class PacmanController
	{
		public int Score { get; private set; }

		public List<IMovable> MovableEntities;
		public List<EntityModel> DrawableEntities;

		public List<Tank> Tanks { get; private set; }
		public List<Wall> Walls { get; private set; }
		public List<Apple> Apples { get; private set; }
		private List<Bullet> invisibleBullets;
		private List<Bullet> bullets;

		public Kolobok Kolobok { get; set; }

		public bool GameOver { get; private set; }

		public PacmanController()
		{
			Score = 0;

			Walls = new List<Wall>();
			InitWalls();

			Kolobok = new Kolobok(new Position(170, 420), 36, 36);
			DrawableEntities = new List<EntityModel>();
			MovableEntities = new List<IMovable>();
			Tanks = new List<Tank>();
			Apples = new List<Apple>();
			bullets = new List<Bullet>();
			invisibleBullets = new List<Bullet>();

			MovableEntities.Add(Kolobok);
			DrawableEntities.Add(Kolobok);
		}

		public void CheckCollisions(Size clientSize)
		{
			IsOutOfBounds(Kolobok, clientSize);

			foreach (var wall in Walls)
			{
				if (IsBoxColiding(Kolobok, wall))
				{
					Direction collidingSide = ReturnCollidingSide(Kolobok, wall);
					if (collidingSide != Direction.NONE)
					{
						CorrectPosition(collidingSide, Kolobok, wall);
						SwitchDirection(Kolobok);
					}
				}

				foreach (var tank in Tanks)
				{
					Direction collidingSide = ReturnCollidingSide(tank, wall);
					if (IsBoxColiding(tank, wall))
					{
						CorrectPosition(collidingSide, tank, wall);
						SwitchDirection(tank);
					}
					else if (IsOutOfBounds(tank, clientSize))
					{
						SwitchDirection(tank);
					}
				}

				foreach (var bullet in bullets)
				{
					if (IsBoxColiding(wall, bullet))
					{
						bullets.Remove(bullet);
						DrawableEntities.Remove(bullet);
						MovableEntities.Remove(bullet);
						break;
					}
					if (IsBoxColiding(Kolobok, bullet))
					{
						GameOver = true;
						break;
					}
				}

				foreach (var invisibleBullet in invisibleBullets)
				{
					if (IsBoxColiding(wall, invisibleBullet))
					{
						invisibleBullets.Remove(invisibleBullet);
						DrawableEntities.Remove(invisibleBullet);
						MovableEntities.Remove(invisibleBullet);
						break;
					}
				}
			}
			bool keepChecking = true;
			foreach (var tank in Tanks)
			{
				foreach (var invisibleBullet in invisibleBullets)
				{
					if (IsBoxColiding(Kolobok, invisibleBullet) && !bullets.Exists(bullet => bullet.shooter == tank))
					{
						Bullet bullet = invisibleBullet.shooter.Shoot();
						DrawableEntities.Add(bullet);
						MovableEntities.Add(bullet);
						bullets.Add(bullet);
						break;
					}
				}


				if (IsBoxColiding(Kolobok, tank))
				{
					GameOver = true;
					break;
				}

				foreach (var anotherTank in Tanks)
				{
					if (!ReferenceEquals(anotherTank, tank) && (IsBoxColiding(tank, anotherTank)))
					{
						tank.Turn180();
						anotherTank.Turn180();
					}
				}

				foreach (var bullet in bullets)
				{
					if (IsBoxColiding(tank, bullet))
					{
						Tanks.Remove(tank);
						DrawableEntities.Remove(tank);
						MovableEntities.Remove(tank);

						bullets.Remove(bullet);
						DrawableEntities.Remove(bullet);
						MovableEntities.Remove(bullet);
						keepChecking = false;
						break;
					}
				}
				if (!keepChecking) break;
			}

			


			foreach (var apple in Apples)
			{
				if (IsBoxColiding(Kolobok, apple))
				{
					Score++;
					Apples.Remove(apple);
					DrawableEntities.Remove(apple);
					break;
				}
			}
		}

		private bool IsBoxColiding(EntityModel entity1, EntityModel entity2)
		{
			return !(entity1.Right <= entity2.Left ||
				entity1.Left > entity2.Right ||
				entity1.Bottom <= entity2.Top ||
				entity1.Top > entity2.Bottom);
		}

		public bool IsSpawnColliding(EntityModel entity)
		{
			foreach (var wall in Walls)
			{
				if (IsBoxColiding(entity, wall))
				{
					return true;
				}
			}
			foreach (var tank in Tanks)
			{
				if (IsBoxColiding(entity, tank))
				{
					return true;
				}
			}
			if (IsBoxColiding(entity, Kolobok))
			{
				return true;
			}
			return false;
		}

		private bool IsOutOfBounds(EntityModel entity, Size clientSize)
		{
			if(entity.Bottom > clientSize.Height)
			{
				entity.Coordinates.Y = clientSize.Height - entity.Height;
				return true;
			}
			else if (entity.Top < 0)
			{
				entity.Coordinates.Y = 0;
				return true;
			}
			else if (entity.Left < 0)
			{
				entity.Coordinates.X = 0;
				return true;
			}
			else if (entity.Right > clientSize.Width)
			{
				entity.Coordinates.X = clientSize.Width - entity.Height;
				return true;
			}
			return false;
		}

		private Direction ReturnCollidingSide(EntityModel collidingEntity, EntityModel obstacle)
		{
			if (collidingEntity.Right <= obstacle.Left + 1)
				return Direction.Right;
			if (collidingEntity.Left + 1 >= obstacle.Right)
				return Direction.Left;
			if (collidingEntity.Top + 1 >= obstacle.Bottom)
				return Direction.Up;
			if (collidingEntity.Bottom >= obstacle.Top)
				return Direction.Down;

			return Direction.NONE;
		}

		private void CorrectPosition(Direction collisionDirection, EntityModel collidingEntity, EntityModel obstacle)
		{
			switch (collisionDirection)
			{
				case Direction.Right:
					{
						collidingEntity.Coordinates.X = obstacle.Left - collidingEntity.Width - 1;
						break;
					}
				case Direction.Left:
					{
						collidingEntity.Coordinates.X = obstacle.Right + 1;
						break;
					}
				case Direction.Up:
					{
						collidingEntity.Coordinates.Y = obstacle.Bottom + 1;
						break;
					}
				case Direction.Down:
					{
						collidingEntity.Coordinates.Y = obstacle.Top - collidingEntity.Height - 1;
						break;
					}
			}
		}

		public void SwitchDirection(IMovable entity)
		{
			entity.SwitchDirection();
		}

		public void MoveEntity(IMovable entity)
		{
			entity.Move();
		}

		public void CheckEnemyAhead(Tank tank)
		{
			Random rnd = new Random();
			if (1 - rnd.NextDouble() > 0.5)
			{
				Bullet bullet = tank.ShootInvisibleBullet();
				MovableEntities.Add(bullet);
				invisibleBullets.Add(bullet);
			}
			
		}

		public void SpawnTank()
		{
			if(Tanks.Count < 5)
			{
				Random rnd = new Random();
				Position position = new Position(rnd.Next(0, 500), rnd.Next(0, 500));
				Tank tank = new Tank(position, 36, 36);
				if (!IsSpawnColliding(tank))
				{
					Tanks.Add(tank);
					MovableEntities.Add(tank);
					DrawableEntities.Add(tank);
				}
			}
		}

		public void SpawnApple()
		{
			if (Apples.Count < 5)
			{
				Random rnd = new Random();
				Position position = new Position(rnd.Next(0, 500), rnd.Next(0, 500));
				Apple apple = new Apple(position, 30, 34);
				if (!IsSpawnColliding(apple))
				{
					Apples.Add(apple);
					DrawableEntities.Add(apple);
				}
			}
			
		}

		public void KeyIsDown_Handler(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.D:
				case Keys.Right:
					{
						Kolobok.direction = Direction.Right;
						break;
					}
				case Keys.A:
				case Keys.Left:
					{
						Kolobok.direction = Direction.Left;
						break;
					}
				case Keys.W:
				case Keys.Up:
					{
						Kolobok.direction = Direction.Up;
						break;
					}
				case Keys.S:
				case Keys.Down:
					{
						Kolobok.direction = Direction.Down;
						break;
					}
				case Keys.Space:
					{
						Bullet bullet = Kolobok.Shoot();
						DrawableEntities.Add(bullet);
						MovableEntities.Add(bullet);
						bullets.Add(bullet);
						break;
					}
			}
			
		}

		private void InitWalls()
		{
			Wall wall1 = new Wall(new Position(38, 38), 36, 144);
			Wall wall2 = new Wall(new Position(116, 38), 36, 144);
			Wall wall3 = new Wall(new Position(194, 38), 36, 108);
			Wall wall4 = new Wall(new Position(278, 38), 36, 108);
			Wall wall5 = new Wall(new Position(356, 38), 36, 144);
			Wall wall6 = new Wall(new Position(434, 38), 36, 144);

			Wall wall7 = new Wall(new Position(0, 226), 36, 36);
			Wall wall8 = new Wall(new Position(77, 226), 72, 36);

			Wall wall9 = new Wall(new Position(194, 192), 36, 36);
			Wall wall10 = new Wall(new Position(278, 192), 36, 36);

			Wall wall11 = new Wall(new Position(356, 226), 72, 36);
			Wall wall12 = new Wall(new Position(473, 226), 36, 36);

			Wall wall13 = new Wall(new Position(194, 273), 36, 108);
			Wall wall14 = new Wall(new Position(278, 273), 36, 108);

			Wall wall15 = new Wall(new Position(38, 307), 36, 108);
			Wall wall16 = new Wall(new Position(116, 307), 36, 108);

			Wall wall17 = new Wall(new Position(356, 307), 36, 108);
			Wall wall18 = new Wall(new Position(434, 307), 36, 108);

			Walls.Add(wall1);
			Walls.Add(wall2);
			Walls.Add(wall3);
			Walls.Add(wall4);
			Walls.Add(wall5);
			Walls.Add(wall6);
			Walls.Add(wall7);
			Walls.Add(wall8);
			Walls.Add(wall9);
			Walls.Add(wall10);
			Walls.Add(wall11);
			Walls.Add(wall12);
			Walls.Add(wall13);
			Walls.Add(wall14);
			Walls.Add(wall15);
			Walls.Add(wall16);
			Walls.Add(wall17);
			Walls.Add(wall18);
		}
	}
}
