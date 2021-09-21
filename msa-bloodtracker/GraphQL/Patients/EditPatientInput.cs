using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.GraphQL.Patients
{
    public record EditPatientInput(
        string PatientId,
        string? Name,
        string? GithubURL);
}
