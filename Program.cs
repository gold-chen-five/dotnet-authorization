using Api;

var server = Server.NewServer(args);
server.SetupMiddleware();
server.Start();