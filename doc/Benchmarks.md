# Benchmarks

## 2025-10-07
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

| Method              | Mean     | Error    | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 107.4 ns | 10.96 ns | 7.25 ns |  1.00 |    0.10 | 0.0324 |     272 B |        1.00 |
| MediatorSendRequest | 231.8 ns | 14.81 ns | 8.81 ns |  2.17 |    0.17 | 0.0324 |     272 B |        1.00 |

### `IRequest + Interceptors[5]`

| Method              | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 437.1 ns | 25.24 ns | 16.69 ns |  1.00 |    0.05 | 0.1240 |   1.02 KB |        1.00 |
| MediatorSendRequest | 499.4 ns | 65.28 ns | 38.85 ns |  1.14 |    0.10 | 0.1240 |   1.02 KB |        1.00 |

### `IRequest<TResponse>`

| Method              | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 130.8 ns |  9.48 ns |  5.64 ns |  1.00 |    0.06 | 0.0440 |     368 B |        1.00 |
| MediatorSendRequest | 176.7 ns | 16.71 ns | 11.05 ns |  1.35 |    0.10 | 0.0439 |     368 B |        1.00 |

### `IRequest<TResponse> + Interceptors[5]`

| Method              | Mean     | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorExecute | 456.5 ns |  26.15 ns | 15.56 ns |  1.00 |    0.05 | 0.1354 |   1.11 KB |        1.00 |
| MediatorSendRequest | 525.3 ns | 106.02 ns | 70.13 ns |  1.15 |    0.15 | 0.1354 |   1.11 KB |        1.00 |

### `INotification + Handler[1]`
| Method              | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorNotify  | 202.4 ns | 19.45 ns | 11.57 ns |  1.00 |    0.08 | 0.0668 |     560 B |        1.00 |
| MediatorPublish     | 262.2 ns | 87.64 ns | 57.97 ns |  1.30 |    0.28 | 0.0381 |     320 B |        0.57 |

### `INotification + Handler[5]`
| Method              | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| OrchestratorNotify  | 231.6 ns | 19.17 ns | 12.68 ns |  1.00 |    0.08 | 0.0811 |     680 B |        1.00 |
| MediatorPublish     | 401.9 ns | 24.62 ns | 16.28 ns |  1.74 |    0.12 | 0.1106 |     928 B |        1.36 |