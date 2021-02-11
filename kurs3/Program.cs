using System;
using System.Linq;
using System.Security.Cryptography;
using static System.Text.Encoding;
using Convert = System.Convert;

namespace Kurs3
{
    class Program
    {
        static string[] Winners(string[] args, int user)
        {
            var winners = new string[(args.Length-1)/2];

            var index = 0;
            for (int i = 0; i < ((args.Length-1)/2); i++)
            {
                winners[index] = args[((user) + i) % args.Length];
                index++;
            }
            
            return winners;
        }
        static bool UniqueElems(string[] args)
        {
            if (args.Length >= 3 && args.Length % 2 != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    for (int j = i; j < args.Length; j++)
                    {
                        if (i != j && args[i] == args[j])
                            return false;
                    }
                }
                return true;
            }
            return false;
        }
        static void Main(string[] args)
        {
            if (!UniqueElems(args) || args.Length < 3)
                throw new Exception("Error");
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[16];
            RandomNumberGenerator.Create().GetBytes(buff);
            var hmacsha256 = new HMACSHA256(buff);
            var computer = args[RandomNumberGenerator.GetInt32(0, args.Length - 1)];
            var hash = Convert.ToHexString(hmacsha256.ComputeHash(ASCII.GetBytes(computer)));
            Console.WriteLine("HMAC:" + hash);
            
            Console.WriteLine("Available Moves: ");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i+1} - {args[i]}");
            }
            Console.Write($"{0} - Exit \nEnter your move: ");
            int user = int.Parse(Console.ReadLine());
            Console.WriteLine($"Your move: {args[user-1]}\nComputer move: {computer}");
            
            if(computer == args[user-1])
                Console.WriteLine("Draw");

            if(!Winners(args, user).Contains(computer))
                Console.WriteLine("You Win!");
            else
                Console.WriteLine("You Lose!");
            Console.WriteLine($"HMAC Key: {Convert.ToHexString(buff)}");
        }
    }
}
