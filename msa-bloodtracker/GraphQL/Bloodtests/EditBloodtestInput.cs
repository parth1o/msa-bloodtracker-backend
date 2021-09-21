using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.GraphQL.Bloodtests
{
    public record EditBloodtestInput(
        [property: GraphQLType(typeof(NonNullType<IdType>))]
        int BloodtestId,
        string Date,
        string? Description,
        int? Hb,
        int? Platelets,
        float? WBC,
        float? Neuts,
        int? Creatinine,
        float? Mg
        );
}
