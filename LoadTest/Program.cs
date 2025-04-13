
using System.Diagnostics;

if (args.Length < 2)
{
    Console.WriteLine("Usage: LoadTest <strategy> <requests>");
    Console.WriteLine("Strategies: roundrobin, leastconnection, stickysessions");
    Console.WriteLine("Example: LoadTest roundrobin 100");
    return;
}

var strategy = args[0].ToLower();
int totalRequests = int.Parse(args[1]);

var baseUrl = "http://localhost:50000";
var endpoint = string.Empty;

switch (strategy)
{
    case "roundrobin":
        endpoint = $"{baseUrl}/gateway/roundrobin/products/instance-info";
        break;
    case "leastconnection":
        endpoint = $"{baseUrl}/gateway/leastconnection/products/instance-info";
        break;
    case "stickysessions":
        endpoint = $"{baseUrl}/gateway/stickysessions/products/instance-info";
        break;
    default:
        Console.WriteLine("Unknown strategy. Use: roundrobin, leastconnection, or stickysessions");
        break;
}

Console.WriteLine($"Running load test for {strategy} strategy with {totalRequests} requests to {endpoint}");

// Test results
var instanceHits = new Dictionary<string, int>();
var responseTimes = new List<long>();
int successCount = 0;
int failCount = 0;

var stopwatch = new Stopwatch();
stopwatch.Start();

var httpClient = new HttpClient();
var tasks = new List<Task>();

for (int i = 0; i < totalRequests; i++)
{
    tasks.Add(Task.Run(async () =>
    {
        try
        {
            var requestStopwatch = new Stopwatch();
            requestStopwatch.Start();

            var response = await httpClient.GetAsync(endpoint);
            requestStopwatch.Stop();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                string instance = "Unknown";
                if (content.Contains("Instance"))
                {
                    instance = content.Split("Instance\":\"")[1].Split("\"")[0];
                }

                lock (instanceHits)
                {
                    if (!instanceHits.ContainsKey(instance))
                        instanceHits[instance] = 0;

                    instanceHits[instance]++;
                    successCount++;
                    responseTimes.Add(requestStopwatch.ElapsedMilliseconds);
                }
            }
            else
            {
                lock (instanceHits)
                {
                    failCount++;
                }
            }
        }
        catch (Exception ex)
        {
            lock (instanceHits)
            {
                Console.WriteLine($"Error: {ex.Message}");
                failCount++;
            }
        }
    }));

    // A small delay to spread the load
    if (i % 10 == 0) await Task.Delay(50);
}

await Task.WhenAll(tasks);
stopwatch.Stop();

var totalTime = stopwatch.ElapsedMilliseconds;
var avgResponseTime = responseTimes.Count > 0 ? responseTimes.Average() : 0;
var maxResponseTime = responseTimes.Count > 0 ? responseTimes.Max() : 0;
var minResponseTime = responseTimes.Count > 0 ? responseTimes.Min() : 0;

Console.WriteLine("\n---- TEST RESULTS ----");
Console.WriteLine($"Strategy: {strategy}");
Console.WriteLine($"Total Requests: {totalRequests}");
Console.WriteLine($"Successful Requests: {successCount}");
Console.WriteLine($"Failed Requests: {failCount}");
Console.WriteLine($"Total Test Time: {totalTime}ms");
Console.WriteLine($"Average Response Time: {avgResponseTime:F2}ms");
Console.WriteLine($"Min Response Time: {minResponseTime}ms");
Console.WriteLine($"Max Response Time: {maxResponseTime}ms");

Console.WriteLine("\nInstance Distribution:");
foreach (var instance in instanceHits)
{
    double percentage = (double)instance.Value / successCount * 100;
    Console.WriteLine($"  {instance.Key}: {instance.Value} requests ({percentage:F2}%)");
}