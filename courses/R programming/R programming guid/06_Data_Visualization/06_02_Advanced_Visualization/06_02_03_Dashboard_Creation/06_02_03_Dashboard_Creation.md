# Dashboard Creation with Shiny

## Learning Objectives

- Build interactive web applications using Shiny
- Understand reactive programming in Shiny
- Create UI and server components
- Add interactive widgets for user input
- Deploy Shiny applications

## Theoretical Background

### What is Shiny?

Shiny is an R package that enables creation of interactive web applications entirely in R. It provides:
- A web framework for R
- Reactive programming model
- Built-in widgets for user interaction
- Integration with R's statistical capabilities

### Shiny Application Structure

A Shiny app consists of two main components:
1. **UI (User Interface)**: Defines the layout and controls
2. **Server**: Contains the R code that runs for each user

### Reactive Programming

Shiny uses a reactive programming model:
- Reactive values automatically update when inputs change
- Reactive expressions only re-evaluate when dependencies change
- This enables efficient, responsive applications

### Dashboard Layouts

Shiny supports various dashboard layouts:
- Single page with sidebar
- Multi-page navigation
- Tabbed panels
- Grid-based layouts

## Code Examples

### Example 1: Basic Shiny App

```r
library(shiny)

# UI - defines the interface
ui <- fluidPage(
  titlePanel("My First Shiny App"),
  sidebarLayout(
    sidebarPanel(
      sliderInput("num", 
                  "Choose a number:",
                  min = 1, max = 100, value = 50)
    ),
    mainPanel(
      verbatimTextOutput("value"),
      plotOutput("hist")
    )
  )
)

# Server - defines the logic
server <- function(input, output) {
  output$value <- renderPrint({
    paste("You selected:", input$num)
  })
  
  output$hist <- renderPlot({
    hist(rnorm(1000, mean = input$num),
         main = paste("Histogram with mean =", input$num),
         xlab = "Value")
  })
}

# Run the app
shinyApp(ui, server)
```

### Example 2: Interactive Data Table

```r
library(shiny)
library(DT)

ui <- fluidPage(
  titlePanel("Data Table Viewer"),
  sidebarLayout(
    sidebarPanel(
      selectInput("dataset", "Choose Dataset:",
                  choices = c("mtcars", "iris", "airquality")),
      numericInput("rows", "Number of rows:", 
                   value = 10, min = 1, max = 50)
    ),
    mainPanel(
      dataTableOutput("table")
    )
  )
)

server <- function(input, output) {
  output$table <- renderDataTable({
    data <- get(input$dataset)
    datatable(head(data, input$rows),
              options = list(pageLength = input$rows))
  })
}

shinyApp(ui, server)
```

### Example 3: Plot with User Controls

```r
library(shiny)
library(ggplot2)

ui <- fluidPage(
  titlePanel("Interactive Plot Creator"),
  sidebarLayout(
    sidebarPanel(
      selectInput("x_var", "X Variable:",
                  choices = names(mtcars)),
      selectInput("y_var", "Y Variable:",
                  choices = names(mtcars)),
      selectInput("color_var", "Color by:",
                  choices = c("None", names(mtcars))),
      checkboxInput("show_points", "Show Points", TRUE),
      checkboxInput("show_smooth", "Add Trend Line", FALSE),
      sliderInput("alpha", "Point Transparency:",
                  min = 0, max = 1, value = 0.7)
    ),
    mainPanel(
      plotOutput("plot")
    )
  )
)

server <- function(input, output) {
  output$plot <- renderPlot({
    p <- ggplot(mtcars, aes(x = .data[[input$x_var]],
                            y = .data[[input$y_var]]))
    
    if(input$color_var != "None") {
      p <- p + aes(color = .data[[input$color_var]])
    }
    
    if(input$show_points) {
      p <- p + geom_point(alpha = input$alpha)
    }
    
    if(input$show_smooth) {
      p <- p + geom_smooth(method = "lm")
    }
    
    p + theme_minimal() +
      labs(title = paste(input$y_var, "vs", input$x_var))
  })
}

shinyApp(ui, server)
```

### Example 4: File Upload and Processing

```r
library(shiny)

ui <- fluidPage(
  titlePanel("Data File Analyzer"),
  sidebarLayout(
    sidebarPanel(
      fileInput("file", "Upload CSV File:",
                accept = c("text/csv", 
                          "text/comma-separated-values",
                          ".csv")),
      checkboxInput("header", "Has Header?", TRUE),
      radioButtons("sep", "Separator:",
                   choices = c(Comma = ",", 
                              Semicolon = ";",
                              Tab = "\t"),
                   selected = ","),
      actionButton("process", "Process Data")
    ),
    mainPanel(
      verbatimTextOutput("summary"),
      tableOutput("preview")
    )
  )
)

server <- function(input, output) {
  observeEvent(input$process, {
    req(input$file)
    
    data <- read.csv(input$file$datapath,
                     header = input$header,
                     sep = input$sep)
    
    output$summary <- renderPrint({
      cat("Dimensions:", nrow(data), "rows,", 
          ncol(data), "columns\n\n")
      cat("Column Types:\n")
      str(data)
    })
    
    output$preview <- renderTable({
      head(data, 10)
    })
  })
}

shinyApp(ui, server)
```

