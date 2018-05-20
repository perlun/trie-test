# trie-test

A simple [trie](https://en.wikipedia.org/wiki/Trie) implementation using
C# and .NET Core.

The program (which is console-based and should work on any platform where
.NET Core runs) loads a list of cities from a text file. It then lets the
user type a search prefix, and searches for the 10 first cities matching
the search prefix.

There are two implementations of the search, which illustrates why tries
are useful for this kind of scenarios:

- A plain "array scan" approach which loops over the full array of cities
  on every search. Performance characteristics are `O(n)` where `n` is
  around 127 000 in this case.
- A trie-based approach, which navigates through the trie based on the
  user-provided filter. Performance characteristics are `O(m)` where `m` is
  the length of the filter.

## Benchmarks

Hard facts speak most clearly in this case, so here are a few sample runs
using the array scan approach (of course, to get better data on this the
tests should be run many more times and the average be calculated; this is
just to give a rough illustration of the performance characteristics of
these algorithms):

```
Search for kniv took $00:00:00.2871540
Search for stock took $00:00:00.1209150
Search for vanc took $00:00:00.2784010
Search for kniv took $00:00:00.2752800
Search for stock took $00:00:00.1161210
Search for vanc took $00:00:00.2775150
Search for kniv took $00:00:00.2757990
Search for stock took $00:00:00.1161600
Search for vanc took $00:00:00.2739880
Search for kniv took $00:00:00.2753880
Search for stock took $00:00:00.1171500
Search for vanc took $00:00:00.2799750
Search for kniv took $00:00:00.2766980
Search for stock took $00:00:00.1163990
Search for vanc took $00:00:00.2754160
Search for kniv took $00:00:00.2792080
Search for stock took $00:00:00.1184880
Search for vanc took $00:00:00.2724220
Search for kniv took $00:00:00.2738530
Search for stock took $00:00:00.1166990
Search for vanc took $00:00:00.2745610
Search for kniv took $00:00:00.2762910
Search for stock took $00:00:00.1204550
Search for vanc took $00:00:00.2800050
Search for kniv took $00:00:00.2782420
Search for stock took $00:00:00.1166420
Search for vanc took $00:00:00.2743970
Search for kniv took $00:00:00.2763690
Search for stock took $00:00:00.1251960
Search for vanc took $00:00:00.2744810
```

And then the same using the trie approach:

```
Search for kniv took $00:00:00.0045630
Search for stock took $00:00:00.0000950
Search for vanc took $00:00:00.0000590
Search for kniv took $00:00:00.0000050
Search for stock took $00:00:00.0000220
Search for vanc took $00:00:00.0000040
Search for kniv took $00:00:00.0000010
Search for stock took $00:00:00.0000090
Search for vanc took $00:00:00.0000020
Search for kniv took $00:00:00.0000020
Search for stock took $00:00:00.0000080
Search for vanc took $00:00:00.0000030
Search for kniv took $00:00:00.0000020
Search for stock took $00:00:00.0000080
Search for vanc took $00:00:00.0000020
Search for kniv took $00:00:00.0000010
Search for stock took $00:00:00.0000080
Search for vanc took $00:00:00.0000030
Search for kniv took $00:00:00.0000010
Search for stock took $00:00:00.0000130
Search for vanc took $00:00:00.0000030
Search for kniv took $00:00:00.0000020
Search for stock took $00:00:00.0000080
Search for vanc took $00:00:00.0000030
Search for kniv took $00:00:00.0000020
Search for stock took $00:00:00.0000080
Search for vanc took $00:00:00.0000080
Search for kniv took $00:00:00.0000010
Search for stock took $00:00:00.0000080
Search for vanc took $00:00:00.0000030
```

As can be seen, the trie-based approach is typically is multiple orders of
magnitudes faster - the above figures show about 43 000 times faster for
the trie implementation when excluding the three "slow ones" in the
beginning. These presumably were because JIT compilation/optimization,
caching etc. was taking place. If we allow these slow runs to pollute the
data, the trie implementation is still about 1400 times faster than the
array scanning one.

## Conclusions

(Also known as _"Why does this really matter?"_)

I don't know about you, but I personally feel rather fed up using
inefficient, high-latency software. I feel that typically, too little care
is spent by companies and individual engineers alike on thinking about the
performance implications of software engineering design choices.

Using the proper algorithms is of course only _one_ of the things that
makes your program run fast, but it's an important one.

I believe that if we all cared only 10% more about these things, the world
would be a better place. The carbon dioxide emissions would also likely be
significantly decreased, since modern computer hardware typically adjust
the power usage heavily based on CPU usage.

Faster algorithms => less power being used => lower emissions => a better,
more sustainable future for ourselves, our children and grand-children.

(Not to mention that _fast, low-latency software is much nicer to use_ than
sluggish, high-latency software. Your users, including me, will be much
happier if your next program runs blazing fast.)

## Acknowledgements

- `cities.txt` is based on `cities1000.txt` from
  [Geonames](http://download.geonames.org/export/dump/). I extracted the
  city names to a separate file for easy processing using the following
  `awk` script:

  ```shell
  $ cat cities1000.txt | awk -F '\t' '{ print $2 }' > cities.txt
  ```
