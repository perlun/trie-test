using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrieTest {
    struct TrieNode {
        // Is the prefix a name itself?
        public bool IsName;

        // A dictionary for all names one character longer than prefix.
        // Example: if prefix is 'van', this dictionary will contain
        // 'vana', 'vanb', 'vand' etc. for all letters where there are
        // names matching that prefix.
        public Dictionary<string, TrieNode> NextNodes;
    }

    class Program {
        static string[] cities;
        static Dictionary<string, TrieNode> trieRoots =
            new Dictionary<string, TrieNode>();

        static void Main(string[] args) {
            cities = Benchmark<string[]>(
                () => File.ReadAllLines("cities.txt"),
                "Reading cities file"
            );

            Benchmark(() => {
                foreach (var city in cities) {
                    if (city.Length < 3) {
                        // TODO: could consider properly supporting these
                        // extremely-short city names. The whole prefix
                        // thing might be a bit of an overengineering feat.
                        continue;
                    }

                    var prefix = city.Substring(0, 3);

                    if (!trieRoots.ContainsKey(prefix)) {
                        trieRoots[prefix] = new TrieNode {
                            IsName = city == prefix,
                            NextNodes = new Dictionary<string, TrieNode>()
                        };
                    }

                    var currentNode = trieRoots[prefix];
                    for (int i = 3; i < city.Length; i++) {
                        var c = city[i];

                        //currentNode
                        throw new NotImplementedException();
                    }
                }
            }, "Building cities trie");

            Console.WriteLine("Initialized. Enter a search string to try me out.");

            while (true) {
                var s = Console.ReadLine();

                var matchingCities = Benchmark(
                    () => FindMatchingCitiesTrie(s),
                    "Search"
                );

                foreach (var city in matchingCities) {
                    Console.WriteLine(city);
                }


                Console.WriteLine(s);
            }
        }

        // Naive, O(n) search of a plain array. Takes from about 13 ms
        // if you're lucky down to worst-case of 280 ms on a 127 000 record
        // file. Since it takes the first 10 matching results, it is much
        // faster for common name prefixes.
        private static string[] FindMatchingCitiesArrayScan(string s) =>
            cities.Where(c => c.StartsWith(s)).Take(10).ToArray();

        private static string[] FindMatchingCitiesTrie(string s) {
            // The trie is constructed with a minimum token prefix length
            //
            if (s.Length < 3) {
                return new string[0];
            }

            var prefix = s.Substring(0, 3);

            throw new NotImplementedException();
        }

        private static T Benchmark<T>(Func<T> callback, string name) {
            var before = DateTime.Now;
            var result = callback();
            var after = DateTime.Now;
            Console.WriteLine($"{name} took ${after - before}");

            return result;
        }

        private static void Benchmark(Action callback, string name) {
            var before = DateTime.Now;
            callback();
            var after = DateTime.Now;
            Console.WriteLine($"{name} took ${after - before}");
        }
    }
}
