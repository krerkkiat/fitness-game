# Socket Base Services

## Dependenceis

- Python 3.11.3
- Protoc (Protocol Buffer Compiler)

```console
protoc -I../Protos --python_out=. --pyi_out=. --grpc_python_out=. ../Protos/fitness.proto
```

## Note

Why not use gRPC at this point? We can't. The problem is that UWP 10.0 does not have `System.Net.Http.SocketsHttpHandler`
(see issue [#1915](https://github.com/grpc/grpc-dotnet/issues/1915)).
