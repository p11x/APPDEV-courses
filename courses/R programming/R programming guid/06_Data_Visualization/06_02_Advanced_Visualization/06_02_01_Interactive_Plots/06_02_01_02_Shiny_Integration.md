# Shiny Integration with Plots

## Learning Objectives

- Understand Shiny web application framework
- Create reactive plots that respond to user input
- Use plotOutput() and renderPlot() functions
- Add interactive plot elements with Shiny
- Combine Plotly with Shiny for enhanced interactivity

## Theoretical Background

### What is Shiny?

Shiny is an R package for building interactive web applications entirely in R. It allows data scientists to create interactive dashboards and visualizations without JavaScript programming.

### Shiny Architecture

1. **UI (User Interface)**: Defines layout and inputs
2. **Server Function**: Contains reactive logic
3. **Reactive Expressions**: Automatically re-run when inputs change
4. **Outputs**: Rendered results displayed in UI

### Key Functions

*UI Side:*
- fluidPage(), fluidRow(), column() - Layout
- sidebarLayout(), sidebarPanel() - Sidebar layout
- plotOutput() - Plot output placeholder
- sliderInput(), selectInput(), textInput() - Input controls

*Server Side:*
- server = function(input, output, session) - Server logic
- renderPlot() - Render base R plots
- renderPlotly() - Render Plotly plots
- reactive() - Create reactive expression

## Code Examples

### Example: Basic Shiny App with Plot

```r
cat("===== BASIC SHINY PLOT APP =====\n\n")

# This example shows the structure of a basic Shiny app
# In practice, this would be in app.R file

# UI Definition
ui <- fluidPage(
  titlePanel("Interactive Data Visualization"),
  
  sidebarLayout(
    sidebarPanel(
      # Input: Sample size slider
      sliderInput("n", 
                  "Sample Size", 
                  min = 50, 
                  max = 1000, 
                  value = 200),
      
      # Input: Mean selector
      sliderInput("mean",
                 "Mean",
                 min = -10,
                 max = 10,
                 value = 0)
    ),
    
    mainPanel(
      plotOutput("distPlot")
    )
  )
)

# Server Definition
server <- function(input, output, session) {
  
  # Render plot
  output$distPlot <- renderPlot({
    set.seed(42)
    x <- rnorm(input$n, input$mean, 1)
    
    hist(x, 
         breaks = 30,
         col = "steelblue",
         border = "white",
         main = paste("Normal Distribution (n =", input$n, ")"),
         xlab = "Value")
  })
}

# Note: To run, use: shinyApp(ui = ui, server = server)

cat("Created basic Shiny app structure\n")
```

### Example: Interactive Scatter with Selection

```r
cat("\n===== SHINY SCATTER WITH SELECTION =====\n\n")

# UI with multiple inputs
ui <- fluidPage(
  titlePanel("Customer Analysis Dashboard"),
  
  sidebarLayout(
    sidebarPanel(
      selectInput("xvar", "X Variable:",
                choices = c("Income", "Age", "Score")),
      selectInput("yvar", "Y Variable:",
                choices = c("Income", "Age", "Score")),
      selectInput("color", "Color By:",
                choices = c("None", "Segment", "Region"))
    ),
    
    mainPanel(
      plotOutput("scatterPlot", 
               click = "plot_click"),
      verbatimTextOutput("click_info")
    )
  )
)

# Server with reactive data
server <- function(input, output, session) {
  
  # Sample data
  data <- reactive({
    data.frame(
      Income = rnorm(500, 60000, 15000),
      Age = rnorm(500, 40, 12),
      Score = rnorm(500, 70, 15),
      Segment = sample(c("Premium", "Standard"), 500, replace = TRUE),
      Region = sample(c("North", "South", "East", "West"), 500, replace = TRUE)
    )
  })
  
  output$scatterPlot <- renderPlot({
    df <- data()
    x_col <- tolower(input$xvar)
    y_col <- tolower(input$yvar)
    
    col <- ifelse(input$color == "None", "black",
                 ifelse(input$color == "Segment", df$Segment, df$Region))
    
    colors <- ifelse(input$color == "None", "steelblue",
                     ifelse(input$color == "Segment", 
                            c("steelblue", "darkorange"),
                            c("green", "blue", "red", "purple")))
    
    plot(df[[x_col]], df[[y_col]], 
         col = col, pch = 19, alpha = 0.6,
         main = paste(input$yvar, "vs", input$xvar),
         xlab = input$xvar,
         ylab = input$yvar)
  })
  
  output$click_info <- renderPrint({
    req(input$plot_click)
    cat("Clicked at:\n")
    print(input$plot_click)
  })
}

cat("Created reactive scatter plot app\n")
```

