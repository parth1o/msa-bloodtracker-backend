using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.GraphQL.Patients
{
    public record EditSelfInput(
        string? Name,
        string? GithubUrl);
}
