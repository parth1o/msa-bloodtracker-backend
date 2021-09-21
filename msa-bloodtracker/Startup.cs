using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using msa_bloodtracker.Data;
using msa_bloodtracker.GraphQL.Bloodtests;
using msa_bloodtracker.GraphQL.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msa_bloodtracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; } = default!;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));

            services.AddPooledDbContextFactory<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<PatientQueries>()
                    .AddTypeExtension<BloodtestQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<PatientMutations>()
                    .AddTypeExtension<BloodtestMutations>()
                .AddType<PatientType>()
                .AddType<BloodtestType>();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidIssuer = "msa-bloodtracker",
                            ValidAudience = "MSA-Patient",
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = signingKey
                        };
                });

            services.AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("MyPolicy");

            app.UseRouting();

            app.UseAuthentication();




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
