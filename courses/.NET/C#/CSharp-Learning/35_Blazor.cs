/*
================================================================================
TOPIC 35: BLAZOR
================================================================================

Blazor is a web framework for building interactive web apps with C#.

TABLE OF CONTENTS:
1. What is Blazor?
2. Blazor Server
3. Blazor WebAssembly
4. Components
5. Data Binding
================================================================================
*/

namespace BlazorConcepts
{
    // ====================================================================
    // BLAZOR COMPONENT EXAMPLE (.razor)
    // ====================================================================
    
    // Example: Counter.razor
    /*
    @page "/counter"
    
    <h1>Counter</h1>
    
    <p>Current count: @currentCount</p>
    
    <button class="btn btn-primary" @onclick="IncrementCount">
        Click me
    </button>
    
    @code {
        private int currentCount = 0;
        
        private void IncrementCount()
        {
            currentCount++;
        }
    }
    */
    
    // ====================================================================
    // BLAZOR PAGE WITH PARAMETERS
    // ====================================================================
    
    // Example: FetchData.razor
    /*
    @page "/fetchdata"
    @inject HttpClient Http
    
    <h1>Weather Forecast</h1>
    
    @if (forecasts == null)
    {
        <p>Loading...</p>
    }
    else
    {
        <table class="table">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp (°C)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var forecast in forecasts)
                {
                    <tr>
                        <td>@forecast.Date.ToShortDateString()</td>
                        <td>@forecast.TemperatureC</td>
                        <td>@forecast.Summary</td>
                    </tr>
                }
            </tbody>
        </table>
    }
    
    @code {
        private WeatherForecast[] forecasts;
        
        protected override async Task OnInitializedAsync()
        {
            forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>(
                "sample-data/weather.json");
        }
        
        public class WeatherForecast
        {
            public DateTime Date { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }
        }
    }
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Blazor ===");
            
            Console.WriteLine("\nBlazor Server:");
            Console.WriteLine("- Runs on server, sends HTML via SignalR");
            Console.WriteLine("- Fast initial load");
            Console.WriteLine("- Needs SignalR connection");
            Console.WriteLine("- Good for: Line-of-business apps");
            
            Console.WriteLine("\nBlazor WebAssembly:");
            Console.WriteLine("- Runs in browser via WebAssembly");
            Console.WriteLine("- Full SPA experience");
            Console.WriteLine("- Can work offline");
            Console.WriteLine("- Good for: Interactive apps, PWAs");
            
            Console.WriteLine("\n.NET MAUI Blazor:");
            Console.WriteLine("- Cross-platform mobile apps");
            Console.WriteLine("- Share UI code with web");
        }
    }
}

/*
BLAZOR FEATURES:
----------------
- C# instead of JavaScript
- Reusable components
- Two-way data binding
- Dependency injection
- Routing
- Forms and validation
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 36 covers Testing.
*/
