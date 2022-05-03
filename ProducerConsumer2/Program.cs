using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {
        static Queue<int> products = new Queue<int>();
        static void Main(string[] args)
        {
            Thread t1 = new Thread(Produce);
            Thread t2 = new Thread(Consume);

            t1.Name = "Producer";
            t2.Name = "Consumer";

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }

        static void Consume()
        {
            while (true)
            {
                lock (products)
                {
                    while (products.Count == 0)
                    {
                        Monitor.PulseAll(products);
                        Monitor.Wait(products);
                    }
                    products.Dequeue();
                    Console.WriteLine(Thread.CurrentThread.Name + " consumed: " + products.Count);
                }
            }
        }
        static void Produce()
        {
            while (true)
            {
                lock (products)
                {
                    if (products.Count < 3)
                    {
                        products.Enqueue(1);
                        Console.WriteLine(Thread.CurrentThread.Name + " produced: " + products.Count);
                    }
                    else
                    {
                        Console.WriteLine(Thread.CurrentThread.Name + " waits.....");
                        Monitor.PulseAll(products);
                        Monitor.Wait(products);
                    }
                }
            }
        }
    }
}

