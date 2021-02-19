using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Tanks
{
	public class PacmanController
	{
		public int Score { get; private set; }

		private int maxApples;
		private int maxTanks;

		public List<IMovable> MovableEntities;
		public List<IDrawable> DrawableEntities;

		public Kolobok Kolobok { get; private set; }
		public List<Tank> Tanks { get; private set; }

		private List<Wall> walls;
		private List<River> rivers;
		private List<Apple> apples;
		private List<Bullet> invisibleBullets;
		private List<Bullet> bullets;

		public bool GameOver { get; private set; }

		public PacmanController(int maxApples, int maxTanks)
		{
			this.maxApples = maxApples;
			this.maxTanks = maxTanks;
			Score = 0;

			DrawableEntities = new List<IDrawable>();
			MovableEntities = new List<IMovable>();


			walls = new List<Wall>();
			rivers = new List<River>();

			Tanks = new List<Tank>();
			apples = new List<Apple>();
			bullets = new List<Bullet>();
			invisibleBullets = new List<Bullet>();

			InitGameObjects();
		}

		public void SpawnTank(Size fieldSize)
		{
			if (Tanks.Count < maxTanks)
			{
				Random rnd = new Random();
				Position position = new Position(rnd.Next(0, fieldSize.Width - 36), rnd.Next(0, fieldSize.Height - 36));
				Tank tank = new Tank(position, 36, 36);
				if (!IsSpawnColliding(tank))
				{
					MovableEntities.Add(tank);
					DrawableEntities.Add(tank);
					Tanks.Add(tank);
				}
			}
		}

		public void SpawnApple(Size fieldSize)
		{
			if (apples.Count < maxApples)
			{
				Random rnd = new Random();
				Position position = new Position(rnd.Next(0, fieldSize.Width - 30), rnd.Next(0, fieldSize.Height - 34));
				Apple apple = new Apple(position, 30, 34);
				if (!IsSpawnColliding(apple))
				{
					DrawableEntities.Add(apple);
					apples.Add(apple);
				}
			}
		}

		public void LookForCollisions(Size gameField)
		{
			CheckKolobokCollisions(gameField);
			CheckWallCollisions();
			CheckTanksCollisions(gameField);
		}

		private void CheckWallCollisions()
		{
			foreach (var wall in walls)
			{
				CheckAndCorrectCollision(Kolobok, wall);

				foreach (var bullet in bullets)
				{
					if (IsBoxColiding(wall, bullet))
					{
						DrawableEntities.Add(new Explosion(new Position(bullet.Coordinates.X - 8, bullet.Coordinates.Y - 8), 16, 16));
						bullets.Remove(bullet);
						DrawableEntities.Remove(bullet);
						MovableEntities.Remove(bullet);
						if (wall.Health != null)
						{
							wall.Health -= 5;
							if (wall.Health <= 0)
							{
								walls.Remove(wall);
								DrawableEntities.Remove(wall);
								DrawableEntities.Add(new Explosion(
										new Position(wall.Coordinates.X, wall.Coordinates.Y), 36, 36));
								return;
							}
						}
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
		}
		private void CheckKolobokCollisions(Size gameField)
		{
			if (ReturnInBounds(Kolobok, gameField)) ;
			foreach (var bullet in bullets)
			{
				if (IsBoxColiding(Kolobok, bullet))
				{
					GameOver = true;
					Thread.Sleep(1000);
					return;
				}
			}
			foreach (var apple in apples)
			{
				if (IsBoxColiding(Kolobok, apple))
				{
					Score++;
					apples.Remove(apple);
					DrawableEntities.Remove(apple);
					break;
				}
			}

			foreach (var river in rivers)
			{
				CheckAndCorrectCollision(Kolobok, river);
			}
		}
		private void CheckTanksCollisions(Size gameField)
		{
			foreach (var tank in Tanks)
			{
				if (ReturnInBounds(tank, gameField))
					tank.SwitchDirection();

				foreach (var wall in walls)
				{
					CheckAndCorrectCollision(tank, wall);
				}

				foreach (var river in rivers)
				{
					CheckAndCorrectCollision(tank, river);
				}

				foreach (var invisibleBullet in invisibleBullets)
				{
					if (IsBoxColiding(Kolobok, invisibleBullet))
					{
						Random rnd = new Random();
						if (1 - rnd.NextDouble() > 0.9)
						{
							Bullet bullet = invisibleBullet.shooter.Shoot();

							DrawableEntities.Add(bullet);
							MovableEntities.Add(bullet);
							bullets.Add(bullet);
						}
					}
				}
				foreach (var anotherTank in Tanks)
				{
					if (IsBoxColiding(tank, anotherTank) && !ReferenceEquals(tank, anotherTank))
					{
						tank.Turn180();
						anotherTank.Turn180();
					}
				}
				if (IsBoxColiding(Kolobok, tank))
				{
					GameOver = true;
					Thread.Sleep(1000);
					break;
				}

				foreach (var bullet in bullets)
				{
					if (IsBoxColiding(tank, bullet))
					{
						Tanks.Remove(tank);
						DrawableEntities.Remove(tank);
						MovableEntities.Remove(tank);

						DrawableEntities.Add(new Explosion(tank.Coordinates, tank.Width, tank.Height));

						bullets.Remove(bullet);
						DrawableEntities.Remove(bullet);
						MovableEntities.Remove(bullet);
						return;
					}
				}
			}

		}

		private void CheckAndCorrectCollision(EntityModel entity, EntityModel obstacle)
		{
			if (IsBoxColiding(entity, obstacle))
				CorrectPosition(entity, obstacle);
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
			foreach (var wall in walls)
			{
				if (IsBoxColiding(entity, wall)) return true;
			}
			foreach (var tank in Tanks)
			{
				if (IsBoxColiding(entity, tank)) return true;
			}
			foreach (var river in rivers)
			{
				if (IsBoxColiding(entity, river)) return true;
			}
			if (IsBoxColiding(entity, Kolobok)) return true;
			return false;
		}

		private bool ReturnInBounds(EntityModel entity, Size clientSize)
		{
			if (entity.Bottom > clientSize.Height)
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
			if (collidingEntity.Right - 2 <= obstacle.Left)
				return Direction.Right;
			else if (collidingEntity.Left + 2 >= obstacle.Right)
				return Direction.Left;
			else if (collidingEntity.Top + 2 >= obstacle.Bottom)
				return Direction.Up;
			else if (collidingEntity.Bottom - 2 >= obstacle.Top)
				return Direction.Down;

			return Direction.NONE;
		}

		private void CorrectPosition(EntityModel collidingEntity, EntityModel obstacle)
		{
			switch (ReturnCollidingSide(collidingEntity, obstacle))
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


			IMovable entity = collidingEntity as IMovable;
			entity?.SwitchDirection();
		}

		public void MoveEntity(IMovable entity)
		{
			entity.Move();
		}

		public void DrawEntity(IDrawable entity, Graphics g)
		{
			entity.Draw(g);
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

		private void InitGameObjects()
		{
			using (StreamReader sr = new StreamReader("gamemap.txt"))
			{
				int coordX = 1;
				int coordY = 1;
				while (!sr.EndOfStream)
				{
					string row = sr.ReadLine();
					for (int i = 0; i < row.Length; i++)
					{
						if (row[i] == 'w')
						{
							int x = coordX * (i) * 36;
							int y = coordY;
							Wall wall = new Wall(new Position(x, y), 36, 36);

							walls.Add(wall);
							DrawableEntities.Add(wall);
						}
						else if (row[i] == 'r')
						{
							int x = coordX * (i) * 36;
							int y = coordY;
							River river = new River(new Position(x, y), 36, 36);

							rivers.Add(river);
							DrawableEntities.Add(river);
						}
						else if (row[i] == 'h')
						{
							int x = coordX * (i) * 36;
							int y = coordY;
							IndestructibleWall wall = new IndestructibleWall(new Position(x, y), 36, 36);

							walls.Add(wall);
							DrawableEntities.Add(wall);
						}
						else if (row[i] == 'k')
						{
							int x = coordX * (i) * 36;
							int y = coordY;
							Kolobok = new Kolobok(new Position(x, y), 33, 33);

							MovableEntities.Add(Kolobok);
							DrawableEntities.Add(Kolobok);
						}
					}
					coordY += 36;
				}
			}
		}
	}
}
