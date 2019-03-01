using System.Threading.Tasks;

namespace GraphQLExample.Infrastructure
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}