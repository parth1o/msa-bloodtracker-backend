using HotChocolate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.Model
{
    public class Bloodtest
    {
        [Key]
        public int BloodtestId { get; set; }

        [Required]
        public string Date { get; set; } = null!;

        [Required]
        public int Hb { get; set; }

        [Required]
        public int Platelets { get; set; }

        [Required]
        public float WBC { get; set; }

        [Required]
        public float Neuts { get; set; }

        [Required]
        public int Creatinine { get; set; }

        [Required]
        public float Mg { get; set; }

        [Required]
        [GraphQLIgnore]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
    }
}