### Example 5: Dynamic UI Generation

```r
library(shiny)

ui <- fluidPage(
  titlePanel("Dynamic Filter Builder"),
  sidebarLayout(
    sidebarPanel(
      selectInput("dataset", "Dataset:",
                  choices = c("mtcars", "iris")),
      uiOutput("filter_ui"),
      actionButton("apply", "Apply Filters")
    ),
    mainPanel(
      plotOutput("plot"),
      tableOutput("data")
    )
  )
)

server <- function(input, output) {
  output$filter_ui <- renderUI({
    data <- get(input$dataset)
    cols <- names(data)[sapply(data, is.numeric)]
    
    tagList(
      lapply(cols, function(col) {
        fluidRow(
          column(6, selectInput(paste0("filter_", col),
                                col,
                                choices = c("All", 
                                          sort(unique(data[[col]])))))
        )
      })
    )
  })
  
  observeEvent(input$apply, {
    data <- get(input$dataset)
    
    output$plot <- renderPlot({
      numeric_cols <- names(data)[sapply(data, is.numeric)]
      plot(data[[numeric_cols[1]]], 
           data[[numeric_cols[2]]],
           pch = 19, col = "steelblue",
           main = paste("Plot from", input$dataset))
    })
    
    output$data <- renderTable({
      head(data, 20)
    })
  })
}

shinyApp(ui, server)
```

### Example 6: Tabbed Dashboard

```r
library(shiny)

ui <- fluidPage(
  titlePanel("Statistical Dashboard"),
  
  navlistPanel(
    "Navigation",
    tabPanel("Summary",
             fluidRow(
               column(6, verbatimTextOutput("summary")),
               column(6, plotOutput("dist_plot"))
             )),
    tabPanel("Data Table",
             dataTableOutput("data_table")),
    tabPanel("Visualization",
             plotOutput("main_plot")),
    tabPanel("About",
             "This dashboard provides interactive analysis.")
  )
)

server <- function(input, output) {
  output$summary <- renderPrint({
    summary(mtcars)
  })
  
  output$dist_plot <- renderPlot({
    hist(mtcars$mpg, breaks = 10,
         main = "MPG Distribution",
         col = "steelblue", border = "white")
  })
  
  output$data_table <- renderDataTable({
    datatable(mtcars)
  })
  
  output$main_plot <- renderPlot({
    pairs(mtcars[, 1:4],
          main = "MTCARS Pairwise Plot")
  })
}

shinyApp(ui, server)
```

### Example 7: Real-time Updates

```r
library(shiny)

ui <- fluidPage(
  titlePanel("Live Data Simulator"),
  sidebarLayout(
    sidebarPanel(
      sliderInput("n_points", "Number of Points:",
                  min = 10, max = 1000, value = 100),
      sliderInput("speed", "Update Speed (seconds):",
                  min = 0.5, max = 5, value = 1),
      actionButton("start", "Start"),
      actionButton("stop", "Stop")
    ),
    mainPanel(
      plotOutput("live_plot"),
      textOutput("status")
    )
  )
)

server <- function(input, output) {
  vals <- reactiveValues(
    data = NULL,
    running = FALSE
  )
  
  observeEvent(input$start, {
    vals$running <- TRUE
  })
  
  observeEvent(input$stop, {
    vals$running <- FALSE
  })
  
  observe({
    if(vals$running) {
      invalidateLater(input$speed * 1000)
      vals$data <- data.frame(
        x = 1:input$n_points,
        y = cumsum(rnorm(input$n_points))
      )
    }
  })
  
  output$live_plot <- renderPlot({
    req(vals$data)
    plot(vals$data, type = "l",
         main = "Live Random Walk",
         col = "steelblue", lwd = 2)
  })
  
  output$status <- renderText({
    if(vals$running) "Running..." else "Stopped"
  })
}

shinyApp(ui, server)
```

### Example 8: Error Handling and Feedback

```r
library(shiny)

ui <- fluidPage(
  titlePanel("Safe Data Processor"),
  sidebarLayout(
    sidebarPanel(
      textInput("url", "Data URL:"),
      actionButton("load", "Load Data"),
      helpText("Enter a valid URL to a CSV file")
    ),
    mainPanel(
      verbatimTextOutput("status"),
      tableOutput("result")
    )
  )
)

server <- function(input, output) {
  data_val <- reactiveVal(NULL)
  
  observeEvent(input$load, {
    output$status <- renderText({
      "Loading..."
    })
    
    tryCatch({
      data_val(read.csv(input$url))
      output$status <- renderText({
        paste("Successfully loaded", 
              nrow(data_val()), "rows")
      })
    }, error = function(e) {
      output$status <- renderText({
        paste("Error:", e$message)
      })
    })
  })
  
  output$result <- renderTable({
    req(data_val())
    head(data_val(), 10)
  })
}

shinyApp(ui, server)
```

