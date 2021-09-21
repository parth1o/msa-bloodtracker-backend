using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using msa_bloodtracker.Data;
using msa_bloodtracker.Extensions;
using msa_bloodtracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace msa_bloodtracker.GraphQL.Patients
{
    [ExtendObjectType(name: "Query")]
    public class PatientQueries
    {
        [UseAppDbContext]
        [UsePaging]
        [Authorize]
        public IQueryable<Patient> GetPatients([ScopedService] AppDbContext context)
        {
            return context.Patients;
        }

        [UseAppDbContext]
        [Authorize]
        public Patient GetPatient([GraphQLType(typeof(NonNullType<IdType>))] string id, [ScopedService] AppDbContext context)
        {
            return context.Patients.Find(int.Parse(id));
        }

        [UseAppDbContext]
        [Authorize]
        public Patient GetSelf(ClaimsPrincipal claimsPrincipal, [ScopedService] AppDbContext context)
        {
            var PatientIdStr = claimsPrincipal.Claims.First(c => c.Type == "PatientId").Value;

            return context.Patients.Find(int.Parse(PatientIdStr));
        }

    }
}
