# Benchmarks (2025-10-09)
### System Info
```
BenchmarkDotNet v0.15.4, Windows 11 (10.0.26200.6584)
AMD Ryzen 7 PRO 5850U with Radeon Graphics 1.90GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.304
  [Host]     : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
  Job-XPUURG : .NET 8.0.19 (8.0.19, 8.0.1925.36514), X64 RyuJIT x86-64-v3
IterationCount=10  WarmupCount=5  
```
### `IRequest`

| Method              | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute |  92.07 ns | 10.652 ns | 7.045 ns |  1.01 |    0.10 | 0.0191 |     160 B |        1.00 |
| MediatorSendRequest | 162.72 ns |  5.510 ns | 3.644 ns |  1.78 |    0.13 | 0.0324 |     272 B |        1.70 |

### `IRequest + Interceptors[5]`

| Method              | Mean     | Error    | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 394.9 ns | 12.28 ns | 7.31 ns |  1.00 |    0.03 | 0.1106 |     928 B |        1.00 |
| MediatorSendRequest | 375.3 ns | 12.20 ns | 7.26 ns |  0.95 |    0.02 | 0.1240 |    1040 B |        1.12 |

### `IRequest<TResponse>`

| Method              | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 106.7 ns | 3.66 ns | 2.18 ns |  1.00 |    0.03 | 0.0305 |     256 B |        1.00 |
| MediatorSendRequest | 159.5 ns | 6.15 ns | 4.07 ns |  1.50 |    0.05 | 0.0439 |     368 B |        1.44 |

### `IRequest<TResponse> + Interceptors[5]`
| Method              | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 420.3 ns | 25.41 ns | 16.81 ns |  1.00 |    0.05 | 0.1221 |      1 KB |        1.00 |
| MediatorSendRequest | 333.8 ns | 15.85 ns | 10.48 ns |  0.80 |    0.04 | 0.1354 |   1.11 KB |        1.11 |


### `INotification + Handler[1]`
| Method             | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorNotify | 229.7 ns | 6.63 ns | 4.38 ns |  1.00 |    0.03 | 0.0544 |     456 B |        1.00 |
| MediatorPublish    | 150.4 ns | 3.89 ns | 2.31 ns |  0.66 |    0.02 | 0.0381 |     320 B |        0.70 |

### `INotification + Handler[5]`
| Method              | Mean     | Error    | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 351.5 ns |  9.12 ns | 6.03 ns |  1.00 |    0.02 | 0.0954 |     800 B |        1.00 |
| MediatorSendRequest | 380.2 ns | 15.48 ns | 9.21 ns |  1.08 |    0.03 | 0.1106 |     928 B |        1.16 |

