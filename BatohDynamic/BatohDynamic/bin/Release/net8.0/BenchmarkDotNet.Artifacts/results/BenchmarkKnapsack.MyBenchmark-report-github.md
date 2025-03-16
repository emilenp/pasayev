```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
Intel Core i5-9300H CPU 2.40GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.401
  [Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2


```
| Method                      | Mean       | Error    | StdDev    | Gen0   | Allocated |
|---------------------------- |-----------:|---------:|----------:|-------:|----------:|
| Knapsack_Backtracking       | 4,339.6 ns | 86.58 ns | 212.38 ns | 1.5488 |    6504 B |
| Knapsack_DynamicProgramming |   359.4 ns |  6.90 ns |   7.94 ns | 0.0725 |     304 B |
