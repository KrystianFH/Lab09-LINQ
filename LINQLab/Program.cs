using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;



namespace LINQLab
{
    delegate void FindNeighborhoods(int number);
    class Program
    {
        static void Main(string[] args)
        {
            EvaluateJsonData();
        }

        static void EvaluateJsonData()
        {
             // find json file path, stringify
            string path = @"../../../../data.json";
            string jsonToString = File.ReadAllText(path);

            // make string readable
            Root root = JsonConvert.DeserializeObject<Root>(jsonToString);

            findAllNeighborhoods(root);
            filterAllNeighborhoodsMethod(root);
            filterNeighborhoodsWithNoName(root);
            filterOutDuplicateNeighborhoods(root);
            consolidateQueries(root);

        }

        // find all neighborhoods in json file, output is 147 neighborhoods
        public static void findAllNeighborhoods(Root root)
        {
            int count = 1;
            foreach (Feature feature in root.features)
            {
                Console.WriteLine($"{count}, {feature.properties.neighborhood}");
                count++;
            }
        }

        // opposing method for filterAllNeighborhoods
        public static void filterAllNeighborhoodsMethod(Root root)
        {
            int count = 1;
            var query = root.features
                        .Select(feature => new { feature.properties.neighborhood });

            foreach(var feature in query)
            {
                Console.WriteLine($"All Neighborhoods Method: {count}, {feature}");
                count++;
            }

        }

        // filter out neighborhoods with no name, should return 143 neighborhoods
        public static void filterNeighborhoodsWithNoName(Root root)
        {
            int count = 1;
            var query = from feature in root.features
                        where feature.properties.neighborhood != ""
                        select feature.properties.neighborhood;

            foreach (var feature in query)
            {
                Console.WriteLine($"{count}, {feature}");
                count++;
            }

        }

        // filter out duplicates, should return 39 neighborhoods
        public static void filterOutDuplicateNeighborhoods(Root root)
        {
            int count = 1;
            var query = from feature in root.features
                        where feature.properties.neighborhood != ""
                        select feature.properties.neighborhood;

            var removeDuplicates = query.Distinct(); // .Distinct() removes duplicates, referenced from https://www.dotnetperls.com/duplicates

            foreach (var feature in removeDuplicates)
            {
                Console.WriteLine($"{count}, {feature}");
                count++;
            }

        }

        // method to consolidate queries
        public static void consolidateQueries(Root root)
        {
            int count = 1;
            var query = (from feature in root.features
                         where (feature.properties.neighborhood != "")
                         select (feature.properties.neighborhood)).Distinct();

            foreach(var feature in query)
            {
                Console.WriteLine($"Consolidated Queries {count}, {feature}");
                count++;
            }
        }

        // IntelliSense recommendations below
        public class Root
        {
            public string type { get; set; }
            public List<Feature> features { get; set; }

        }

        public class Feature
        {
            public string type { get; set; }
            public Geometry geometry { get; set; }
            public Properties properties { get; set; }

        }

        public class Geometry
        {
            public string type { get; set; }
            public List<double> coordinates { get; set; }
        }

        public class Properties
        {
            public string zip { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string address { get; set; }
            public string borough { get; set; }
            public string neighborhood { get; set; }
            public string county { get; set; }
        }
    }
}
