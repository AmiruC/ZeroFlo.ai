using ZeroFloApp.Services.Auth;
using ZeroFloApp.Services.Users;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var authService = new AuthService();
        string? token = await authService.GetBearerToken();

        if (token != null)
        {
            Console.WriteLine("Bearer token obtained successfully:");
            Console.WriteLine();

            // Call the function to create a user
            var userService = new UserService();
            Console.WriteLine("Calling Function");
            await userService.GetUserWallet("buyer-0000000001", token);
            // await userService.CreateSubWallet("buyer-0000000001", token);
        }
        else
        {
            Console.WriteLine("Main token went wrong");
        }
    }
}