### Example: Dynamic Plot Updates

```r
cat("\n===== DYNAMIC PLOT UPDATES =====\n\n")

# UI with tabs
ui <- fluidPage(
  titlePanel("Dynamic Analysis"),
  
  tabsetPanel(
    tabPanel("Distribution",
             sidebarLayout(
               sidebarPanel(
                 selectInput("dist", "Distribution:",
                           c("Normal", "Exponential", "Uniform")),
                 numericInput("n_samples", "Samples:", 500)
               ),
               mainPanel(plotOutput("distPlot"))
             )),
    
    tabPanel("Time Series",
             sidebarLayout(
               sidebarPanel(
                 sliderInput("smoothing", "Smoothing:", 
                            min = 0.1, max = 1, value = 0.5)
               ),
               mainPanel(plotOutput("tsPlot"))
             ))
  )
)

server <- function(input, output, session) {
  
  output$distPlot <- renderPlot({
    if (input$dist == "Normal") {
      x <- rnorm(input$n_samples)
    } else if (input$dist == "Exponential") {
      x <- rexp(input$n_samples)
    } else {
      x <- runif(input$n_samples)
    }
    
    hist(x, col = "steelblue", border = "white",
         main = paste(input$dist, "Distribution"))
  })
  
  output$tsPlot <- renderPlot({
    n <- 100
    x <- cumsum(rnorm(n))
    
    plot(x, type = "l", col = "darkgreen",
         main = "Time Series with Smoothing")
    
    # Simple smoothing
    if (input$smoothing > 0) {
      lines(lowess(x, f = input$smoothing), 
            col = "red", lwd = 2)
    }
  })
}

cat("Created tabbed plot app\n")
```

### Real-World Example: Sales Dashboard

```r
cat("\n===== SALES DASHBOARD =====\n\n")

# UI for sales dashboard
ui <--fluidPage(
  theme = "cerulean",
  
  titlePanel(h1("Sales Performance Dashboard", 
                style = "color: steelblue")),
  
  br(),
  
  fluidRow(
    column(4, wellPanel(
      selectInput("region", "Select Region:",
                 choices = c("All", "North", "South", "East", "West"))
    )),
    column(4, wellPanel(
      selectInput("metric", "Performance Metric:",
                 choices = c("Revenue", "Units", "Customers"))
    )),
    column(4, wellPanel(
      dateRangeInput("dates", "Date Range:",
                   start = "2024-01-01",
                   end = "2024-12-31")
    ))
  ),
  
  fluidRow(
    column(8, plotOutput("trendPlot")),
    column(4, plotOutput("piePlot"))
  )
)

server <- function(input, output, session) {
  
  # Generate sales data
  sales_data <- reactive({
    set.seed(42)
    data.frame(
      date = as.Date("2024-01-01") + 0:364,
      region = sample(c("North", "South", "East", "West"), 365, replace = TRUE),
      revenue = rnorm(365, 5000, 1000),
      units = rnorm(365, 100, 20),
      customers = rnorm(365, 50, 10)
    )
  })
  
  output$trendPlot <- renderPlot({
    df <- sales_data()
    
    if (input$region != "All") {
      df <- df[df$region == input$region, ]
    }
    
    agg <- aggregate(df[input$metric] ~ df$date, FUN = sum)
    
    plot(agg[, 1], agg[, 2], type = "l", col = "steelblue",
         main = paste(input$metric, "Over Time"),
         xlab = "Date", ylab = input$metric)
  })
  
  output$piePlot <- renderPlot({
    df <- sales_data()
    
    if (input$region != "All") {
      df <- df[df$region == input$region, ]
    }
    
    agg <- aggregate(df$revenue ~ df$region, FUN = sum)
    
    pie(agg[, 2], labels = agg[, 1], col = rainbow(4),
        main = "Revenue by Region")
  })
}

cat("Created sales dashboard\n")
```

