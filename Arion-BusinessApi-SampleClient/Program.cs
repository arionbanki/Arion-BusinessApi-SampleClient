using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

class Program
{
    #region Base Paths (not configurable)
    private static readonly string IOBWS_SANDBOX_BASE_PATH = "https://apigwsandbox.arionbanki.is/cards/api/v1";
    private static readonly string IOBWS_LIVE_BASE_PATH = "https://apigw.arionbanki.is/cards/api/v1";
    private static readonly string IOBWS_CLAIMS_BASE_PATH = "https://apigw.arionbanki.is/claims/api/v1";
    private static readonly string IOBWS_DOCUMENTS_BASE_PATH = "https://apigw.arionbanki.is/documents/api/v1";
    private static readonly string AUTH_TOKEN_URL = "https://apigw.arionbanki.is/oauth/v2/oauth-token";
    #endregion

    // Configuration loaded from appsettings.json
    private static IConfiguration _config = null!;

    // Helper to read a config value (throws if missing or empty)
    private static string Cfg(string key) =>
        _config[key] ?? throw new InvalidOperationException($"Missing configuration value: '{key}'");

    static async Task Main(string[] args)
    {
        _config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

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
        Console.WriteLine("5) Live - Get Cards");
        Console.WriteLine("6) Live - Get Cards From Id");
        Console.WriteLine("7) Live - Get Cards Balances");
        Console.WriteLine("8) Live - Get Cards Transactions");
        Console.WriteLine("==== IOBWS 3.0 Claims");
        Console.WriteLine("9) Live - Get Claim From Id");
        Console.WriteLine("10) Live - Get Claim From Id History");
        Console.WriteLine("11) Live - Get Claim From Id Transactions");
        Console.WriteLine("12) Live - Get Claims");
        Console.WriteLine("13) Live - Get Batches From Id");
        Console.WriteLine("==== IOBWS 3.0 Documents");
        Console.WriteLine("14) Live - Get Documents");
        Console.WriteLine("15) Live - Get Document by owner and Id");
        Console.WriteLine("16) Live - Get Document types by sender");
        Console.WriteLine("17) Live - Post Pdf Document");
        Console.WriteLine("18) Live - Post Xml Document");
        Console.WriteLine("19) Live - Get Cross-Reference by document Id");
        Console.WriteLine("20) Live - Get Cross-Reference by key and key-type");
        Console.WriteLine("21) Live - Post Cross-References");
        Console.WriteLine("22) Live - Put Cross-References");
        Console.WriteLine("23) Live - Delete Cross-Reference");
        
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
                await LiveGetCards();
                return true;
            case "6":
                await LiveGetCardsFromId();
                return true;
            case "7":
                await LiveGetCardBalances();
                return true;
            case "8":
                await LiveGetCardTransactions();
                return true;
            case "9":
                await LiveGetClaimFromId();
                return true;
            case "10":
                await LiveGetClaimFromIdHistory();
                return true;
            case "11":
                await LiveGetClaimFromIdTransactions();
                return true;
            case "12":
                await LiveGetClaims();
                return true;
            case "13":
                await LiveBatchFromId();
                return true;
            case "14":
                await LiveGetDocuments();
                return true;
            case "15":
                await LiveGetDocumentByOwnerAndId();
                return true;
            case "16":
                await LiveGetDocumentTypesBySender();
                return true;
            case "17":
                await LivePostPdfDocument();
                return true;
            case "18":
                await LivePostXmlDocument();
                return true;
            case "19":
                await LiveGetCrossReferenceByDocumentId();
                return true;
            case "20":
                await LiveGetCrossReferenceByKeyAndKeyType();
                return true;
            case "21":
                await LivePostCrossReferences();
                return true;
            case "22":
                await LivePutCrossReferences();
                return true;
            case "23":
                await LiveDeleteCrossReference();
                return true;
            default:
                return true;
        }
    }

    #region Cards Sandbox
    private static async Task SandboxGetCards()
    {
        // Build Request

        // Api call
        var client = SetupHttpSandboxClient();
        var response = await client.GetAsync($"{IOBWS_SANDBOX_BASE_PATH}/cards");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task SandboxGetCardsFromId()
    {
        // Build Request
        string cardId = Cfg("CardsSandbox:CardId");

        // Api call
        var client = SetupHttpSandboxClient();
        var response = await client.GetAsync($"{IOBWS_SANDBOX_BASE_PATH}/cards/{cardId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task SandboxGetCardBalances()
    {
        // Build Request
        string cardid = Cfg("CardsSandbox:CardId");
        DateTime today = DateTime.UtcNow.AddDays(-30);

        // Api call
        var client = SetupHttpSandboxClient();
        var response = await client.GetAsync($"{IOBWS_SANDBOX_BASE_PATH}/cards/{cardid}/balances?dateFrom={today}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task SandboxGetCardTransactions()
    {
        // Build Request
        string cardid = Cfg("CardsSandbox:CardId");
        string bookingStatus = "booked";
        DateTime dateFrom = DateTime.UtcNow.AddDays(-30);
        DateTime dateTo = DateTime.UtcNow;

        // Api call
        var client = SetupHttpSandboxClient();

        // Make the request with formatted date strings
        var response = await client.GetAsync(
            $"{IOBWS_SANDBOX_BASE_PATH}/cards/{cardid}/transactions?bookingStatus={bookingStatus}&dateFrom={dateFrom}&dateTo={dateTo}"
        );

        var result = await response.Content.ReadAsStringAsync();
    }
    #endregion
    #region Cards Live
    private static async Task LiveGetCards()
    {
        // Build Request

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_LIVE_BASE_PATH}/cards");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetCardsFromId()
    {
        // Build Request
        string cardId = Cfg("CardsLive:CardId");

        // Api call
        var client = SetupHttpSandboxClient();
        var response = await client.GetAsync($"{IOBWS_LIVE_BASE_PATH}/cards/{cardId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetCardBalances()
    {
        // Build Request
        string cardid = Cfg("CardsLive:CardId");
        DateTime today = DateTime.UtcNow.AddDays(-30);

        // Api call
        var client = SetupHttpSandboxClient();
        var response = await client.GetAsync($"{IOBWS_LIVE_BASE_PATH}/cards/{cardid}/balances?dateFrom={today}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetCardTransactions()
    {
        // Build Request
        string cardid = Cfg("CardsLive:CardId");
        string bookingStatus = "booked";
        DateTime dateFrom = DateTime.UtcNow.AddDays(-30);
        DateTime dateTo = DateTime.UtcNow;

        // Api call
        var client = SetupHttpSandboxClient();

        // Make the request with formatted date strings
        var response = await client.GetAsync(
            $"{IOBWS_LIVE_BASE_PATH}/cards/{cardid}/transactions?bookingStatus={bookingStatus}&dateFrom={dateFrom}&dateTo={dateTo}"
        );

        var result = await response.Content.ReadAsStringAsync();
    }
    #endregion
    #region Claims
    private static async Task LiveGetClaimFromId()
    {
        // Build Request
        string claimId = Cfg("Claims:ClaimId");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetClaimFromIdHistory()
    {
        // Build Request
        string claimId = Cfg("Claims:ClaimId");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}/history?page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetClaimFromIdTransactions()
    {
        // Build Request
        string claimId = Cfg("Claims:ClaimId");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims/{claimId}/transactions?page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetClaims()
    {
        // Build Request
        string dateFrom = Cfg("Claims:DateFrom");
        string dateTo = Cfg("Claims:DateTo");
        string claimId = Cfg("Claims:ClaimId");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/claims?dateFrom={dateFrom}&dateTo={dateTo}&claimantId={claimId[..10]}&page=1&itemsPerPage=500");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveBatchFromId()
    {
        // Build Request
        string batchId = Cfg("Claims:BatchId");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_CLAIMS_BASE_PATH}/batches/{batchId}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    #endregion
    #region Documents
    private static async Task LiveGetDocuments()
    {
        // Build Request
        string senderKennitala = Cfg("Documents:SenderId");
        string ownerKennitala = Cfg("Documents:OwnerId");
        string dateFrom = Cfg("Documents:DateFrom");
        string dateTo = Cfg("Documents:DateTo");
        string order = Cfg("Documents:Ordering");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/documents?SenderKennitala={senderKennitala}&OwnerKennitala={ownerKennitala}&DateFrom={dateFrom}&DateTo={dateTo}&Ordering={order}&Page=1&ItemsPerPage=10");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetDocumentByOwnerAndId()
    {
        // Build Request
        string ownerKennitala = Cfg("Documents:OwnerId");
        string documentId = Cfg("Documents:DocumentId");
        string documentStore = Cfg("Documents:DocumentStore"); // Optional query parameter

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/documents/{ownerKennitala}/{documentId}?documentStore={documentStore}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetDocumentTypesBySender()
    {
        // Build Request
        string senderKennitala = Cfg("Documents:SenderId");
        string documentStore = Cfg("Documents:DocumentStore"); // Optional query parameter

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/documents/{senderKennitala}/types?documentStore={documentStore}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LivePostPdfDocument()
    {
        // Build Request
        string uuid = Guid.NewGuid().ToString();
        string senderKennitala = Cfg("Documents:SenderId");
        string ownerKennitala = Cfg("Documents:OwnerId");
        string styleSheetName = Cfg("Documents:StyleSheetName");
        DateTime date = DateTime.UtcNow;
        string pdfFileName = "document.pdf"; // a document you have to add into the root folder of the project
        
        string jsonBody = CreatePdf(uuid, senderKennitala, ownerKennitala, styleSheetName, date, pdfFileName);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.PostAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/documents", content);

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LivePostXmlDocument()
    {
        // Build Request
        string uuid = Guid.NewGuid().ToString();
        string senderKennitala = Cfg("Documents:SenderId");
        string ownerKennitala = Cfg("Documents:OwnerId");
        string styleSheetName = Cfg("Documents:StyleSheetName");
        string definitionName = "Synidaemi";
        DateTime date = DateTime.UtcNow;
        
        string jsonBody = CreateXml(uuid, senderKennitala, ownerKennitala, styleSheetName, definitionName, date);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.PostAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/documents", content);

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetCrossReferenceByDocumentId()
    {
        // Build Request
        string documentId = Cfg("Documents:DocumentId");
        string documentStore = Cfg("Documents:DocumentStore");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/crossreferences?DocumentStore={documentStore}&DocumentId={documentId}&");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveGetCrossReferenceByKeyAndKeyType()
    {
        // Build Request
        string key = Cfg("Documents:DocumentKey");
        string keyType = Cfg("Documents:DocumentKeyType");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.GetAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/crossreferences?Key={key}&KeyType={keyType}&");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LivePostCrossReferences()
    {
        // Build Request
        string key = Cfg("Documents:DocumentKey");
        string keyType = Cfg("Documents:DocumentKeyType");
        string documentId = Cfg("Documents:DocumentId");
        string documentStore = Cfg("Documents:DocumentStore");
        string documentCategory = Cfg("Documents:DocumentCategory");
        DateTime date = DateTime.UtcNow;
        string documentDescription = "Test document description";
        string externalEventId = "event_1234";

        var body = new[]
        {
            new
            {
                Key = key,
                KeyType = keyType,
                DocumentId = documentId,
                DocumentStore = documentStore,
                DocumentCategory = documentCategory,
                DocumentEffectiveDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                DocumentDescription = documentDescription,
                ExternalEventId = externalEventId
            }
        };
        var jsonBody = JsonSerializer.Serialize(body);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
        
        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.PostAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/crossreferences", content);

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LivePutCrossReferences()
    {
        // Build Request
        string documentId = Cfg("Documents:DocumentId");
        // Edit any of the following values to update the document cross-reference
        string key = Cfg("Documents:DocumentKey");
        string keyType = Cfg("Documents:DocumentKeyType");
        string documentStore = Cfg("Documents:DocumentStore");
        string documentCategory = Cfg("Documents:DocumentCategory");
        DateTime date = DateTime.UtcNow;
        string documentDescription = "Test document description";
        string externalEventId = "event_1234";

        var body = new[]
        {
            new
            {
                Key = key,
                KeyType = keyType,
                DocumentId = documentId,
                DocumentStore = documentStore,
                DocumentCategory = documentCategory,
                DocumentEffectiveDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                DocumentDescription = documentDescription,
                ExternalEventId = externalEventId
            }
        };
        var jsonBody = JsonSerializer.Serialize(body);
        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
        
        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.PutAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/crossreferences", content);

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    private static async Task LiveDeleteCrossReference()
    {
        // Build Request
        string documentId = Cfg("Documents:DocumentId");
        string key = Cfg("Documents:DocumentKey");
        string keyType = Cfg("Documents:DocumentKeyType");

        // Api call
        var client = await SetupHttpLiveClient();
        var response = await client.DeleteAsync($"{IOBWS_DOCUMENTS_BASE_PATH}/crossreferences/{documentId}/{keyType}/{key}");

        // Results
        var result = await response.Content.ReadAsStringAsync();
    }
    #endregion

    #region Helpers
    private static HttpClient SetupHttpSandboxClient()
    {
        // Get HttpClient
        HttpClient httpClient = new();

        // Set headers
        httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Cfg("CardsSandbox:ApiKey"));
        httpClient.DefaultRequestHeaders.Add("xRequestId", Guid.NewGuid().ToString());
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        // Set bearer token
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Cfg("CardsSandbox:AccessToken"));

        return httpClient;
    }

    private static async Task<HttpClient> SetupHttpLiveClient()
    {
        var nvc = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", Cfg("Auth:ClientId")),
            new("client_secret", Cfg("Auth:ClientSecret")),
            new("scope", Cfg("Auth:Scopes"))
        };

        // Fetch certificate from store
        var store = new X509Store(StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);
        var cers = store.Certificates.Find(X509FindType.FindBySubjectName, Cfg("Auth:CertificateSubjectName"), false);

        // Get token from curity
        var tokenClientHandler = new HttpClientHandler();
        tokenClientHandler.ClientCertificates.Add(cers[0]);
        var client = new HttpClient(tokenClientHandler);
        var res = await client.PostAsync(AUTH_TOKEN_URL, new FormUrlEncodedContent(nvc));
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
        clientWithCertificate.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Cfg("Auth:ApiKey"));
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
    
    public static string CreateXml(
        string uuid,
        string senderKennitala,
        string ownerKennitala,
        string styleSheetName,
        string definitionName,
        DateTime date)
    {
        var dateString = date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
        var body = new
        {
            SenderKennitala = senderKennitala,
            DocumentTypeCode = "ARTS",
            DocumentStore = "Ark",
            EffectiveDate = date,
            StyleSheetName = styleSheetName,

            Files = new[]
            {
                new 
                {
                    Id = uuid,
                    Description = "This is a XML document for testing",
                    ReceiverKennitala = ownerKennitala,
                    FileType = "XML",
                    File = $"""
                            <?xml version="1.0" encoding="utf-8"?>
                            <!DOCTYPE XML-S SYSTEM "XML-S.dtd">
                            <XML-S>
                                <Statement Acct="{senderKennitala}{ownerKennitala}" Date="{dateString}" XKey="2">
                                    <?bgls.BlueGill.com DefinitionName={definitionName}?>
                                    <?bgls.BlueGill.com User1={senderKennitala}?>
                                    <?bgls.BlueGill.com User3={styleSheetName}?>
                                    <?bgls.BlueGill.com User4={uuid}?>
                                    <Section Name="BODY" Occ="1">
                                        <Field Name="Message">Functional test XML document.</Field>
                                        <Field Name="YourName">Automated test</Field>
                                    </Section>
                                </Statement>
                            </XML-S>
                            """
                }
            }
        };
        return JsonSerializer.Serialize(body);
    }
    
    public static string CreatePdf(
        string uuid,
        string senderKennitala,
        string ownerKennitala,
        string styleSheetName,
        DateTime date,
        string pdfFileName)
    {
        
        // Fetching the pdf file from the root of the project.
        string pdfPath = Path.Combine(Directory.GetCurrentDirectory(), pdfFileName);

        // Fallback: when the code is running from bin/Debug/net8.0, walk up to project root
        if (!File.Exists(pdfPath))
        {
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
            pdfPath = Path.Combine(projectRoot, pdfFileName);
        }

        if (!File.Exists(pdfPath))
        {
            throw new FileNotFoundException($"PDF file not found at '{pdfPath}'");
        }        
        string pdfBase64 = Convert.ToBase64String(File.ReadAllBytes(pdfPath));
        
        var body = new
        {
            SenderKennitala = senderKennitala,
            DocumentTypeCode = "ARTSR",
            DocumentStore = "Ark",
            EffectiveDate = date,
            StyleSheetName = styleSheetName,

            Files = new[]
            {
                new 
                {
                    Id = uuid,
                    Description = "This is a PDF document for testing",
                    ReceiverKennitala = ownerKennitala,
                    FileType = "PDF",
                    File = pdfBase64
                }
            }
        };
        return JsonSerializer.Serialize(body);
    }
    #endregion Helpers
}

