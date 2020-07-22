using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            //AddSamurai();
            //InsertMultipleSamurais();
            //InsertVariousTypes();
            //GetSamuraisSimpler();
            //QueryFilters();
            RetrieveAndUpdateSamurai();
            RetrieveAndUpdateMultipleSamurais();
            Console.WriteLine("Hello World!");
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(3).ToList();
            samurais.ForEach(s => s.Name += " the 1st");
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "B%")).ToList();
            foreach (Samurai s in samurais)
            {
                s.Name += " San";
            }
            _context.SaveChanges();
        }

        private static void QueryFilters()
        {
            //var samurais = _context.Samurais.Where(s => s.Name == "Bob").ToList();
            var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "%B%")).ToList();
            //var samurais = _context.Samurais.Where(s => s.Name.Contains("B")).ToList();
            foreach (Samurai s in samurais)
            {
                Console.WriteLine($"Found: {s.Name} with ID: {s.Id}");
            }
            
        }

        private static void GetSamuraisSimpler()
        {
            //var samurais = context.Samurais.ToList();
            var query = _context.Samurais;
            //var samurais = query.ToList();
            foreach (var samurai in query)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void InsertVariousTypes()
        {
            var samurai = new Samurai { Name = "KiChoRex" };
            var clan = new Clan { ClanName = "Imperial Clan" };
            _context.AddRange(samurai, clan);
            _context.SaveChanges();
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "Rex" };
            var samurai2 = new Samurai { Name = "Bob" };
            var samurai3 = new Samurai { Name = "Bill" };
            var samurai4 = new Samurai { Name = "Beer" };
            // can also pass as range a List<Samurai>
            _context.Samurais.AddRange(samurai, samurai2, samurai3, samurai4);
            _context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Jim" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }


    }
}
