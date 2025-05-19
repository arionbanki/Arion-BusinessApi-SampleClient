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

    // Base path to claims API
    private static readonly string IOBWS_CLAIMS_BASE_PATH = "https://apigw-dev.arionbanki.is/claimstemp/api/v1";

    // Config these variables to your needs
    private static readonly string CLAIM_API_ACCESS_TOKEN = "[Place your token from the identity service in here]"; // you can retrieve this token from the identity service
    private static readonly string CLAIM_ID = "[Place the claim id here]"; // this is returned from endpoint /api/v1/claims/{claimId} "resourceId"
    private static readonly string BATCH_ID = "[Place the batch id here]"; // this is returned from endpoint /api/v1/batches/{batchId} "resourceId"
    private static readonly string DATE_FROM = "YYYY-mm-dd"; // this is the date from when records will be taken "YYYY-mm-dd"
    private static readonly string DATE_TO = "YYYY-mm-dd"; // this is the date from when records will be taken "YYYY-mm-dd"

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
        Console.WriteLine("==== IOBWS 3.0 Claims");
        Console.WriteLine("5) Sandbox - Get Claim From Id");
        Console.WriteLine("6) Sandbox - Get Claim From Id History");
        Console.WriteLine("7) Sandbox - Get Claim From Id Transactions");
        Console.WriteLine("8) Sandbox - Get Claims");
        Console.WriteLine("9) Sandbox - Get Batches From Id");
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
            case "5":
                await SandboxGetClaimFromId();
                return true;
            case "6":
                await SandboxGetClaimFromIdHistory();
                return true;
            case "7":
                await SandboxGetClaimFromIdTransactions();
                return true;
            case "8":
                await SandboxGetClaims();
                return true;
            case "9":
                await SandboxBatchFromId();
                return true;
            default:
                return true;
        }
    }

    #region Sandbox Endpoints
    #region Cards
    private static async Task SandboxGetCards()
    {
        // Build Request

        // Api call
        HttpClient client = SetupHttpCardsClient(true);
        var response = await client.GetAsync($"{IOBWS_SANDBOX_BASE_PATH}/cards");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }

    private static async Task SandboxGetCardsFromId()
    {
        // Build Request
        string cardId = SANDBOX_CARD_ID;

        // Api call
        HttpClient client = SetupHttpCardsClient(true);
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
        HttpClient client = SetupHttpCardsClient(true);
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
        HttpClient client = SetupHttpCardsClient(true);

        // Make the request with formatted date strings
        var response = await client.GetAsync(
            $"{IOBWS_SANDBOX_BASE_PATH}/cards/{cardid}/transactions?bookingStatus={bookingStatus}&dateFrom={dateFrom}&dateTo={dateTo}"
        );

        var result = await response.Content.ReadAsStringAsync();
    }
    #endregion
    #region Claims
    private static async Task SandboxGetClaimFromId()
    {
        // Build Request
        string claimId = CLAIM_ID;

        // Api call
        HttpClient client = SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task SandboxGetClaimFromIdHistory()
    {
        // Build Request
        string claimId = CLAIM_ID;

        // Api call
        HttpClient client = SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}/history?page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task SandboxGetClaimFromIdTransactions()
    {
        // Build Request
        string claimId = CLAIM_ID;

        // Api call
        HttpClient client = SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}/transactions?page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task SandboxGetClaims()
    {
        // Build Request

        // Api call
        HttpClient client = SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims?dateFrom={DATE_FROM}&dateTo={DATE_TO}&claimantId={CLAIM_ID.Take(10)}&page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task SandboxBatchFromId()
    {
        // Build Request
        string batchId = BATCH_ID;

        // Api call
        HttpClient client = SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/batches/{batchId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    #endregion
    #endregion Sandbox Endpoints

    #region Helpers
    private static HttpClient SetupHttpCardsClient(bool sandbox)
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

    private static HttpClient SetupHttpClaimsClient()
    {
        // Get HttpClient
        HttpClient httpClient = new HttpClient();

        // Set headers
        httpClient.DefaultRequestHeaders.Add("X-Request-ID", Guid.NewGuid().ToString());
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        // Set bearer token
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CLAIM_API_ACCESS_TOKEN); // Bearer token

        return httpClient;
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

