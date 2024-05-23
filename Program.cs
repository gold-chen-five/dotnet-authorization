using Api;
using DotNetEnv;

Env.Load("app.env");

var server = ServerBuilder.NewServer(args);
server.Start();