using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{


    public abstract class Auto
    {
        public event EventHandler<DateTime> finish;

        public bool isFinished = false;
        protected int maxspeed;
        protected int minspeed;
        protected int distance;
        protected DateTime dateOfFinish = new DateTime();
        public void setdistance(int distance)
        {
            this.distance = distance;
        }
        public Auto()
        {
            maxspeed = 100;
            minspeed = 50;
            distance = 1000;

        }
        public virtual void Start(object sender, EventArgs e)
        {
            Console.WriteLine("Auto starting");
        }
        public void go(object sender, EventArgs e)
        {
            if (distance > 0)
            {
                Random r = new Random();
                distance -= r.Next(minspeed, maxspeed);
                Console.WriteLine(this.ToString() + " need to pass " + distance + " more!");
            }
            else
            {
                dateOfFinish = DateTime.Now;
                finish(this, dateOfFinish);
                isFinished = true;
            }
        }
        public class Car : Auto
        {
            public Car()
                : base()
            {
                maxspeed = 150;
                minspeed = 100;
            }
            public override void Start(object sender, EventArgs e)
            {
                Console.WriteLine("Car starting!");
            }


        }
        public class RaceCar : Auto
        {
            public RaceCar()
                : base()
            {
                maxspeed = 200;
                minspeed = 100;
            }
            public override void Start(object sender, EventArgs e)
            {
                Console.WriteLine("RaceCar starting!");
            }
        }
        public class Bike : Auto
        {
            public Bike()
                : base()
            {
                maxspeed = 300;
                minspeed = 50;
            }
            public override void Start(object sender, EventArgs e)
            {
                Console.WriteLine("Bike starting!");
            }
        }
        class Program
        {
            static int countFinished = 0;
            public static void OnFinishHandler(object car, DateTime time)
            {
                Console.WriteLine(car.ToString() + " finished at " + time.ToString());
                countFinished++;


            }
            public delegate void set(int i);
            public static event set setOn;
            public static event EventHandler start;
            public static event EventHandler Go;
            static void Main(string[] args)
            {
                Auto car1 = new Car();
                Auto car2 = new Car();
                Auto raceCar = new RaceCar();
                Auto bike = new Bike();
                setOn += car1.setdistance;
                setOn += car2.setdistance;
                setOn += raceCar.setdistance;
                setOn += bike.setdistance;
                Console.WriteLine("set the distance ");
                int distance = Convert.ToInt32(Console.ReadLine());
                setOn(distance);
                start += car1.Start;
                start += car2.Start;
                start += raceCar.Start;
                start += bike.Start;
                car1.finish += OnFinishHandler;
                car2.finish += OnFinishHandler;
                bike.finish += OnFinishHandler;
                raceCar.finish += OnFinishHandler;
                Go += car1.go;
                Go += car2.go;
                Go += bike.go;
                Go += raceCar.go;
                List<Auto> list = new List<Auto>();
                list.Add(car1);
                list.Add(car2);
                list.Add(bike);
                list.Add(raceCar);
                List<Auto> FinishingList = new List<Auto>();
                if (start != null)
                {
                    start(null, EventArgs.Empty);
                }
                if (Go != null)
                {
                    do
                    {
                        Go(null, EventArgs.Empty);
                        Thread.Sleep(1000);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].isFinished == true)
                            {
                                list[i].finish -= OnFinishHandler;
                                Go -= list[i].go;
                                if(!FinishingList.Contains(list[i]))
                                FinishingList.Add(list[i]);
                            }
                        }
                    }
                    while (countFinished < list.Count);
                }
                Console.WriteLine("On finish: ");
                for (int i = 0; i < FinishingList.Count; i++)
                {
                    Console.WriteLine(FinishingList[i].ToString() + " finished at " + (i + 1) + " position at " + FinishingList[i].dateOfFinish);
                }


            }
        }
    }
}
