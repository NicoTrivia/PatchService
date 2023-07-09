
namespace CSqlManager;
using System.IdentityModel.Tokens.Jwt;

public class SecureEnpoint
{
            
    // Assuming you have access to the HttpContext
    public static string? GetJwtTokenFromContext(HttpContext context)
    {
        // Get the Authorization header value
        string authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        {
            // Extract the JWT token from the Authorization header
            string jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Return the JWT token
            return jwtToken;
        }

        // No valid JWT token found in the Authorization header
        return null;
    }

    public static JwtClaims getJwtClaims(HttpContext context) {
        JwtClaims result = new JwtClaims();
         result.Valid = false;
        string jwtToken = GetJwtTokenFromContext(context);

        if (jwtToken != null) {
            var tokenHandler = new JwtSecurityTokenHandler();
        
            // Parse the JWT token
            var jwt = tokenHandler.ReadJwtToken(jwtToken);
            
            // Extract and display the claims
            foreach (var claim in jwt.Claims)
            {
                if (claim.Type == "APP") {
                    result.APP = claim.Value;
                } else if (claim.Type == "User") {
                    result.User = claim.Value;
                }  else if (claim.Type == "Tenant") {
                    result.Tenant = claim.Value;
                }  else if (claim.Type == "Profile") {
                    result.Profile = claim.Value;
                } else if (claim.Type == "UserId") {
                    int r = 0;
                    if (int.TryParse(claim.Value, out r)) {
                        result.UserId = r;
                    }
                }
                
            }
            result.Valid = (result.APP == "Patch Services") && (result.Tenant != null) && (result.Profile != null) && (result.User != null);
        }
        return result;
    }
}