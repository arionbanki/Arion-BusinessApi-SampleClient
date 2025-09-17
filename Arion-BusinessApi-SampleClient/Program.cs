using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
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
    private static readonly string IOBWS_CLAIMS_BASE_PATH = "https://apigw.arionbanki.is/claims/api/v1";

    // Config these variables to your needs
    private static readonly string CLAIM_ID = "[Place the claim id here]"; // this is returned from endpoint /api/v1/claims/{claimId} "resourceId"
    private static readonly string BATCH_ID = "[Place the batch id here]"; // this is returned from endpoint /api/v1/batches/{batchId} "resourceId"
    private static readonly string DATE_FROM = "[Place the date from here]"; // this is the date from when records will be taken "YYYY-mm-dd"
    private static readonly string DATE_TO = "[Place the date to here]"; // this is the date from when records will be taken "YYYY-mm-dd"

    // Authorization for Claims API
    private static readonly string CLAIMS_TOKEN_URL = "https://apigw.arionbanki.is/oauth/v2/oauth-token"; // url to identity token service
    private static readonly string CLIENT_ID = "[Place client id here]";
    private static readonly string CLIENT_SECRET = "[Place client secret here]";
    private static readonly string CLIENT_SCOPES = "[Place client scopes here]";

    // UserApplication api key obtained from Developer Portal
    private static readonly string APIKEY = "[Place api key here]";

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
        Console.WriteLine("5) Live - Get Claim From Id");
        Console.WriteLine("6) Live - Get Claim From Id History");
        Console.WriteLine("7) Live - Get Claim From Id Transactions");
        Console.WriteLine("8) Live - Get Claims");
        Console.WriteLine("9) Live - Get Batches From Id");
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
                await GetClaimFromId();
                return true;
            case "6":
                await GetClaimFromIdHistory();
                return true;
            case "7":
                await GetClaimFromIdTransactions();
                return true;
            case "8":
                await GetClaims();
                return true;
            case "9":
                await BatchFromId();
                return true;
            default:
                return true;
        }
    }

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
    private static async Task GetClaimFromId()
    {
        // Build Request
        string claimId = CLAIM_ID;

        // Api call
        var client = await SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task GetClaimFromIdHistory()
    {
        // Build Request
        string claimId = CLAIM_ID;

        // Api call
        var client = await SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}/history?page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task GetClaimFromIdTransactions()
    {
        // Build Request
        string claimId = CLAIM_ID;

        // Api call
        var client = await SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}/transactions?page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task GetClaims()
    {
        // Build Request

        // Api call
        var client = await SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims?dateFrom={DATE_FROM}&dateTo={DATE_TO}&claimantId={CLAIM_ID[..10]}&page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task BatchFromId()
    {
        // Build Request
        string batchId = BATCH_ID;

        // Api call
        var client = await SetupHttpClaimsClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/batches/{batchId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    #endregion

    #region Helpers
    private static HttpClient SetupHttpCardsClient(bool sandbox)
    {
        if (sandbox)
        {
            // Get HttpClient
            HttpClient httpClient = new();

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

    private static async Task<HttpClient> SetupHttpClaimsClient()
    {
        var nvc = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", CLIENT_ID),
            new("client_secret", CLIENT_SECRET),
            new("scope", CLIENT_SCOPES)
        };

        // Fetch certificate from store
        var store = new X509Store(StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);
        var cers = store.Certificates.Find(X509FindType.FindBySubjectName, "[Enter subject name]", false);

        // Get token from curity
        var tokenClientHandler = new HttpClientHandler();
        tokenClientHandler.ClientCertificates.Add(cers[0]);
        var client = new HttpClient(tokenClientHandler);
        var res = await client.PostAsync(CLAIMS_TOKEN_URL, new FormUrlEncodedContent(nvc));
        tokenClientHandler.Dispose();
        client.Dispose();
        var json = await res.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<Token>(json);

        // Adding certificate to handler
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(cers[0]);
        store.Close();

        // Get HttpClient
        var clientWithCertificate = new HttpClient(handler);

        // Set headers
        clientWithCertificate.DefaultRequestHeaders.Add("X-Request-ID", Guid.NewGuid().ToString());
        clientWithCertificate.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKEY);
        clientWithCertificate.DefaultRequestHeaders.Add("Accept", "application/json");

        // Set bearer token
        clientWithCertificate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);

        return clientWithCertificate;
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

