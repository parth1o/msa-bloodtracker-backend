using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using msa_bloodtracker.Data;
using msa_bloodtracker.Model;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using msa_bloodtracker.GraphQL.Bloodtests;

namespace msa_bloodtracker.GraphQL.Bloodtests
{
    public class BloodtestType : ObjectType<Bloodtest>
    {
        protected override void Configure(IObjectTypeDescriptor<Bloodtest> descriptor)
        {
            descriptor.Field(p => p.BloodtestId).Type<NonNullType<IdType>>();
            descriptor.Field(p => p.Date).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Hb).Type<NonNullType<IntType>>();
            descriptor.Field(p => p.Platelets).Type<NonNullType<IntType>>();
            descriptor.Field(p => p.WBC).Type<NonNullType<FloatType>>();
            descriptor.Field(p => p.Neuts).Type<NonNullType<FloatType>>();
            descriptor.Field(p => p.Creatinine).Type<NonNullType<IntType>>();
            descriptor.Field(p => p.Mg).Type<NonNullType<FloatType>>();

            descriptor
                .Field(p => p.Patient)
                .ResolveWith<Resolvers>(r => r.GetPatient(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<PatientType>>();

        }


        private class Resolvers
        {
            public async Task<Patient> GetPatient(Bloodtest bloodtest, [ScopedService] AppDbContext context,
               CancellationToken cancellationToken)
            {
                return await context.Patients.FindAsync(new object[] { bloodtest.PatientId }, cancellationToken);
            }

        }
    }
}
