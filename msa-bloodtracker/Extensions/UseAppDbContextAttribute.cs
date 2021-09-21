using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using msa_bloodtracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace msa_bloodtracker.Extensions
{
    public class UseAppDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<AppDbContext>();
        }
    }
}
