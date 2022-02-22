namespace AppleStore.Common
{
    public static class GlobalConstants
    {
        public class DataBaseConstants
        {
            public const string Name = "AppleStoreDb";
        }

        public class UserEmailConstants
        {
            public const string AdminEmail = "admin@mail.com";
        } 

        public class UserRolesConstants
        {
            public const string AdminRole = "Admin";
            public const string UserRole = "User";
        }

        public class AppleFormConstants
        {
            public const int NameMaxLength = 64;
            public const int DescriptionMaxLength = 664;
        }
    }
}
