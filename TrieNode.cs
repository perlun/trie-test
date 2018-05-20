using System.Collections.Generic;

namespace TrieTest {
    struct TrieNode {
        public string Name;

        // A dictionary for all names one character longer than prefix.
        // Example: if prefix is 'van', this dictionary will contain
        // 'vana', 'vanb', 'vand' etc. for all letters where there are
        // names matching that prefix.
        public Dictionary<char, TrieNode> NextNodes;

        public bool IsLeaf {
            get => Name != null;
        }
    }
}
