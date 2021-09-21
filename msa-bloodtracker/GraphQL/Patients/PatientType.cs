using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Types;
using msa_bloodtracker.Data;
using msa_bloodtracker.Model;

namespace msa_bloodtracker.GraphQL.Bloodtests
{
    public class PatientType : ObjectType<Patient>
    {
        protected override void Configure(IObjectTypeDescriptor<Patient> descriptor)
        {
            descriptor.Field(s => s.PatientId).Type<NonNullType<IdType>>();
            descriptor.Field(s => s.Name).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.GithubURL).Type<NonNullType<StringType>>();

            descriptor
                .Field(s => s.Bloodtests)
                .ResolveWith<Resolvers>(r => r.GetBloodtests(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<ListType<NonNullType<BloodtestType>>>>();
        }

        private class Resolvers
        {
            public async Task<IEnumerable<Bloodtest>> GetBloodtests(Patient patient, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Bloodtests.Where(c => c.PatientId == patient.PatientId).ToArrayAsync(cancellationToken);
            }

        }
    }
}
