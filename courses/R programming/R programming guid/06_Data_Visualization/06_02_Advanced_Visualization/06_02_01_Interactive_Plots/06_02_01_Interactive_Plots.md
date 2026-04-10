# Interactive Plots with plotly and htmlwidgets

## Learning Objectives

- Create interactive plots using the plotly package
- Understand htmlwidgets framework for R
- Convert static ggplot2 plots to interactive visualizations
- Add interactive elements: tooltips, zooming, panning
- Embed interactive plots in R Markdown and Shiny applications

## Theoretical Background

### The plotly Package

Plotly is an R package that creates interactive web-based visualizations. It works by:
1. Creating data visualizations using the plotly API
2. Rendering them as interactive HTML widgets
3. Using JavaScript under the hood for interactivity

### htmlwidgets Framework

The `htmlwidgets` package provides a framework for creating R bindings to JavaScript libraries. It:
- Enables R functions to return interactive HTML elements
- Provides a standardized interface for JavaScript libraries
- Works seamlessly with R Markdown and Shiny

### Advantages of Interactive Plots

- Tooltip information on hover
- Zoom and pan capabilities
- Click events for data exploration
- Animated transitions
- Responsive to screen size

## Code Examples

### Example 1: Basic Interactive Scatter Plot

```r
# Load plotly library
library(plotly)

# Create sample data
set.seed(42)
n <- 100
data <- data.frame(
  x = rnorm(n),
  y = rnorm(n),
  group = factor(sample(c("A", "B", "C"), n, replace = TRUE)),
  size = runif(n, 5, 20),
  text = paste("Point", 1:n, "<br>Value:", round(rnorm(n), 2))
)

# Basic scatter plot
fig <- plot_ly(data, x = ~x, y = ~y,
               type = "scatter",
               mode = "markers",
               marker = list(size = ~size,
                            color = ~group,
                            opacity = 0.7),
               text = ~text,
               hoverinfo = "text")

fig <- fig %>% layout(title = "Interactive Scatter Plot")
fig
```

### Example 2: Interactive Line and Area Charts

```r
# Time series data
dates <- seq(as.Date("2024-01-01"), by = "month", length.out = 12)
sales <- c(100, 120, 135, 130, 145, 160, 175, 170, 185, 195, 210, 230)
costs <- c(60, 65, 70, 68, 72, 75, 80, 78, 82, 85, 90, 95)

ts_data <- data.frame(date = dates, sales = sales, costs = costs)

# Interactive line chart
fig <- plot_ly(ts_data, x = ~date, y = ~sales,
               type = "scatter", mode = "lines+markers",
               name = "Sales",
               line = list(color = "blue", width = 2))

fig <- fig %>% add_trace(y = ~costs,
                         name = "Costs",
                         line = list(color = "red"))

fig <- fig %>% layout(title = "Monthly Sales vs Costs",
                      xaxis = list(title = "Month"),
                      yaxis = list(title = "Amount ($)"),
                      hovermode = "x unified")

fig
```

### Example 3: Interactive Bar Charts with Dropdowns

```r
# Sample data for grouped bar chart
df <- data.frame(
  year = rep(2020:2023, each = 4),
  quarter = rep(c("Q1", "Q2", "Q3", "Q4"), 4),
  revenue = c(100, 120, 110, 130,
              125, 140, 135, 150,
              145, 155, 160, 175,
              170, 180, 185, 200),
  profit = c(20, 25, 22, 28,
             25, 30, 28, 35,
             30, 35, 38, 40,
             35, 40, 42, 45)
)

# Create buttons for switching metrics
fig <- plot_ly(df, x = ~quarter, y = ~revenue,
               type = "bar", name = "Revenue",
               color = I("steelblue"))

fig <- fig %>% add_trace(y = ~profit,
                         name = "Profit",
                         color = I("coral"))

fig <- fig %>% layout(title = "Quarterly Performance",
                      barmode = "group",
                      updatemenus = list(
                        list(
                          type = "buttons",
                          direction = "down",
                          x = 0.1, y = 1.15,
                          buttons = list(
                            list(label = "Revenue",
                                 method = "update",
                                 args = list(list(visible = list(TRUE, FALSE)),
                                             list(title = "Revenue"))),
                            list(label = "Profit",
                                 method = "update",
                                 args = list(list(visible = list(FALSE, TRUE)),
                                             list(title = "Profit"))),
                            list(label = "Both",
                                 method = "update",
                                 args = list(list(visible = list(TRUE, TRUE)),
                                             list(title = "Revenue and Profit")))
                          )
                        )
                      ))

fig
```

### Example 4: 3D Interactive Scatter Plot

```r
# Generate 3D data
set.seed(123)
n <- 200
iris_sample <- iris[sample(1:150, n), ]

# 3D scatter plot
fig <- plot_ly(iris_sample, 
               x = ~Sepal.Length,
               y = ~Sepal.Width,
               z = ~Petal.Length,
               color = ~Species,
               type = "scatter3d",
               mode = "markers",
               marker = list(size = 4, opacity = 0.8))

fig <- fig %>% layout(title = "Iris Dataset - 3D View",
                      scene = list(
                        xaxis = list(title = "Sepal Length"),
                        yaxis = list(title = "Sepal Width"),
                        zaxis = list(title = "Petal Length")
                      ))

fig
```

### Example 5: Converting ggplot2 to plotly

