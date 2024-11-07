using System.Net.Http.Headers;
using System.Text.Json.Serialization;

class Program
{
    // Base path to cards sandbox API
    private static readonly string IOBWS_SANDBOX_BASE_PATH = "https://apigwsandbox.arionbanki.is/cards/api/v1";

    // Config these sandbox variables to your needs
    private static readonly string SANDBOX_ACCESS_TOKEN = "[Place your token from the developer portal in here]"; // you can create this token in the developer portal for your application
    private static readonly string SANDBOX_API_KEY = "[Place your ApiKey from developer portal here]";
    private static readonly string SANDBOX_CARD_ID = "[Place your sandbox card id here]"; // this is returned from endpoint /api/v1/cards "resourceId"

    static async Task Main(string[] args)
    {
        bool showMenu = true;
        while (showMenu)
        {
            showMenu = await MainMenu();
        }
    }

    private static async Task<bool> MainMenu()
    {
        Console.WriteLine("");
        Console.WriteLine("==== IOBWS 3.0 Cards");
        Console.WriteLine("1) Sandbox - Get Cards");
        Console.WriteLine("2) Sandbox - Get Cards From Id");
        Console.WriteLine("3) Sandbox - Get Cards Balances");
        Console.WriteLine("4) Sandbox - Get Cards Transactions");
        Console.Write("\r\nSelect an option: ");

        switch (Console.ReadLine())
        {
            case "1":
                await SandboxGetCards();          
                return true;
            case "2":
                await SandboxGetCardsFromId();
                return true;
            case "3":
                await SandboxGetCardBalances();
                return true;
            case "4":
                await SandboxGetCardTransactions();
                return true;
            default:
                return true;
        }
    }

    #region Sandbox Endpoints
    private static async Task SandboxGetCards()
    {
        // Build Request

        // Api call
        HttpClient client = SetupHttpClient(true);
        var response = await client.GetAsync($"{IOBWS_SANDBOX_BASE_PATH}/cards");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }

    private static async Task SandboxGetCardsFromId()
    {
        // Build Request
        string cardId = SANDBOX_CARD_ID;

        // Api call
        HttpClient client = SetupHttpClient(true);
        var response = await client.GetAsync($"{IOBWS_SANDBOX_BASE_PATH}/cards/{cardId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }

    private static async Task SandboxGetCardBalances()
    {
        // Build Request
        string cardid = SANDBOX_CARD_ID;
        DateTime today = DateTime.Now.AddDays(-30);

        // Api call
        HttpClient client = SetupHttpClient(true);
        var response = await client.GetAsync($"{IOBWS_SANDBOX_BASE_PATH}/cards/{cardid}/balances?dateFrom={today}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }

    private static async Task SandboxGetCardTransactions()
    {
        // Build Request
        string cardid = SANDBOX_CARD_ID;
        string bookingStatus = "booked";
        DateTime dateFrom = DateTime.Now.AddDays(-30);
        DateTime dateTo = DateTime.UtcNow;

        // Api call
        HttpClient client = SetupHttpClient(true);

        // Make the request with formatted date strings
        var response = await client.GetAsync(
            $"{IOBWS_SANDBOX_BASE_PATH}/cards/{cardid}/transactions?bookingStatus={bookingStatus}&dateFrom={dateFrom}&dateTo={dateTo}"
        );

        var result = await response.Content.ReadAsStringAsync();
    }

    #endregion Sandbox Endpoints

    #region Helpers
    private static HttpClient SetupHttpClient(bool sandbox)
    {
        if (sandbox)
        {
            // Get HttpClient
            HttpClient httpClient = new HttpClient();

            // Set headers
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SANDBOX_API_KEY);
            httpClient.DefaultRequestHeaders.Add("xRequestId", Guid.NewGuid().ToString());
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Set bearer token
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SANDBOX_ACCESS_TOKEN); // Bearer token

            return httpClient;
        }
        else
        {
            // Will be added when Cards.Api is live
            return new HttpClient();
        }
    }

    internal class Token
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
    }

    #endregion Helpers
}

