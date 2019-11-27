using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Mail;
using SecretSanta;

namespace SecretSantaConsole
{
    class Program
    {
        private const char DictFileSeparator = ',';
        private const string OutputDictSeparator = " -> ";

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting...");
                CreateSantasList(args);
                Console.WriteLine("Complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured: {0}", ex);
            }
            finally
            {
                Console.WriteLine("Press enter to quit.");
                Console.ReadLine();
            }
        }

        private static void CreateSantasList(string[] args)
        {
            var participants = ReadPeople(args[0]);

            IDictionary<Person, Person> bannedPairs = new Dictionary<Person, Person>();

            string outputFile;

            switch (args.Length)
            {
                case 2:
                    outputFile = args[1];
                    break;

                case 3:
                    bannedPairs = ReadDictFile(args[1]);
                    outputFile = args[2];
                    break;

                default:
                    throw new ArgumentException("Invalid number of arguments passed");
            }

            var santasList = SecretSantaGenerator.Generate(participants, bannedPairs);

            using (var smtp = new SmtpClient("mailserver.ttint.com"))
            {
                foreach (var s in santasList)
                {
                    Console.WriteLine(s.Key + "->" + s.Value);
                    using (var mail = new MailMessage("shawd@ttint.com", s.Key.Email))
                    {
                        mail.Sender = new MailAddress(s.Key.Email, s.Key.Name);
                        mail.Subject = "Your secret santa!";
                        mail.Body = "You, " + s.Key.Name + ", have to buy a book for " + s.Value.Name + "!";

                        smtp.Send(mail);
                    }
                }
            }

            WriteDictFile(outputFile, santasList);
        }

        private static List<string> ReadFile(string filePath)
        {
            return File.ReadLines(filePath).Select(record => record.Trim()).ToList();
        }

        private static IList<Person> ReadPeople(string filePath)
        {
            var people = new List<Person>();

            foreach (var record in ReadFile(filePath))
            {
                var splitRecord = record.Split(DictFileSeparator);
                people.Add(new Person(splitRecord[0].Trim(), splitRecord[1].Trim()));
            }

            return people;
        }

        private static IDictionary<Person, Person> ReadDictFile(string filePath)
        {
            var dict = new Dictionary<Person, Person>();

            foreach (var record in ReadFile(filePath))
            {
                var splitRecord = record.Split(DictFileSeparator);
                dict.Add(new Person(splitRecord[0].Trim(), ""), new Person(splitRecord[1].Trim(), ""));
            }

            return dict;
        }

        private static void WriteDictFile(string filePath, IDictionary<Person, Person> recordPairs)
        {
            var records = recordPairs.Select(pair => string.Concat(pair.Key.ToString(), OutputDictSeparator, pair.Value.ToString()));

            File.WriteAllLines(filePath, records);
        }
    }
}
