using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
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
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //RetrieveAndDeleteSamurai();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //InsertNewSamuraiWithQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(27);
            //AddQuoteToExistingSamuraiNotTracked_Easy(27);
            //EagerLoadSamuraiWithQuotes();
            //EagerLoadSamuraiWithQuotesFiltered();
            //ProjectSomeProperties();
            ProjectSamuraiWithQuotes();
            Console.WriteLine("Hello World!");
        }

        private static void ProjectSamuraiWithQuotes()
        {
            //var somePropertiesWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, s.Quotes})
            //    .ToList();
            //var somePropertiesWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, 
            //        HappyQuotes = s.Quotes.Where(q=>q.Text.Contains("happy")) })
            //    .ToList();
            var somePropertiesWithQuotes = _context.Samurais
                .Select(s => new
                {
                    Samurai = s,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                })
                .ToList();
            foreach (var item in somePropertiesWithQuotes)
            {
                foreach (var quote in item.HappyQuotes)
                {
                    Console.WriteLine($"Select samurai: {item.Samurai.Id} {item.Samurai.Name}: {quote.Text}");
                }
                
            }
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idsAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
            foreach (var combo in someProperties)
            {
                Console.WriteLine($"Anonymous object: {combo.Id} {combo.Name}");
            }
            Console.WriteLine("------------------");
            foreach (var item in idsAndNames)
            {
                Console.WriteLine($"Struct: {item.Id} {item.Name}");
            }
        }

        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }

        private static void EagerLoadSamuraiWithQuotesFiltered()
        {
            // Include is a method for DbSet not other objects like Samurai, on Samurai it won't work
            // Eager also loads the entire set
            var samuraiWithQuotes = _context.Samurais.Where(s => s.Name.Contains("Ky"))
                                                     .Include(s => s.Quotes).ToList();
            foreach (Samurai samurai in samuraiWithQuotes)
            {
                foreach (Quote quote in samurai.Quotes)
                {
                    Console.WriteLine($"{samurai.Name}: {quote.Text}");
                }
            }
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            foreach (Samurai samurai in samuraiWithQuotes)
            {
                foreach (Quote quote in samurai.Quotes)
                {
                    Console.WriteLine($"{samurai.Name}: {quote.Text}");
                }
            }
        }

        private static void AddQuoteToExistingSamuraiNotTracked_Easy(int samuraiId)
        {
            var quote = new Quote
            {
                Text = "Now that I saved you, will you feed me dinner again?",
                SamuraiId = samuraiId
            };
            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote); 
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                //newContext.Samurais.Update(samurai); // Update works on ALL items in the table
                newContext.Samurais.Attach(samurai); // attach works only on one
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Kambei Shimada");
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
               {
                   new Quote { Text = "Watch out for my sharp sword!" },
                   new Quote { Text = "I told you to watch out for the sharp sword! Oh well!" }
               }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "I've come to save you."}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle
            {
                Name = "Battle of Okehazma",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });
            _context.SaveChanges();
        }

        private static void RetrieveAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(26);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
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
