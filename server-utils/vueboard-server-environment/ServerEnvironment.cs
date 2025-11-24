namespace Vueboard.Server.Environment;

public class ServerEnvironment : IServerEnvironment
{
  public ServerEnvironmentType EnvironmentType { get; set; }

  public static IServerEnvironment FromEnvironmentVariables()
  {
    var env = new ServerEnvironment();

    var envTypeString = System.Environment.GetEnvironmentVariable("VUEBOARD_SERVER_ENVIRONMENT");
    if (String.IsNullOrWhiteSpace(envTypeString)) envTypeString = "Development";
    env.EnvironmentType = Enum.Parse<ServerEnvironmentType>(envTypeString);

    return env;
  }
}
