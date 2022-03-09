using System;
using System.Collections.Generic;

namespace Aquarium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium();
            Player player = new Player(aquarium);

            while (player.IsActive)
            {
                aquarium.ShowInfo();
                Console.WriteLine("\n\n\n");
                player.ShowInfo();

                Console.WriteLine($"\n\n\nВыберите действие:" +
                                  $"\n\n{(int)MenuCommand.PutFish}. {MenuCommand.PutFish}" +
                                  $"\n{(int)MenuCommand.TakeFish}. {MenuCommand.TakeFish}" +
                                  $"\n{(int)MenuCommand.Skip}. {MenuCommand.Skip}");
                MenuCommand command = (MenuCommand)(GetNumber(Console.ReadLine()));

                switch (command)
                {
                    case MenuCommand.PutFish:
                        player.PutFish();
                        break;
                    case MenuCommand.TakeFish:
                        player.TakeFish();
                        break;
                    case MenuCommand.Skip:
                        player.SkipYear();
                        break;
                }

                Console.ReadKey(true);
                Console.Clear();
            }

            Console.WriteLine("Аквариум пустует ...");
        }

        public static int GetNumber(string numberText)
        {
            int number;

            while (int.TryParse(numberText, out number) == false)
            {
                Console.WriteLine("Повторите попытку:");
                numberText = Console.ReadLine();
            }

            return number;
        }
    }

    enum MenuCommand
    {
        PutFish = 1,
        TakeFish,
        Skip
    }

    class Player : IChangeYear, IShowInfo
    {
        private Aquarium _aquarium = new Aquarium();
        private List<Fish> _fishs = new List<Fish>();

        public bool IsActive => _fishs.Count > 0 || _aquarium.GetCountFish() > 0;

        public Player(Aquarium aquarium, List<Fish> fishs = null)
        {
            _aquarium = aquarium;

            if (fishs == null)
            {
                SetDefaultFishList();
            }
        }
        
        public void PutFish()
        {
            Console.Clear();

            if (_fishs.Count > 0)
            {
                ShowInfo();
                int index = GetIndexFish();
                Fish fish = _fishs[index];
                _fishs.RemoveAt(index);
                _aquarium.AddFish(fish);
                Console.WriteLine("Вы положили в аквариум рыбку");
            }
            else
            {
                Console.WriteLine("Рыбок нет");
            }
        }

        public void TakeFish()
        {
            Console.Clear();

            if (_aquarium.GetCountFish() > 0)
            {
                _aquarium.ShowInfo();
                int index = GetIndexFish(_aquarium.GetCountFish());
                _fishs.Add(_aquarium.GetFish(index));
                Console.WriteLine("Вы взяли рыбку");
            }
            else
            {
                Console.WriteLine("В аквариуме пусто");
            }
        }

        public void SkipYear()
        {
            _aquarium.SkipYear();

            for (int i = 0; i < _fishs.Count; i++)
            {
                _fishs[i].SkipYear();

                if (_fishs[i].IsAlive == false)
                {
                    _fishs.Remove(_fishs[i]);
                }
            }
        }

        public void ShowInfo()
        {
            Console.WriteLine("Рыбки у игрока:");

            for (int i = 0; i < _fishs.Count; i++)
            {
                Console.Write(i + 1 + ". ");
                _fishs[i].ShowInfo();
            }
        }

        private int GetIndexFish(int count = -1)
        {
            if (count == -1)
            {
                count = _fishs.Count;
            }

            int index;

            do
            {
                index = Program.GetNumber(Console.ReadLine()) - 1;
            } while (index < 0 || index >= _fishs.Count);

            return index;
        }

        private void SetDefaultFishList()
        {
            Random random = new Random();
            int availableColor = 7;
            int maximumFish = 15;
            int minimumFish = 7;
            int maximumHealthFish = 10;
            int minimumHealthFish = 5;

            for (int i = 0; i < random.Next(minimumFish, maximumFish); i++)
            {
                _fishs.Add(new Fish($"Рыбка_{i + 1}", random.Next(minimumHealthFish, maximumHealthFish), (Color)random.Next(0, availableColor)));
            }
        }
    }

    class Aquarium : IChangeYear, IShowInfo
    {
        private List<Fish> _fishs = new List<Fish>();

        public void AddFish(Fish fish)
        {
            _fishs.Add(fish);
        }

        public Fish GetFish(int index)
        {
            Fish fish = _fishs[index];
            _fishs.RemoveAt(index);

            return fish;
        }

        public void SkipYear()
        {
            for (int i = 0; i < _fishs.Count; i++)
            {
                _fishs[i].SkipYear();

                if (_fishs[i].IsAlive == false)
                {
                    _fishs.Remove(_fishs[i]);
                }
            }
        }

        public int GetCountFish() => _fishs.Count;

        public void ShowInfo()
        {
            Console.WriteLine("Рыбки в аквариуме:");

            for (int i = 0; i < _fishs.Count; i++)
            {
                Console.Write(i + 1 + ". ");
                _fishs[i].ShowInfo();
            }
        }
    }

    enum Color
    {
        Red,
        Green,
        Blue,
        Brown,
        Grey,
        Black,
        Pink
    }

    interface IChangeYear
    {
        public void SkipYear();
    }

    interface IShowInfo
    {
        public void ShowInfo();
    }

    class Fish : IChangeYear, IShowInfo
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public Color Color { get; private set; }
        public bool IsAlive { get; private set; }

        public Fish(string name, int health, Color color)
        {
            Name = name;
            Health = health;
            Color = color;
            IsAlive = true;
        }

        public void SkipYear()
        {
            --Health;

            if (Health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Console.WriteLine("Умерла рыбка:");
            ShowInfo();
            IsAlive = false;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя - {Name} | Жизней - {Health} | Цвет - {Color}");
        }
    }
}