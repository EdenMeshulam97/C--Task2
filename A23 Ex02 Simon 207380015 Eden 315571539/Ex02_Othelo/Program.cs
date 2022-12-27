using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    public class Program
    {
        private static void Main()
        {
           GameManeger manager = new GameManeger();
            manager.StartGmae();
            Console.ReadLine();
        } 
    }
}