### Example 9: CSS Styling

```r
library(shiny)

ui <- fluidPage(
  tags$head(
    tags$style(HTML("
      .well {
        background-color: #f5f5f5;
      }
      .btn-primary {
        background-color: #337ab7;
      }
      h2 {
        color: #2c3e50;
      }
      .shiny-input-container {
        margin-bottom: 15px;
      }
    "))
  ),
  
  titlePanel("Styled Dashboard"),
  
  fluidRow(
    column(4,
           wellPanel(
             h4("Controls"),
             sliderInput("slider", "Value:", 0, 100, 50),
             selectInput("select", "Option:", 
                        choices = c("A", "B", "C"))
           )),
    column(8,
           wellPanel(
             plotOutput("plot")
           )
    )
  )
)

server <- function(input, output) {
  output$plot <- renderPlot({
    hist(rnorm(1000, input$slider, 10),
         main = "Random Distribution",
         col = "steelblue", border = "white")
  })
}

shinyApp(ui, server)
```

### Example 10: Complete Dashboard Layout

```r
library(shiny)
library(ggplot2)
library(DT)

ui <- fluidPage(
  theme = NULL,
  
  # Header
  tags$head(
    tags$style(HTML("
      .navbar { background-color: #2c3e50; }
      .navbar-default .navbar-brand { color: white; }
      .tab-content { padding: 20px; }
    "))
  ),
  
  navbarPage("Analytics Dashboard",
    tabPanel("Overview",
             fluidRow(
               valueBox("Total Users", "15,234", icon("users")),
               valueBox("Revenue", "$45,678", icon("dollar")),
               valueBox("Growth", "23%", icon("arrow-up"))
             ),
             fluidRow(
               column(6, plotOutput("trend_plot")),
               column(6, plotOutput("distribution"))
             )),
    
    tabPanel("Data",
             DTOutput("data_table")),
    
    tabPanel("Analysis",
             sidebarLayout(
               sidebarPanel(
                 h4("Analysis Controls"),
                 selectInput("var", "Variable:", 
                             names(mtcars)),
                 sliderInput("bins", "Histogram Bins:",
                             5, 50, 20),
                 checkboxInput("add_fit", "Add Fit", FALSE)
               ),
               mainPanel(
                 plotOutput("analysis_plot"),
                 verbatimTextOutput("stats")
               )
             )),
    
    tabPanel("Settings",
             wellPanel(
               h4("Application Settings"),
               textInput("title", "Dashboard Title:"),
               actionButton("save", "Save Settings")
             ))
  )
)

server <- function(input, output) {
  output$trend_plot <- renderPlot({
    ggplot(mtcars, aes(x = 1:nrow(mtcars), y = mpg)) +
      geom_line(color = "steelblue") +
      labs(title = "MPG Trend") +
      theme_minimal()
  })
  
  output$distribution <- renderPlot({
    ggplot(mtcars, aes(x = hp)) +
      geom_histogram(bins = 20, fill = "coral") +
      labs(title = "Horsepower Distribution") +
      theme_minimal()
  })
  
  output$data_table <- renderDT({
    datatable(mtcars, options = list(pageLength = 10))
  })
  
  output$analysis_plot <- renderPlot({
    p <- ggplot(mtcars, aes(x = .data[[input$var]], y = mpg)) +
      geom_point() +
      theme_minimal()
    
    if(input$add_fit) {
      p <- p + geom_smooth(method = "lm")
    }
    p
  })
  
  output$stats <- renderPrint({
    summary(mtcars[, input$var])
  })
}

shinyApp(ui, server)
```

## Best Practices

### 1. Reactive Patterns

- Use `reactive()` for expensive calculations
- Avoid unnecessary reactive dependencies
- Use `isolate()` when needed to prevent updates

### 2. Performance Optimization

- Cache expensive computations
- Use `renderUI()` sparingly
- Consider using `data.table` for large datasets

### 3. User Experience

- Add loading indicators for long computations
- Provide feedback on user actions
- Use appropriate validation for inputs

### 4. Code Organization

- Separate UI and server logic in larger apps
- Use modules for reusable components
- Document your app with comments

### 5. Error Handling

- Wrap potentially failing code in `tryCatch()`
- Provide meaningful error messages
- Validate user inputs before processing

### Common Issues

1. Slow reactivity: Use `reactiveTimer()` or `invalidateLater()` efficiently
2. Memory leaks: Clear large objects when no longer needed
3. JavaScript conflicts: Use unique IDs for custom JavaScript
4. Deployment issues: Ensure all packages are in `renv` or DESCRIPTION