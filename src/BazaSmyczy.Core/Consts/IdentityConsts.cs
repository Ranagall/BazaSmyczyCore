namespace BazaSmyczy.Core.Consts
{
    public class IdentityConsts
    {
        public const int MaxFailedAccessAttempts = 3;
   
        /// <summary>
        /// Lockout duration in minutes 
        /// </summary>
        public const int LockoutDuration = 30;
    }
}
