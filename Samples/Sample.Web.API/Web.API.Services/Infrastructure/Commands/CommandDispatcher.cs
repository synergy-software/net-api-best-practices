using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Services.Infrastructure.Commands
{
    [UsedImplicitly]
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;
        private readonly ILogger<CommandDispatcher> _logger;

        public CommandDispatcher(ICommandHandlerFactory commandHandlerFactory, ILogger<CommandDispatcher> logger)
        {
            this.commandHandlerFactory = commandHandlerFactory;
            _logger = logger;
        }

        public Task<TCommandResult> Dispatch<TCommand, TCommandHandler, TCommandResult>(TCommand command)
            where TCommand : class
            where TCommandResult : class
            where TCommandHandler : ICommandHandler<TCommand, TCommandResult>
        {
            Fail.IfArgumentNull(command, nameof(command));
            var commandHandler = commandHandlerFactory.Create<TCommand, TCommandResult>(command);
            _logger.LogTrace("Command {Command} dispatch started by {CommandHandler}", command.GetType().Name, commandHandler.GetType().Name);

            try
            {
                var result = commandHandler.Handle(command);
                return result;
            }
            finally
            {
                commandHandlerFactory.Destroy(commandHandler);
            }
        }
    }

    public interface ICommandDispatcher
    {
        Task<TCommandResult> Dispatch<TCommand, TCommandHandler, TCommandResult>([NotNull] TCommand command)
            where TCommand : class
            where TCommandResult : class
            where TCommandHandler : ICommandHandler<TCommand, TCommandResult>;
    }
}