### Real-World Example: Machine Learning Visualization

```r
cat("\n===== ML VISUALIZATION APP =====\n\n")

# UI for ML model visualization
ui <- fluidPage(
  titlePanel("Model Training Visualization"),
  
  sidebarLayout(
    sidebarPanel(
      h4("Model Parameters"),
      sliderInput("train_ratio", "Train/Test Split:",
                 min = 0.5, max = 0.9, value = 0.7),
      sliderInput("neighbors", "K Neighbors:",
                 min = 1, max = 20, value = 5),
      actionButton("train", "Train Model")
    ),
    
    mainPanel(
      tabsetPanel(
        tabPanel("Scatter Plot", plotOutput("scatter")),
        tabPanel("Confusion Matrix", plotOutput("confusion")),
        tabPanel("Variable Importance", plotOutput("importance"))
      )
    )
  )
)

server <- function(input, output, session) {
  
  # Training event
  observeEvent(input$train, {
    
    # Generate classification data
    set.seed(42)
    n <- 500
    df <- data.frame(
      x1 = rnorm(n),
      x2 = rnorm(n),
      class = factor(ifelse(rnorm(n) > 0, "A", "B"))
    )
    
    # Split data
    train_idx <- sample(1:n, n * input$train_ratio)
    train <- df[train_idx, ]
    test <- df[-train_idx, ]
    
    # Simple KNN simulation
    output$scatter <- renderPlot({
      colors <- ifelse(df$class == "A", "steelblue", "darkorange")
      plot(df$x1, df$x2, col = colors, pch = 19,
           main = paste("KNN Classification (K =", input$neighbors, ")"),
           xlab = "Feature 1", ylab = "Feature 2")
    })
    
    # Confusion matrix visualization
    output$confusion <- renderPlot({
      conf <- table(df$class[test_idx],
                 sample(df$class, length(test_idx)))
      
      mosaicplot(conf, col = c("steelblue", "darkorange"),
                main = "Confusion Matrix")
    })
    
    # Variable importance (simulated)
    output$importance <- renderPlot({
      barplot(c(x1 = runif(1, 0.5, 1), 
              x2 = runif(1, 0.3, 0.8)),
             horiz = TRUE, col = "steelblue",
             main = "Variable Importance")
    })
  })
}

cat("Created ML visualization app\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use reactive()** for data that depends on inputs
2. **Isolate()** when you need to prevent reactivity
3. **Use observeEvent()** for button-triggered actions
4. **Modularize** with modules for complex apps
5. **Set bookmarkable** state for shareable URLs

### Common Issues

1. **Plot doesn't update**: Check reactive dependencies
2. **Slow performance**: Use data.table or optimize queries
3. **Memory issues**: Clear large objects in reactive
4. **UI not loading**: Check for JavaScript errors

### Basic vs Reactive vs observeEvent

```r
# Basic - runs once
output$plot <- renderPlot({ plot(data) })

# Reactive - re-runs when input$n changes
data <- reactive({ rnorm(input$n) })
output$plot <- renderPlot({ hist(data()) })

# observeEvent - runs only when button clicked
observeEvent(input$button, {
  model <- train_model()
  output$result <- renderPlot({ plot(model) })
})
```

## Related Concepts

- `shiny` - Core Shiny package
- `plotly` - Interactive Plotly integration
- `ggplot2` - Grammar of graphics with Shiny
- `shinydashboard` - Dashboard layout framework

## Exercise Problems

1. Create a Shiny app that displays different distributions when user selects distribution type
2. Add zoom/brush selection to update a secondary plot
3. Create a dashboard with multiple reactive plots
4. Use observeEvent to add a "Submit" button that triggers analysis
5. Create a reactive data frame that filters based on user input