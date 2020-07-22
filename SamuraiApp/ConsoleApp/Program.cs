using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;

namespace ConsoleApp
{
    class Program
    {
        private static SamuraiContext context = new SamuraiContext();
        static void Main(string[] args)
        {
            //AddSamurai();
            InsertMultipleSamurais();
            InsertVariousTypes();
            Console.WriteLine("Hello World!");
        }

        private static void InsertVariousTypes()
        {
            var samurai = new Samurai { Name = "KiChoRex" };
            var clan = new Clan { ClanName = "Imperial Clan" };
            context.AddRange(samurai, clan);
            context.SaveChanges();
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "Rex" };
            var samurai2 = new Samurai { Name = "Bob" };
            var samurai3 = new Samurai { Name = "Bill" };
            var samurai4 = new Samurai { Name = "Beer" };
            // can also pass as range a List<Samurai>
            context.Samurais.AddRange(samurai, samurai2, samurai3, samurai4);
            context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Jim" };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }
    }
}
