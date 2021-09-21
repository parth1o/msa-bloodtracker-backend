using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.GraphQL.Bloodtests
{
    public record AddBloodtestInput(
        string Date ,
        int Hb,
        int Platelets,
        float WBC,
        float Neuts,
        int Creatinine,
        float Mg);
}
