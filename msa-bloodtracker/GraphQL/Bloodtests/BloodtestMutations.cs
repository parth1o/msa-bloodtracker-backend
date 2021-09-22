using HotChocolate;
using HotChocolate.Types;
using msa_bloodtracker.Data;
using msa_bloodtracker.Extensions;
using msa_bloodtracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.AspNetCore;

namespace msa_bloodtracker.GraphQL.Bloodtests
{
    [ExtendObjectType(name: "Mutation")]
    public class BloodtestMutations
    {
        [UseAppDbContext]
        
        public async Task<Bloodtest> AddBloodtestAsync(AddBloodtestInput input, ClaimsPrincipal claimsPrincipal,
            [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var PatientIdStr = claimsPrincipal.Claims.First(c => c.Type == "PatientId").Value;
            var bloodtest = new Bloodtest
            {
                Date = input.Date,
                Hb = input.Hb,
                Platelets = input.Platelets,
                WBC = input.WBC,
                Neuts = input.Neuts,
                Creatinine = input.Creatinine,
                Mg = input.Mg,
                PatientId = int.Parse(PatientIdStr)
            };
            context.Bloodtests.Add(bloodtest);

            await context.SaveChangesAsync(cancellationToken);

            return bloodtest;
        }

        [UseAppDbContext]
        [Authorize]
        public async Task<Bloodtest> EditBloodtestAsync(EditBloodtestInput input, ClaimsPrincipal claimsPrincipal,
            [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var PatientIdStr = claimsPrincipal.Claims.First(c => c.Type == "PatientId").Value;
            var bloodtest = await context.Bloodtests.FindAsync(input.BloodtestId);


            bloodtest.Date = input.Date;
            bloodtest.Hb = input.Hb ?? bloodtest.Hb;
            bloodtest.Platelets = input.Platelets ?? bloodtest.Platelets;
            bloodtest.WBC = input.WBC ?? bloodtest.WBC;
            bloodtest.Neuts = input.Neuts ?? bloodtest.Neuts;
            bloodtest.Creatinine = input.Creatinine ?? bloodtest.Creatinine;
            bloodtest.Mg = input.Mg ?? bloodtest.Mg;

            if (bloodtest.PatientId != int.Parse(PatientIdStr))
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Not owned by student")
                    .SetCode("AUTH_NOT_AUTHORIZED")
                    .Build());
            }
            context.Bloodtests.Add(bloodtest);

            await context.SaveChangesAsync(cancellationToken);

            return bloodtest;
        }

    }
}