```r
# Load required libraries
library(ggplot2)
library(plotly)

# Create ggplot
p <- ggplot(mtcars, aes(x = wt, y = mpg, 
                        color = factor(cyl), 
                        size = hp)) +
  geom_point() +
  labs(title = "MTCARS: Weight vs MPG",
       x = "Weight (1000 lbs)",
       y = "Miles per Gallon") +
  theme_minimal()

# Convert to interactive plotly
fig <- ggplotly(p)

# Customize tooltip
fig <- fig %>% style(hoverinfo = "x+y+color")

fig
```

### Example 6: Subplots with plotly

```r
# Create multiple subplots
fig <- subplot(
  plot_ly(mtcars, x = ~mpg, type = "histogram",
          title = "Miles per Gallon"),
  plot_ly(mtcars, x = ~disp, type = "histogram",
          title = "Displacement"),
  plot_ly(mtcars, x = ~hp, type = "histogram",
          title = "Horsepower"),
  plot_ly(mtcars, x = ~wt, type = "histogram",
          title = "Weight"),
  nrows = 2, shareX = FALSE, shareY = FALSE,
  margin = 0.05
)

fig <- fig %>% layout(title = "MTCARS Distribution Plots")
fig
```

### Example 7: Interactive Heatmap

```r
# Create matrix data
set.seed(10)
matrix_data <- matrix(rnorm(100), nrow = 10)
rownames(matrix_data) <- paste0("Row", 1:10)
colnames(matrix_data) <- paste0("Col", 1:10)

# Interactive heatmap
fig <- plot_ly(x = colnames(matrix_data),
               y = rownames(matrix_data),
               z = matrix_data,
               type = "heatmap",
               colors = "RdBu",
               hoverongaps = FALSE)

fig <- fig %>% layout(title = "Correlation Heatmap")
fig
```

### Example 8: Shiny Integration

```r
# Example Shiny app with plotly
# UI
library(shiny)
library(plotly)

ui <- fluidPage(
  titlePanel("Interactive Plotly in Shiny"),
  sidebarLayout(
    sidebarPanel(
      selectInput("x_var", "X Variable:",
                  choices = names(mtcars)),
      selectInput("color_var", "Color by:",
                  choices = c("None", names(mtcars))),
      sliderInput("point_size", "Point Size:",
                  min = 1, max = 10, value = 5)
    ),
    mainPanel(
      plotlyOutput("scatterPlot")
    )
  )
)

server <- function(input, output) {
  output$scatterPlot <- renderPlotly({
    fig <- plot_ly(mtcars, x = ~get(input$x_var), y = ~mpg)
    
    if(input$color_var != "None") {
      fig <- fig %>% add_markers(
        y = ~mpg,
        x = ~get(input$x_var),
        color = ~get(input$color_var),
        size = input$point_size
      )
    } else {
      fig <- fig %>% add_markers(
        y = ~mpg,
        x = ~get(input$x_var),
        size = input$point_size
      )
    }
    
    fig %>% layout(
      xaxis = list(title = input$x_var),
      yaxis = list(title = "Miles per Gallon")
    )
  })
}

shinyApp(ui, server)
```

### Example 9: Customizing Tooltips and Hover

```r
# Create data with custom hover text
df <- data.frame(
  x = 1:10,
  y = (1:10)^2,
  name = LETTERS[1:10],
  value = round(runif(10, 100, 1000)),
  category = c("A", "B", "A", "B", "A", "B", "A", "B", "A", "B")
)

# Custom hover template
fig <- plot_ly(df, x = ~x, y = ~y)

fig <- fig %>% add_trace(
  type = "scatter",
  mode = "markers+text",
  text = ~name,
  textposition = "top center",
  marker = list(size = 15, color = ~category,
                colors = c("A" = "steelblue", "B" = "coral")),
  hovertemplate = paste(
    "<b>%{text}</b><br>",
    "X: %{x}<br>",
    "Y: %{y}<br>",
    "Value: %{customdata}",
    "<extra></extra>"
  ),
  customdata = ~value
)

fig <- fig %>% layout(title = "Custom Hover Template")
fig
```

### Example 10: Animated Plots

```r
# Animated scatter plot
fig <- plot_ly(
  mtcars,
  x = ~wt,
  y = ~mpg,
  frame = ~factor(cyl),
  text = ~paste("Weight:", wt, "<br>MPG:", mpg),
  type = "scatter",
  mode = "markers",
  marker = list(size = 10, color = ~hp,
                colorscale = "Viridis",
                showscale = TRUE)
)

fig <- fig %>% layout(
  title = "MTCARS: Animated by Cylinders",
  xaxis = list(title = "Weight"),
  yaxis = list(title = "Miles per Gallon")
)

fig
```

## Best Practices

### 1. Optimize Tooltip Information

Include relevant data in tooltips without overwhelming:
```r
hovertemplate = "X: %{x}<br>Y: %{y}<extra></extra>"
```

### 2. Use ggplotly() When Possible

Convert existing ggplot2 code for quick interactive versions, then customize as needed.

### 3. Configure Plotly Theme

Set consistent styling:
```r
layout(template = "plotly_white")
```

### 4. Handle Large Datasets

For large datasets, consider:
- Down-sampling before plotting
- Using webgl rendering (`type = "scattergl"`)

### 5. Testing and Rendering

Test interactive plots in RStudio before embedding. Ensure proper rendering in target output format.

### Common Issues

1. Large file sizes: Use `htmlwidgets::saveWidget()` with `self_contained = FALSE`
2. Missing interactivity in R Markdown: Ensure `htmltools` is loaded
3. JavaScript conflicts: Use `partial_bundle()` for minimal dependencies
4. Tooltip overflow: Customize with `hovertemplate`