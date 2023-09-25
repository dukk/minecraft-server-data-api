namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Configuration;

public class RconConfiguration
{
    public RconServerConfiguration[]? Servers { get; set; }

    public RconServerConfiguration GetServerByName(string name, bool throwIfMissingFields = false)
    {
        if (this.Servers is null)
            throw new ApplicationException("Missing Rcon:Servers configuration.");

        var server = this.Servers.First(s => (s.Name is not null) && s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (throwIfMissingFields) // dukk: this won't work like I wanted it to but I'm going to keep playing with it
        {
            if (server.Host is null)
                throw new ApplicationException($"Missing Rcon:Server:Host configuration for server name '{name}'.");

            if (server.Port is null)
                throw new ApplicationException($"Missing Rcon:Server:Port configuration for server name '{name}'.");

            if (server.Password is null)
                throw new ApplicationException($"Missing Rcon:Server:Password configuration for server name '{name}'.");
        }

        return server;
    }
}
