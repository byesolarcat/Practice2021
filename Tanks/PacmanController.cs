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

			InitWalls();
			InitRivers();

			Kolobok = new Kolobok(new Position(170, 420), 36, 36);
			
			MovableEntities.Add(Kolobok);
			DrawableEntities.Add(Kolobok);
		}

		public void SpawnTank()
		{
			if (Tanks.Count < maxTanks)
			{
				Random rnd = new Random();
				Position position = new Position(rnd.Next(0, 500), rnd.Next(0, 500));
				Tank tank = new Tank(position, 36, 36);
				if (!IsSpawnColliding(tank))
				{
					MovableEntities.Add(tank);
					DrawableEntities.Add(tank);
					Tanks.Add(tank);
				}
			}
		}

		public void SpawnApple()
		{
			if (apples.Count < maxApples)
			{
				Random rnd = new Random();
				Position position = new Position(rnd.Next(0, 500), rnd.Next(0, 500));
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
			CheckTanksCollisions(gameField);

			foreach (var wall in walls)
			{
				CheckAndCorrectCollision(Kolobok, wall);

				foreach (var bullet in bullets)
				{
					if (IsBoxColiding(wall, bullet))
					{
						DrawableEntities.Add(new Explosion(new Position(bullet.Coordinates.X - 16, bullet.Coordinates.Y - 16), 32, 32, Direction.NONE));

						bullets.Remove(bullet);
						DrawableEntities.Remove(bullet);
						MovableEntities.Remove(bullet);
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
			foreach (var wall in walls)
			{
				if (IsBoxColiding(Kolobok, wall))
					CorrectPosition(Kolobok, wall);

				foreach (var bullet in bullets)
				{
					if (IsBoxColiding(Kolobok, bullet))
					{
						GameOver = true;
						return;
					}
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
					break;
				}

				foreach (var bullet in bullets)
				{
					if (IsBoxColiding(tank, bullet))
					{
						Tanks.Remove(tank);
						DrawableEntities.Remove(tank);
						MovableEntities.Remove(tank);

						DrawableEntities.Add(new Explosion(tank.Coordinates, tank.Width, tank.Height, Direction.NONE));

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
			if (IsBoxColiding(entity, Kolobok)) return true;
			return false;
		}

		private bool ReturnInBounds(EntityModel entity, Size clientSize)
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

			if (collidingEntity is Kolobok) return;

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

			//Wall wall9 = new Wall(new Position(194, 192), 36, 36);
			//Wall wall10 = new Wall(new Position(278, 192), 36, 36);

			Wall wall11 = new Wall(new Position(356, 226), 72, 36);
			Wall wall12 = new Wall(new Position(473, 226), 36, 36);

			Wall wall13 = new Wall(new Position(194, 273), 36, 108);
			Wall wall14 = new Wall(new Position(278, 273), 36, 108);

			Wall wall15 = new Wall(new Position(38, 307), 36, 108);
			Wall wall16 = new Wall(new Position(116, 307), 36, 108);

			Wall wall17 = new Wall(new Position(356, 307), 36, 108);
			Wall wall18 = new Wall(new Position(434, 307), 36, 108);

			walls.Add(wall1);
			walls.Add(wall2);
			walls.Add(wall3);
			walls.Add(wall4);
			walls.Add(wall5);
			walls.Add(wall6);
			walls.Add(wall7);
			walls.Add(wall8);
			//walls.Add(wall9);
			//walls.Add(wall10);
			walls.Add(wall11);
			walls.Add(wall12);
			walls.Add(wall13);
			walls.Add(wall14);
			walls.Add(wall15);
			walls.Add(wall16);
			walls.Add(wall17);
			walls.Add(wall18);

			DrawableEntities.Add(wall1);
			DrawableEntities.Add(wall2);
			DrawableEntities.Add(wall3);
			DrawableEntities.Add(wall4);
			DrawableEntities.Add(wall5);
			DrawableEntities.Add(wall6);
			DrawableEntities.Add(wall7);
			DrawableEntities.Add(wall8);
			//DrawableEntities.Add(wall9);
			//DrawableEntities.Add(wall10);
			DrawableEntities.Add(wall11);
			DrawableEntities.Add(wall12);
			DrawableEntities.Add(wall13);
			DrawableEntities.Add(wall14);
			DrawableEntities.Add(wall15);
			DrawableEntities.Add(wall16);
			DrawableEntities.Add(wall17);
			DrawableEntities.Add(wall18);
		}

		private void InitRivers()
		{
			River river = new River(new Position(194, 192), 108, 36);

			rivers.Add(river);
			DrawableEntities.Add(river);
		}
	}
}
