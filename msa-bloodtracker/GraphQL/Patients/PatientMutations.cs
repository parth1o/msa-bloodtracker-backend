using msa_bloodtracker.Extensions;
using msa_bloodtracker.Data;
using msa_bloodtracker.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Octokit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Authorization;
using msa_bloodtracker.GraphQL.Patients;

namespace msa_bloodtracker.GraphQL.Patients
{
    [ExtendObjectType(name: "Mutation")]
    public class PatientMutations
    {

        [UseAppDbContext]
        [Authorize]
        public async Task<Patient> EditSelfAsync(EditSelfInput input, ClaimsPrincipal claimsPrincipal,
                [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var PatientIdStr = claimsPrincipal.Claims.First(c => c.Type == "PatientId").Value;
            var patient = await context.Patients.FindAsync(int.Parse(PatientIdStr), cancellationToken);
            patient.Name = input.Name ?? patient.Name;
            patient.GithubURL = input.GithubUrl ?? patient.GithubURL;

            context.Patients.Add(patient);

            await context.SaveChangesAsync(cancellationToken);

            return patient;
        }

        [UseAppDbContext]
        public async Task<LoginPayload> LoginAsync(LoginInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var client = new GitHubClient(new ProductHeaderValue("msa-bloodtracker"));

            var request = new OauthTokenRequest(Startup.Configuration["Github:ClientId"], Startup.Configuration["Github:ClientSecret"], input.Code);
            var tokenInfo = await client.Oauth.CreateAccessToken(request);

            if (tokenInfo.AccessToken == null)
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Bad code")
                    .SetCode("AUTH_NOT_AUTHENTICATED")
                    .Build());
            }

            client.Credentials = new Credentials(tokenInfo.AccessToken);
            var user = await client.User.Current();

            var patient = await context.Patients.FirstOrDefaultAsync(s => s.GithubURL == user.HtmlUrl, cancellationToken);

            if (patient == null)
            {
                patient = new Patient
                {
                    Name = user.Name ?? user.Login,
                    GithubURL = user.HtmlUrl,
                };

                context.Patients.Add(patient);
                await context.SaveChangesAsync(cancellationToken);
            }

            // authentication successful so generate jwt token
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>{
                new Claim("PatientId", patient.PatientId.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                "msa-bloodtracker",
                "MSA-Patient",
                claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new LoginPayload(patient, token);


        }

    }
}
