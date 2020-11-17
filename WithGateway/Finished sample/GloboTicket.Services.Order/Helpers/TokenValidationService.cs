using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GloboTicket.Services.Ordering.Helpers
{
    public class TokenValidationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenValidationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ValidateTokenAsync(string tokenToValidate, 
            DateTime receivedAt)
        {
            var client = _httpClientFactory.CreateClient();

            var discoveryDocumentResponse = await client
                .GetDiscoveryDocumentAsync("https://localhost:5010");
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            try
            {

                var issuerSigningKeys = new List<SecurityKey>();
                foreach (var webKey in discoveryDocumentResponse.KeySet.Keys)
                {
                    var e = Base64Url.Decode(webKey.E);
                    var n = Base64Url.Decode(webKey.N);

                    var key = new RsaSecurityKey(new RSAParameters
                    { Exponent = e, Modulus = n })
                    {
                        KeyId = webKey.Kid
                    };

                    issuerSigningKeys.Add(key);
                }

                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "ordering",
                    ValidIssuer = "https://localhost:5010",
                    IssuerSigningKeys = issuerSigningKeys,
                    LifetimeValidator = (notBefore, expires, securityToken, tokenValidationParameters) =>
                    {
                        return expires.Value.ToUniversalTime() > receivedAt.ToUniversalTime();
                    }
                };

                _ = new JwtSecurityTokenHandler().ValidateToken(tokenToValidate,
                    tokenValidationParameters, out var rawValidatedToken);

                return true;
            }
            catch (SecurityTokenValidationException)
            {
                // Validation failed - log this if needed
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
