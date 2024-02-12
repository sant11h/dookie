using Microsoft.AspNetCore.SignalR;

class PositionHub : Hub
{
    private readonly ILogger<PositionHub> _logger;
    private float _x;
    private float _y;

    public PositionHub(ILogger<PositionHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("New client connected!");
        await Clients.Caller.SendAsync("UpdatePosition", _x, _y);
        await base.OnConnectedAsync();
    }

    public async Task Move(float x, float y)
    {
        _x = x;
        _y = y;

        this._logger.LogInformation("Position updated to {x},{y}", x, y);

        await Clients.All.SendAsync("UpdatePosition", _x, _y);
    }
}