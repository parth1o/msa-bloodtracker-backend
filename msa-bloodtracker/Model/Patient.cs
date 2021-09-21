using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.Model
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string GithubURL { get; set; } = default!;

        public ICollection<Bloodtest> Bloodtests { get; set; } = new List<Bloodtest>();
    }
}
