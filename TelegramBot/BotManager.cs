using Microsoft.Extensions.Logging;
using TelegramBot.BotNetwork;

namespace TelegramBot
{
    public sealed class BotManager
    {
        private readonly BotMessageHandler _handler;
        private readonly BotMessageReceiver _receiver;
        private readonly ILogger<BotManager> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public BotManager(ILogger<BotManager> logger)
        {
            _handler = new BotMessageHandler(logger);
            _receiver = new BotMessageReceiver(_handler.HandleMessage, _handler.HandleError);
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _logger.LogInformation("Bot started..");
            _receiver.StartReceive(_cancellationTokenSource);
        }

        public void Stop()
        { 
            _logger.LogInformation("Bot stopped..");
            _cancellationTokenSource.Cancel();
        }
    }
}