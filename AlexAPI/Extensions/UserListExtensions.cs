using AlexAPI.Authentication;
using AlexAPI.ViewModels.SimplifiedModels;

namespace AlexAPI.Extensions
{
    public static class UserListExtensions
    {
        public static List<SimplifiedUser> ToSimplifiedList(this IQueryable<ApplicationUser> users)
        {
            return SimplifyUsers(users.ToList());
        }
        public static List<SimplifiedUser> ToSimplifiedList(this IEnumerable<ApplicationUser> users)
        {
            return SimplifyUsers(users.ToList());
        }
        public static List<SimplifiedUser> ToSimplifiedList(this IList<ApplicationUser> users)
        {
            return SimplifyUsers(users.ToList());
        }

        private static List<SimplifiedUser> SimplifyUsers(List<ApplicationUser> users)
        {
            List<SimplifiedUser> result = new List<SimplifiedUser>();
            foreach (ApplicationUser user in users)
            {
                if (user != null)
                {
                    result.Add(
                        new SimplifiedUser
                        {
                            id = user.Id,
                            username = user.UserName,
                            email = user.Email
                        });
                }
            }
            return result;
        }
    }
}
