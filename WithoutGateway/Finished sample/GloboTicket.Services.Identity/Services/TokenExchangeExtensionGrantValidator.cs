using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.Identity.Services
{
    public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";
        private string _accessTokenType => "urn:ietf:params:oauth:token-type:access_token";

        private readonly ITokenValidator _validator;

        public TokenExchangeExtensionGrantValidator(ITokenValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var requestedGrant = context.Request.Raw.Get("grant_type");
            if (string.IsNullOrWhiteSpace(requestedGrant) || requestedGrant != GrantType)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    "Invalid grant.");
                return;
            }

            var subjectToken = context.Request.Raw.Get("subject_token");
            if (string.IsNullOrWhiteSpace(subjectToken))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                    "Subject token missing.");
                return;
            }

            var subjectTokenType = context.Request.Raw.Get("subject_token_type");
            if (string.IsNullOrWhiteSpace(subjectTokenType))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                    "Subject token type missing.");
                return;
            }

            // use the subject token type to know how to validate it.  Must be
            // an access token in our case
            if (subjectTokenType != _accessTokenType)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                   "Subject token type invalid.");
                return;
            }

            var result = await _validator.ValidateAccessTokenAsync(subjectToken);
            if (result.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    "Subject token invalid.");
                return;
            }

            // get the subject from the access token
            var subjectClaim = result.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subjectClaim == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                    "Subject token must contain sub value.");
                return;
            }

            // ... potential authorization checks

            // ... potential claims transformation

            context.Result = new GrantValidationResult(
                subjectClaim.Value, 
                "access_token", 
                result.Claims);

            return;

        }
    }
}
