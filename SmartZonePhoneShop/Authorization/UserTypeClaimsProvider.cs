//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using SmartZonePhoneShop.Data;
//using SmartZonePhoneShop.Model;

//namespace SmartZonePhoneShop.Authentication
//{
//    public class UserTypeClaimsProvider : IClaimsTransformation
//    {
//        private readonly UserManager<User> _userManager;
//        ApplicationDbContext _context;

//        public UserTypeClaimsProvider(UserManager<User> userManager, ApplicationDbContext context)
//        {
//            _userManager = userManager;
//            _context = context;
//        }

//        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
//        {
//            var user = await _userManager.GetUserAsync(principal);

//            if (user == null)
//            {
//                return principal;
//            }

//            var claimsIdentity = (ClaimsIdentity)principal.Identity;

//            // Pronalazimo UserTypeId iz user objekta
//            var userTypeId = user.UserTypeId;

//            // Ucitavamo UserType model za zadati UserTypeId
//            var userType = await _context.UserTypes
//                .Where(ut => ut.UserTypeId == userTypeId)
//                .FirstOrDefaultAsync();

//            if (userType != null)
//            {
//                if (userType.Name == "Administrator")
//                {
//                    claimsIdentity.AddClaim(new Claim("Name", "Administrator"));
//                }
//                else if (userType.Name == "Kupac")
//                {
//                    claimsIdentity.AddClaim(new Claim("Name", "Kupac"));
//                }
//            }


//            return principal;
//        }

//    }
//}
