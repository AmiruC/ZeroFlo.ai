using RestSharp;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ZeroFloApp.Services.Users
{
    public class WalletAccount
    {
        public string? Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal Balance { get; set; }
        public string? Currency { get; set; }
        public decimal TotalAmount { get; set; }
        public WalletAccountLinks? Links { get; set; }
    }

    public class WalletAccountLinks
    {
        public string? Self { get; set; }
        public string? Users { get; set; }
        public string? BatchTransactions { get; set; }
        public string? Transactions { get; set; }
        public string? BpayDetails { get; set; }
        public string? NppDetails { get; set; }
        public string? PayinDetails { get; set; }
        public string? VirtualAccounts { get; set; }
    }

    public class WalletAccountsResponse
    {
        [JsonProperty("wallet_accounts")]
        public WalletAccount? WalletAccounts { get; set; }
    }

    public class UserService
    {
        // Create User API Call
        public async Task CreateUser(string token)
        {
            var options = new RestClientOptions("https://test.api.promisepay.com/users");
            var client = new RestClient(options);
            var request = new RestRequest("", Method.Post);

            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            // JSON body to fill dynamically
            var jsonBody = new
            {
                id = "buyer-1234567890",
                first_name = "John",
                last_name = "Doe",
                email = "john.doe+buyer1234567890@promisepay.com",
                mobile = "+0987654321",
                address_line1 = "456 Example Ave",
                address_line2 = "Suite 101",
                city = "Exampletown",
                state = "EX",
                zip = "67890",
                country = "AUS",
                dob = "02/02/1990",
                government_number = "987654321",
                drivers_license_number = "D9876543",
                drivers_license_state = "EX",
                ip_address = "192.168.1.2",
                logo_url = "http://example.com/new_logo.png",
                color_1 = "#0000FF",
                color_2 = "#FF0000",
                custom_descriptor = "New Custom Descriptor",
                authorized_signer_title = "Manager",
                company = new
                {
                    name = "New Example Company",
                    legal_name = "New Example Company Pty Ltd",
                    tax_number = "123456789",
                    charge_tax = true,
                    address_line1 = "789 New St",
                    address_line2 = "Floor 2",
                    city = "Newcity",
                    state = "NW",
                    zip = "12345",
                    country = "AUS",
                    phone = "+1234567890"
                }
            };

            // Load the request body 
            request.AddJsonBody(jsonBody);

            var response = await client.PostAsync(request);
            // Check if the response is successful
            if (response.IsSuccessful)
            {
                Console.WriteLine("User created successfully:");
                Console.WriteLine(response.Content);
            }
            else
            {
                Console.WriteLine("Error creating user:");
                Console.WriteLine(response.Content);
            }
        }

        // Get the user's wallet details 
        public async Task GetUserWallet(string userId, string token)
        {
            var options = new RestClientOptions($"https://test.api.promisepay.com/users/{userId}/wallet_accounts");
            var client = new RestClient(options);
            var request = new RestRequest("", Method.Get);

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.GetAsync(request);

            if (response.IsSuccessful)
            {
                // Check wallet details have been received successfully 
                Console.WriteLine("Wallet accounts retrieved successfully:");
                Console.WriteLine(response.Content);

                var walletAccountsResponse = JsonConvert.DeserializeObject<WalletAccountsResponse>(response.Content);
                if (walletAccountsResponse != null && walletAccountsResponse.WalletAccounts != null)
                {
                    // walletAccount will contain the wallet details 
                    var walletAccount = walletAccountsResponse.WalletAccounts;

                    // Print wallet details 
                    Console.WriteLine($"ID: {walletAccount.Id}");
                    Console.WriteLine($"Active: {walletAccount.Active}");
                    Console.WriteLine($"Created At: {walletAccount.CreatedAt}");
                    Console.WriteLine($"Updated At: {walletAccount.UpdatedAt}");
                    Console.WriteLine($"Balance: {walletAccount.Balance}");
                    Console.WriteLine($"Currency: {walletAccount.Currency}");
                    Console.WriteLine($"Total Amount: {walletAccount.TotalAmount}");

                    Console.WriteLine($"Links:");
                    Console.WriteLine($"  Self: {walletAccount.Links?.Self}");
                    Console.WriteLine($"  Users: {walletAccount.Links?.Users}");
                    Console.WriteLine($"  Batch Transactions: {walletAccount.Links?.BatchTransactions}");
                    Console.WriteLine($"  Transactions: {walletAccount.Links?.Transactions}");
                    Console.WriteLine($"  Bpay Details: {walletAccount.Links?.BpayDetails}");
                    Console.WriteLine($"  Npp Details: {walletAccount.Links?.NppDetails}");
                    Console.WriteLine($"  Payin Details: {walletAccount.Links?.PayinDetails}");
                    Console.WriteLine($"  Virtual Accounts: {walletAccount.Links?.VirtualAccounts}");
                }
            }
            else
            {
                Console.WriteLine("Error retrieving wallet accounts:");
                Console.WriteLine(response.Content);
            }
        }
    }
}
