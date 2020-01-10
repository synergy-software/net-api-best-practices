using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Synergy.Samples.Web.API.Services.Infrastructure.Commands
{
    public interface ICommandHandler<in TCommand, TCommandResult>
    {
        Task<TCommandResult> Handle([NotNull] TCommand command);
    }
}