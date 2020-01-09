using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Synergy.Samples.Web.API.Services.Infrastructure.Queries
{
    public interface IQueryHandler<in TQuery, TQueryResult>
    {
        Task<TQueryResult> Handle([NotNull] TQuery query);
    }
}