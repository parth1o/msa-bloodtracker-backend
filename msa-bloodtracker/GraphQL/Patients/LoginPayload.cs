using msa_bloodtracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.GraphQL.Patients
{
    public record LoginPayload(
        Patient patient,
        string jwt);
}
