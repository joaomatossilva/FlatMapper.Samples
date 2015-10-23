using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using FlatMapper;

namespace Flatmapper.NestedClasses
{
    class Program
    {
        private static Layout<PersonFlat> _layout;
        private const string DataFile = "Data.txt";

        static void Main(string[] args)
        {
            /* Setup layout and mappings */
            SetupMappings();
            SetupLayout();

            /* this is only needed for this sample, to generate test data file*/
            CreateTestFile();

            /* do the actual read of the file */
            var persons = Read();
            foreach (var person in persons)
            {
                Console.WriteLine("Person {0}, Age {1}", person.Name, person.Age);
                Console.WriteLine("Lives in {0},{1} {2}", person.Address.Street, person.Address.City, person.Address.Number);
                Console.WriteLine("--");
            }
        }

        public static IEnumerable<Person> Read()
        {
            //read the flat entries for file..
            var flatEntries = ReadFile();
            //and then let automapper do the un-flattening
            return Mapper.Map<IEnumerable<Person>>(flatEntries);
        }

        private static IEnumerable<PersonFlat> ReadFile()
        {
            //using the flattmapper as the documentation
            using (var fileStream = File.OpenRead(DataFile))
            {
                var flatFile = new FlatFile<PersonFlat>(_layout, fileStream);
                return flatFile.Read()
                    //we need a ToList here since flatmapper does iteractive reading, 
                    //closing the stream before enumerating the collection will throw an error.
                    .ToList();
            }
        }

        private static void CreateTestFile()
        {
            var testData = new[]
            {
                new PersonFlat
                {
                    Age = 34,
                    Gender = "Male",
                    Name = "Joao",
                    Number = "21",
                    City = "Lisbon",
                    Street = "Main Street"
                },
                new PersonFlat
                {
                    Age = 33,
                    Gender = "Male",
                    Name = "Joao",
                    Number = "20",
                    City = "Lisbon",
                    Street = "Main Street"
                },
                new PersonFlat
                {
                    Age = 32,
                    Gender = "Male",
                    Name = "Joao",
                    Number = "19",
                    City = "Lisbon",
                    Street = "Main Street"
                }
            };

            using (var fileStream = File.OpenWrite(DataFile))
            {
                var flatFile = new FlatFile<PersonFlat>(_layout, fileStream);
                flatFile.Write(testData);
            }
        }

        private static void SetupLayout()
        {
            _layout = new Layout<PersonFlat>.DelimitedLayout()
                .WithDelimiter("|")
                .WithMember(m => m.Name, settings => { })
                .WithMember(m => m.Street, settings => { })
                .WithMember(m => m.Number, settings => { })
                .WithMember(m => m.City, settings => { })
                .WithMember(m => m.Age, settings => { })
                .WithMember(m => m.Gender, settings => { });
        }

        private static void SetupMappings()
        {
            Mapper.CreateMap<PersonFlat, Address>();
            Mapper.CreateMap<PersonFlat, Person>()
                .ForMember(m => m.Address, opt => opt.MapFrom(src => src));
        }
    }
}
