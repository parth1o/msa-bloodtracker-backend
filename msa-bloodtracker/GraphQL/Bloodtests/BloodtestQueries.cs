using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using msa_bloodtracker.Extensions;
using msa_bloodtracker.Data;
using msa_bloodtracker.Model;
using System.Security.Claims;
using HotChocolate.AspNetCore.Authorization;

namespace msa_bloodtracker.GraphQL.Bloodtests
{
    [ExtendObjectType(name: "Query")]
    public class BloodtestQueries
    {
        
        [UseAppDbContext]
        [UsePaging]
        [Authorize]
        public IQueryable<Bloodtest> GetBloodtests([ScopedService] AppDbContext context)
        {
            return context.Bloodtests;
        }

        [UseAppDbContext]
        [UsePaging]
        [Authorize]
        public IQueryable<Bloodtest> GetBloodtestsbyId(ClaimsPrincipal claimsPrincipal, [ScopedService] AppDbContext context)
        {
            var PatientIdStr = claimsPrincipal.Claims.First(c => c.Type == "PatientId").Value;

            return context.Bloodtests.Where(c => c.PatientId == int.Parse(PatientIdStr));
        }

        [UseAppDbContext]
        [Authorize]
        public Bloodtest GetBloodtest([GraphQLType(typeof(NonNullType<IdType>))] string id, [ScopedService] AppDbContext context)
        {
            return context.Bloodtests.Find(int.Parse(id));
        }
    }
}
