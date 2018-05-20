# trie-test

A simple [trie](https://en.wikipedia.org/wiki/Trie) implementation using C# and .NET Core.

## Acknowledgements

- `cities.txt` is based on `cities1000.txt` from [Geonames](http://download.geonames.org/export/dump/).
  I extracted the city names to a separate file for easy processing using
  the following `awk` script:

  ```shell
  $ cat cities1000.txt | awk -F '\t' '{ print $2 }' > cities.txt
  ```
