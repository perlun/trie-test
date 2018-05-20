using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrieTest {
    class Program {
        static string[] cities;
        static TrieNode trieRoot = new TrieNode {
            NextNodes = new Dictionary<char, TrieNode>()
        };

        static void Main(string[] args) {
            cities = Benchmark<string[]>(
                () => File.ReadAllLines("cities.txt"),
                "Reading cities file"
            );

            Benchmark(GenerateCitiesTrie, "Building cities trie");
            Console.WriteLine("Initialized. Enter a search string to try me out.");

            while (true) {
                var s = Console.ReadLine().ToLower();

                var matchingCities = Benchmark(
                    () => FindMatchingCitiesTrie(s),
                    "Search"
                );

                foreach (var city in matchingCities) {
                    Console.WriteLine(city);
                }

                Console.WriteLine();
            }
        }

        private static void GenerateCitiesTrie() {
            foreach (var city in cities) {
                var currentNode = trieRoot;
                var lowercasedCity = city.ToLower();

                for (int i = 0; i < city.Length; i++) {
                    var c = lowercasedCity[i];

                    var nodeNotPresent = !currentNode.NextNodes.ContainsKey(c); //currentNode.NextNodes[c].Equals(default(TrieNode));
                    if (nodeNotPresent) {
                        currentNode.NextNodes[c] = new TrieNode {
                            NextNodes = new Dictionary<char, TrieNode>(),

                            // If we have reached the last node in the
                            // trie, we are a leaf node => store the
                            // actual name in this case.
                            Name = (i == city.Length - 1)
                                ? city
                                : null
                        };
                    }

                    currentNode = currentNode.NextNodes[c];
                }

                // The last node represents all characters in the name.
                currentNode.Name = city;
            }
        }

        const int MATCHING_CITIES_MAX = 10;

        // Naive, O(n) search of a plain array. Takes from about 13 ms
        // if you're lucky down to worst-case of 280 ms on a 127 000 record
        // file. Since it takes the first 10 matching results, it is much
        // faster for common name prefixes.
        private static string[] FindMatchingCitiesArrayScan(string filter) =>
            cities.Where(c => c.StartsWith(filter)).Take(MATCHING_CITIES_MAX).ToArray();

        // More optimized O(m) approach using a trie (m = length of filter.)
        // Worst-case about 5 ms on my machine, typically runs at a
        // fraction of a millisecond.
        private static string[] FindMatchingCitiesTrie(string filter) {
            // Find the node that matches the filter.
            var matchingNode = trieRoot;

            for (int i = 0; i < filter.Length; i++) {
               var c = filter[i];
                matchingNode = matchingNode.NextNodes[c];

                if (matchingNode.Equals(default(TrieNode))) {
                    // This filter does not exist in the trie => empty
                    // result.
                    return new string[0];
                }
            }

            // Depth-first or breadth-first? It depends on which one
            // produces the best search result. I used a simple
            // "trial-and-error" approach, implementing both (using
            // a Stack (i.e. LIFO) and a Queue (i.e. FIFO). The Queue
            // results felt somewhat more natural => go for that.
            var nodesToVisit = new Queue<TrieNode>();
            nodesToVisit.Enqueue(matchingNode);

            var result = new List<string>();

            while (nodesToVisit.Count() > 0 &&
                result.Count < MATCHING_CITIES_MAX) {

                var currentNode = nodesToVisit.Dequeue();

                // Dig further down in the trie
                foreach (var childNode in currentNode.NextNodes.Values) {
                    nodesToVisit.Enqueue(childNode);
                }

                if (currentNode.IsLeaf) {
                    result.Add(currentNode.Name);
                }
            }

            return result.ToArray();
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
