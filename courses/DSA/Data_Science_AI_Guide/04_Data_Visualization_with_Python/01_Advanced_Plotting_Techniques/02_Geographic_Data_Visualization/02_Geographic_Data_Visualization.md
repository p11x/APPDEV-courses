# Geographic Data Visualization

## I. INTRODUCTION

### What is Geographic Data Visualization?

Geographic data visualization (geovisualization) is the practice of representing data with spatial or geographic components through visual representations. This specialized field combines cartography, data visualization, and geographic information systems (GIS) to reveal patterns, relationships, and trends in data that have a geographic dimension.

Geographic visualization encompasses techniques for creating maps, choropleth maps, bubble maps, heat maps, Cartograms, and 3D terrain visualizations. It enables organizations to understand spatial patterns in their data, from customer distribution to disease spread, from sales territory performance to environmental monitoring.

### Why is Geographic Data Visualization Important?

1. **Spatial Pattern Recognition**: Many phenomena are best understood through their geographic distribution - population density, climate patterns, disease outbreaks, economic activity.

2. **Location-Based Decision Making**: Businesses use geographic visualization for site selection, territory management, market analysis, and resource allocation.

3. **Demographic Analysis**: Understanding the geographic distribution of populations, demographics, and characteristics is essential for public policy and marketing.

4. **Navigation and Wayfinding**: Maps provide essential context for understanding routes, distances, and geographic relationships.

5. **Crisis Response**: During natural disasters or health emergencies, geographic visualization helps coordinate response efforts.

### Prerequisites

- Basic Python programming skills
- Understanding of pandas DataFrames
- Familiarity with coordinate systems (latitude/longitude)
- Knowledge of basic statistics
- Installed libraries: matplotlib, seaborn, plotly, pandas, geopandas (optional), folium (optional)

## II. FUNDAMENTALS

### Basic Concepts and Definitions

**Latitude and Longitude**: The geographic coordinate system used to specify locations on Earth. Latitude measures north-south position; longitude measures east-west position.

**Choropleth Map**: A thematic map where areas are colored or shaded according to a statistical variable (e.g., population density, income).

**Proportional Symbol Map**: Uses symbols (typically circles) sized in proportion to the data values at each location.

**Heat Map**: A visualization showing the density of data points in a geographic area, typically using color intensity.

**Cartogram**: A map where areas are distorted to represent a statistical variable (e.g., population, GDP).

**Basemap**: The background map layer showing geographic features like coastlines, roads, and boundaries.

### Key Terminology

| Term | Definition |
|------|------------|
| GeoJSON | Format for encoding geographic data structures |
| Shapefile | Esri's vector data format for geographic information |
| Projection | Method of representing 3D Earth on 2D surface |
| Scale | Ratio between map distance and actual distance |
| Legend | Key explaining the meaning of map symbols/colors |
| Attribution | Metadata about geographic features |
| Tile | Square image piece for web mapping |

### Core Principles

1. **Choose Appropriate Map Type**: Different data requires different map types - categorical data needs choropleth, point data needs symbol maps.

2. **Use Appropriate Projection**: Different map projections preserve different properties (area, shape, distance, direction).

3. **Design for Clarity**: Maps should be easily readable without clutter - avoid overplotting.

4. **Include Context**: Basemaps, labels, and reference points help users understand the map.

5. **Consider Scale**: The level of geographic detail should match the analysis purpose.

## III. IMPLEMENTATION

### Step-by-Step Code Examples

#### Setup and Imports

```python
# Required libraries
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import plotly.express as px
import plotly.graph_objects as go
from plotly.subplots import make_subplots
import warnings
warnings.filterwarnings('ignore')

# Set plotting style
plt.style.use('seaborn-v0_8-whitegrid')
```

#### Creating Basic Choropleth Maps with Plotly

```python
def create_choropleth_map_example():
    """
    Create a choropleth map showing data values by geographic region.
    Choropleth maps use color intensity to represent values.
    """
    # Generate sample data - GDP by country
    np.random.seed(42)
    
    # Sample of countries with their data
    data = pd.DataFrame({
        'Country': ['United States', 'China', 'Japan', 'Germany', 'United Kingdom',
                   'India', 'France', 'Italy', 'Brazil', 'Canada', 'Russia', 'Korea',
                   'Australia', 'Spain', 'Mexico', 'Indonesia', 'Netherlands', 
                   'Saudi Arabia', 'Turkey', 'Switzerland'],
        'GDP_Billions': [23320, 17730, 4940, 4230, 3190, 3170, 2940, 2110, 1930, 1890,
                        1780, 1710, 1550, 1430, 1290, 1190, 1010, 792, 760, 709],
        'Population_Millions': [331, 1412, 126, 84, 67, 1380, 67, 60, 214, 38,
                                 144, 52, 26, 47, 128, 275, 17, 35, 84, 9],
        'GDP_Per_Capita': [70400, 12550, 39200, 50300, 47600, 2300, 43900, 35100, 9010,
                        49700, 12300, 32900, 59700, 30400, 10100, 4330, 59500, 
                        22700, 9040, 83500]
    })
    
    # Calculate GDP growth rate (simulated)
    data['GDP_Growth'] = np.random.uniform(-2, 8, len(data))
    
    # Create choropleth map using Plotly
    fig = px.choropleth(
        data,
        locations='Country',
        locationmode='country names',
        color='GDP_Billions',
        hover_name='Country',
        hover_data={
            'GDP_Billions': ':.1f',
            'Population_Millions': ':.1f',
            'GDP_Per_Capita': ':.0f',
            'GDP_Growth': ':.1f'
        },
        color_continuous_scale='Viridis',
        title='Global GDP by Country (Billions USD)',
        projection='natural earth'
    )
    
    # Update layout for better appearance
    fig.update_layout(
        geo=dict(
            showframe=True,
            showcoastlines=True,
            projection_type='orthographic',
            coastlinecolor='DarkGray',
            showland=True,
            landcolor='LightGray',
            showocean=True,
            oceancolor='LightBlue',
            showlakes=True,
            lakecolor='LightBlue'
        ),
        title_font_size=18,
        title_x=0.5,
        coloraxis_colorbar=dict(
            title='GDP (Billions $)',
            tickprefix='$'
        )
    )
    
    return fig, data

# Execute
fig_gdp, df_gdp = create_choropleth_map_example()
fig_gdp.show()
```

#### Creating Bubble Maps

```python
def create_bubble_map_example():
    """
    Create a bubble map where circle size represents data values.
    Ideal for showing magnitudes at specific geographic locations.
    """
    # Generate sample data - major cities with population
    np.random.seed(42)
    
    data = pd.DataFrame({
        'City': ['New York', 'Los Angeles', 'Chicago', 'Houston', 'Phoenix',
                'London', 'Paris', 'Tokyo', 'Shanghai', 'Beijing', 
                'Sydney', 'Mumbai', 'Dubai', 'Singapore', 'Toronto'],
        'Country': ['USA', 'USA', 'USA', 'USA', 'USA',
                   'UK', 'France', 'Japan', 'China', 'China',
                   'Australia', 'India', 'UAE', 'Singapore', 'Canada'],
        'Latitude': [40.71, 34.05, 41.88, 29.76, 33.45,
                    51.51, 48.86, 35.68, 31.23, 39.90,
                    -33.87, 19.08, 25.20, 1.35, 43.65],
        'Longitude': [-74.01, -118.24, -87.63, -95.37, -112.01,
                     -0.13, 2.35, 139.65, 121.47, 116.41,
                     151.21, 72.88, 55.27, 103.82, -79.38],
        'Population': [8336, 3979, 2697, 2320, 1680,
                      8981, 2161, 13960, 24280, 20700,
                      5312, 20411, 3339, 5682, 2731],
        'GDP_Billions': [1210, 860, 440, 350, 240,
                        731, 564, 1390, 397, 489,
                        337, 310, 78, 337, 276]
    })
    
    # Create bubble map
    fig = px.scatter_geo(
        data,
        lat='Latitude',
        lon='Longitude',
        size='Population',
        color='GDP_Billions',
        hover_name='City',
        hover_data={'Latitude': ':.2f', 'Longitude': ':.2f'},
        color_continuous_scale='Blues',
        size_max=50,
        title='Major Cities: Population (Size) and GDP (Color)',
        projection='natural earth'
    )
    
    # Update layout
    fig.update_layout(
        geo=dict(
            showland=True,
            landcolor='rgb(243, 243, 243)',
            showocean=True,
            oceancolor='rgb(204, 229, 255)',
            showcountries=True,
            countrycolor='rgb(204, 204, 204)',
            showcoastlines=True,
            coastlinecolor='rgb(204, 204, 204)',
            projection_type='equirectangular'
        ),
        title_font_size=16,
        title_x=0.5
    )
    
    return fig, data

fig_bubble, df_bubble = create_bubble_map_example()
fig_bubble.show()
```

#### Creating US State Choropleth Maps

```python
def create_us_state_map_example():
    """
    Create a choropleth map for US states.
    Uses state FIPS codes for accurate geographic binding.
    """
    # Generate sample data - US state economic data
    np.random.seed(42)
    
    # State FIPS codes and economic data
    state_data = {
        'State': ['CA', 'TX', 'NY', 'FL', 'IL', 'PA', 'OH', 'GA', 'NC', 'MI',
                 'NJ', 'VA', 'WA', 'AZ', 'MA', 'TN', 'IN', 'MO', 'MD', 'WI',
                 'CO', 'MN', 'SC', 'AL', 'LA', 'KY', 'OR', 'OK', 'CT', 'IA',
                 'UT', 'NV', 'AR', 'MS', 'KS', 'NM', 'NE', 'WV', 'ID', 'HI',
                 'NH', 'ME', 'RI', 'MT', 'DE', 'SD', 'ND', 'AK', 'VT', 'WY', 
                 'DC'],
        'State_Name': ['California', 'Texas', 'New York', 'Florida', 'Illinois', 
                      'Pennsylvania', 'Ohio', 'Georgia', 'North Carolina', 'Michigan',
                      'New Jersey', 'Virginia', 'Washington', 'Arizona', 
                      'Massachusetts', 'Tennessee', 'Indiana', 'Missouri', 'Maryland', 
                      'Wisconsin', 'Colorado', 'Minnesota', 'South Carolina', 
                      'Alabama', 'Louisiana', 'Kentucky', 'Oregon', 'Oklahoma', 
                      'Connecticut', 'Iowa', 'Utah', 'Nevada', 'Arkansas', 'Mississippi',
                      'Kansas', 'New Mexico', 'Nebraska', 'West Virginia', 'Idaho',
                      'Hawaii', 'New Hampshire', 'Maine', 'Rhode Island', 'Montana',
                      'Delaware', 'South Dakota', 'North Dakota', 'Alaska', 'Vermont',
                      'Wyoming', 'District of Columbia'],
        'GDP_Billions': [3038, 1836, 1708, 1340, 892, 764, 666, 584, 560, 524,
                        517, 500, 446, 409, 403, 369, 358, 318, 317, 312,
                        282, 268, 252, 247, 242, 229, 224, 195, 190, 176,
                        171, 161, 151, 149, 143, 103, 101, 84, 83, 82,
                        79, 68, 58, 52, 38, 35, 33, 31, 29, 26, 14],
        'Population': [39538, 29146, 19454, 21538, 12580, 12792, 11689, 10623, 10488,
                      9958, 8882, 8580, 7615, 7158, 6895, 6729, 6733, 6135, 6046,
                      5822, 5788, 5464, 5149, 4903, 4640, 4447, 3953, 3956, 3555,
                      3206, 3104, 3018, 2975, 2914, 2098, 1935, 1785, 1787, 1379,
                      1362, 1344, 1059, 1069, 973, 884, 762, 731, 643, 578, 689]
    }
    
    df_state = pd.DataFrame(state_data)
    df_state['GDP_Per_Capita'] = (df_state['GDP_Billions'] * 1000 / df_state['Population']).astype(int)
    df_state['GDP_Growth'] = np.random.uniform(0.5, 5.5, len(df_state))
    
    # Create US state choropleth
    fig = px.choropleth(
        df_state,
        locations='State',
        locationmode='USA-states',
        color='GDP_Billions',
        hover_name='State_Name',
        hover_data={
            'GDP_Billions': ':.1f',
            'Population': ':.0f',
            'GDP_Per_Capita': ':',
            'GDP_Growth': ':.1f'
        },
        color_continuous_scale='Blues',
        scope='usa',
        title='US State GDP (Billions USD)'
    )
    
    fig.update_layout(
        geo=dict(
            showlakes=True,
            lakecolor='rgb(255, 255, 255)',
            showsubunits=True,
            subunitcolor='rgb(200, 200, 200)'
        ),
        title_font_size=18,
        title_x=0.5
    )
    
    return fig, df_state

fig_us, df_us = create_us_state_map_example()
fig_us.show()
```

#### Creating Animated Geographic Visualizations

```python
def create_animated_geo_example():
    """
    Create animated geographic visualization showing data changes over time.
    Useful for tracking trends across multiple time periods.
    """
    # Generate time-series geographic data
    np.random.seed(42)
    
    countries = ['USA', 'China', 'Germany', 'Japan', 'UK', 'India', 'France', 'Brazil', 'Canada', 'Australia']
    
    # Create data for multiple years
    years = list(range(2015, 2024))
    data_frames = []
    
    for year in years:
        for country in countries:
            base_gdp = {'USA': 18000, 'China': 11000, 'Germany': 3500, 'Japan': 4500,
                       'UK': 2500, 'India': 2000, 'France': 2400, 'Brazil': 1500,
                       'Canada': 1500, 'Australia': 1200}
            
            df = pd.DataFrame({
                'Year': [year] * len(countries),
                'Country': countries,
                'GDP': [base_gdp[c] * (1 + np.random.uniform(0.02, 0.06)) ** (year - 2015) 
                        for c in countries],
                'Stock_Index': [base_gdp[c] * (1 + np.random.uniform(-0.1, 0.15)) ** (year - 2015)
                             for c in countries]
            })
            data_frames.append(df)
    
    data = pd.concat(data_frames, ignore_index=True)
    
    # Create animated choropleth
    fig = px.choropleth(
        data,
        locations='Country',
        locationmode='country names',
        color='GDP',
        animation_frame='Year',
        hover_name='Country',
        hover_data={'Stock_Index': ':.0f'},
        color_continuous_scale='Viridis',
        title='Global GDP Growth (2015-2023)',
        projection='natural earth'
    )
    
    # Update animation settings
    fig.layout.updatemenus[0].buttons[0].args[1]['frame']['duration'] = 800
    fig.layout.updatemenus[0].buttons[0].args[1]['transition']['duration'] = 400
    
    fig.update_layout(
        geo=dict(
            projection_type='natural earth',
            showland=True,
            showocean=True
        ),
        title_x=0.5
    )
    
    return fig, data

fig_anim, df_anim = create_animated_geo_example()
fig_anim.show()
```

### Using Folium for Interactive Maps

```python
def create_folium_map_example():
    """
    Create interactive maps using Folium.
    Best for mapping applications requiring user interaction.
    """
    try:
        import folium
        from folium import plugins
        
        # Create base map centered on a location
        m = folium.Map(
            location=[39.8283, -98.5795],  # Center of US
            zoom_start=4,
            tiles='cartodbpositron'
        )
        
        # Add marker clusters for locations
        locations = [
            (40.7128, -74.0060, 'New York City', 'Financial Hub'),
            (34.0522, -118.2437, 'Los Angeles', 'Entertainment Hub'),
            (41.8781, -87.6298, 'Chicago', 'Midwest Center'),
            (29.7604, -95.3698, 'Houston', 'Energy Hub'),
            (33.4484, -112.0740, 'Phoenix', 'Growing City'),
            (51.5074, -0.1278, 'London', 'Financial Hub'),
            (48.8566, 2.3522, 'Paris', 'Cultural Hub'),
            (35.6762, 139.6503, 'Tokyo', 'Tech Hub')
        ]
        
        for lat, lon, city, desc in locations:
            folium.CircleMarker(
                location=[lat, lon],
                radius=10,
                popup=f'<b>{city}</b><br>{desc}',
                color='blue',
                fill=True,
                fillColor='blue',
                fillOpacity=0.6
            ).add_to(m)
        
        # Add title
        title_html = '''
        <div style="position: fixed; 
                    top: 10px; left: 50px; width: 250px;
                    border:2px solid grey; z-index:9999; 
                    font-size:14px; padding: 10px;
                    background-color: white;">
            <b>Major Business Centers</b>
        </div>
        '''
        m.get_root().html.add_child(folium.Element(title_html))
        
        return m, locations
        
    except ImportError:
        print("Folium not installed. Install with: pip install folium")
        return None, None

# If folium available
# fig_folium, locs = create_folium_map_example()
```

### Best Practices

1. **Use Appropriate Geographic Binding**: Ensure location data matches geographic identifiers correctly (country names, FIPS codes, ISO codes).

2. **Choose Color Scales Carefully**: Use sequential scales for continuous data, diverging scales for data with meaningful center point.

3. **Include Basemaps**: Provide geographic context with reference features.

4. **Handle Missing Data**: Clearly indicate regions with missing data - don't let them be confused with zero values.

5. **Optimize for Performance**: For large datasets, consider aggregation or sampling.

## IV. APPLICATIONS

### Standard Example: Global Population Analysis

```python
def global_population_analysis():
    """
    Standard example: Analyze and visualize global population data.
    Shows multiple geographic visualization techniques.
    """
    np.random.seed(42)
    
    # Create comprehensive global dataset
    data = pd.DataFrame({
        'Country': ['China', 'India', 'United States', 'Indonesia', 'Pakistan', 'Brazil',
                   'Nigeria', 'Bangladesh', 'Russia', 'Mexico', 'Japan', 'Ethiopia', 'Philippines',
                   'Egypt', 'Vietnam', 'DR Congo', 'Turkey', 'Iran', 'Germany', 'Thailand'],
        'Population_Millions': [1412, 1380, 331, 275, 220, 214,
                               206, 164, 144, 128, 126, 115, 110,
                               102, 97, 89, 84, 84, 83, 70],
        'Area_km2': [9596961, 3287263, 9833520, 1904569, 881912, 8515767,
                   923768, 147570, 17098242, 1964375, 377975, 1104300, 300000,
                   1002450, 331212, 2344858, 783356, 1648195, 357022, 513120],
        'GDP_Per_Capita': [12550, 2300, 70400, 4330, 1570, 9010,
                       2230, 1810, 12300, 10100, 39200, 1020, 3490,
                       3020, 3510, 478, 9040, 45800, 50300, 7060]
    })
    
    # Add derived metrics
    data['Population_Density'] = data['Population_Millions'] * 1000 / data['Area_km2']
    data['GDP_Total'] = data['Population_Millions'] * data['GDP_Per_Capita']
    
    # Create comprehensive visualization
    fig = make_subplots(
        rows=2, cols=2,
        specs=[[{'type': 'choropleth'}, {'type': 'scattergeo'}],
               [{'type': 'bar'}, {'type': 'scatter'}]],
        subplot_titles=('Population by Country', 'Population vs GDP Per Capita',
                     'Top 10 Countries by GDP', 'Population vs Density'),
        vertical_spacing=0.12,
        horizontal_spacing=0.1
    )
    
    # 1. Choropleth - Population
    fig.add_trace(
        go.Choropleth(
            locations=data['Country'],
            locationmode='country names',
            z=data['Population_Millions'],
            colorscale='Blues',
            text=data['Country'] + '<br>Pop: ' + data['Population_Millions'].astype(str) + 'M',
            colorbar=dict(title='Pop (M)', x=0.45, len=0.4, y=0.8)
        ),
        row=1, col=1
    )
    
    # 2. Scatter geo - Population vs GDP
    scatter_data = data.copy()
    scatter_data['text'] = scatter_data['Country'] + '<br>Pop: ' + \
                          scatter_data['Population_Millions'].astype(str) + 'M<br>GDP/cap: $' + \
                          scatter_data['GDP_Per_Capita'].astype(str)
    
    fig.add_trace(
        go.Scattergeo(
            lat=[40.7128, 34.0522, 41.8781, -33.8688, 51.5074],
            lon=[-74.0060, -118.2437, -87.6298, 151.2093, -0.1278],
            mode='markers+text',
            marker=dict(size=scatter_data['Population_Millions'][:5]/30, color='blue'),
            text=scatter_data['Country'][:5],
            textposition='top center',
            name='Major Cities'
        ),
        row=1, col=2
    )
    
    # 3. Bar chart - Top GDP countries
    top_gdp = data.nlargest(10, 'GDP_Total')
    fig.add_trace(
        go.Bar(
            x=top_gdp['Country'],
            y=top_gdp['GDP_Total'] / 1000,
            name='GDP',
            marker_color='steelblue'
        ),
        row=2, col=1
    )
    
    # 4. Scatter - Population vs Density
    fig.add_trace(
        go.Scatter(
            x=data['Population_Density'],
            y=data['GDP_Per_Capita'],
            mode='markers+text',
            marker=dict(
                size=data['Population_Millions']/20,
                color=data['Population_Density'],
                colorscale='Viridis',
                showscale=True
            ),
            text=data['Country'],
            textposition='top center'
        ),
        row=2, col=2
    )
    
    # Update layout
    fig.update_layout(
        title_text='Global Population and Economic Analysis',
        showlegend=False,
        height=900
    )
    
    fig.update_geos(
        scope='world',
        projection_type='natural earth',
        showland=True,
        showocean=True
    )
    
    # Print summary
    print("=" * 60)
    print("GLOBAL POPULATION ANALYSIS SUMMARY")
    print("=" * 60)
    print(f"\nTotal Countries: {len(data)}")
    print(f"Total Population: {data['Population_Millions'].sum():,.0f} Million")
    print(f"Average GDP Per Capita: ${data['GDP_Per_Capita'].mean():,.0f}")
    print(f"Highest Population: {data.loc[data['Population_Millions'].idxmax(), 'Country']}")
    print(f"Highest GDP Per Capita: {data.loc[data['GDP_Per_Capita'].idxmax(), 'Country']}")
    
    return fig, data

fig_pop, df_pop = global_population_analysis()
fig_pop.show()
```

### Real-world Example 1: Banking/Finance Domain

```python
def banking_branch_performance():
    """
    Real-world example: Analyzing bank branch performance across regions.
    Shows geographic performance analysis for financial institutions.
    """
    np.random.seed(42)
    
    # Create bank branch data
    cities_data = {
        'City': ['New York', 'Los Angeles', 'Chicago', 'Houston', 'Phoenix',
                'San Francisco', 'Miami', 'Boston', 'Atlanta', 'Dallas',
                'Seattle', 'Denver', 'Portland', 'Philadelphia', 'Detroit'],
        'State': ['NY', 'CA', 'IL', 'TX', 'AZ', 'CA', 'FL', 'GA', 'TX', 'TX',
                 'WA', 'CO', 'OR', 'PA', 'MI'],
        'Lat': [40.71, 34.05, 41.88, 29.76, 33.45, 37.77, 25.76, 42.36, 33.75, 32.78,
               47.61, 39.74, 45.52, 39.95, 42.33],
        'Lon': [-74.01, -118.24, -87.63, -95.37, -112.01, -122.42, -80.19, -71.06, 
                -84.39, -96.80, -122.33, -104.99, -122.68, -75.17, -83.05],
        'Branches': [45, 38, 32, 28, 22, 20, 25, 18, 22, 18,
                    15, 14, 12, 16, 14],
        'Total_Assets_Billions': [185, 142, 98, 76, 58, 52, 48, 42, 44, 38,
                                32, 28, 22, 26, 24],
        'NIM': [3.2, 3.5, 3.1, 3.8, 3.4, 2.9, 3.6, 2.8, 3.5, 3.7,
               3.0, 3.3, 3.2, 2.7, 3.4],
        'NPL_Ratio': [1.2, 0.8, 1.4, 1.6, 1.1, 0.6, 1.3, 0.7, 1.5, 1.8,
                    0.9, 1.0, 1.1, 1.2, 1.7],
        'Customer_Satisfaction': [4.1, 4.3, 3.9, 4.0, 4.2, 4.5, 3.8, 4.4, 3.9,
                                 3.7, 4.3, 4.1, 4.0, 4.2, 3.6]
    }
    
    df_bank = pd.DataFrame(cities_data)
    df_bank['Revenue_Per_Branch'] = df_bank['Total_Assets_Billions'] * 1000 / df_bank['Branches']
    
    # Create comprehensive banking visualization
    fig = make_subplots(
        rows=2, cols=2,
        specs=[[{'type': 'choropleth'}, {'type': 'scattergeo'}],
               [{'type': 'bar'}, {'type': 'scatter'}]],
        subplot_titles=('Bank Assets by Region (USA)', 'Branch Locations',
                     'Assets by City', 'NIM vs NPL Ratio'),
        vertical_spacing=0.12,
        horizontal_spacing=0.08
    )
    
    # 1. US State Choropleth
    fig.add_trace(
        go.Choropleth(
            locations=df_bank['State'],
            locationmode='USA-states',
            z=df_bank['Total_Assets_Billions'],
            colorscale='Greens',
            colorbar=dict(title='Assets ($B)', x=0.45, len=0.4, y=0.8)
        ),
        row=1, col=1
    )
    
    # 2. Scatter Geo - Branch locations
    fig.add_trace(
        go.Scattergeo(
            lat=df_bank['Lat'],
            lon=df_bank['Lon'],
            mode='markers',
            marker=dict(
                size=df_bank['Branches'] * 1.5,
                color=df_bank['Total_Assets_Billions'],
                colorscale='Greens',
                line_color='darkgreen',
                line_width=1
            ),
            text=df_bank['City'] + ', ' + df_bank['State'] + '<br>Branches: ' + \
                df_bank['Branches'].astype(str) + '<br>Assets: $' + \
                df_bank['Total_Assets_Billions'].astype(str) + 'B',
            hoverinfo='text'
        ),
        row=1, col=2
    )
    
    # 3. Bar chart - Top cities by assets
    fig.add_trace(
        go.Bar(
            x=df_bank.sort_values('Total_Assets_Billions', ascending=False)['City'],
            y=df_bank.sort_values('Total_Assets_Billions', ascending=False)['Total_Assets_Billions'],
            marker_color='green'
        ),
        row=2, col=1
    )
    
    # 4. Scatter - NIM vs NPL
    fig.add_trace(
        go.Scatter(
            x=df_bank['NIM'],
            y=df_bank['NPL_Ratio'],
            mode='markers+text',
            marker=dict(
                size=df_bank['Total_Assets_Billions'],
                color=df_bank['Customer_Satisfaction'],
                colorscale='RdYlGn',
                line_color='black',
                line_width=0.5
            ),
            text=df_bank['City'],
            textposition='top center'
        ),
        row=2, col=2
    )
    
    fig.update_layout(
        title_text='Bank Branch Performance Geographic Analysis',
        showlegend=False,
        height=900
    )
    
    fig.update_geos(
        scope='usa',
        showland=True,
        showlakes=True,
        subunitcolor='rgb(200, 200, 200)'
    )
    
    # Print summary
    print("=" * 60)
    print("BANK BRANCH PERFORMANCE ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Cities: {len(df_bank)}")
    print(f"Total Branches: {df_bank['Branches'].sum()}")
    print(f"Total Assets: ${df_bank['Total_Assets_Billions'].sum():.1f} Billion")
    print(f"Average NIM: {df_bank['NIM'].mean():.2f}%")
    print(f"Average NPL Ratio: {df_bank['NPL_Ratio'].mean():.2f}%")
    print(f"Average Customer Satisfaction: {df_bank['Customer_Satisfaction'].mean():.1f}/5")
    
    return fig, df_bank

fig_bank_perf, df_bank_perf = banking_branch_performance()
fig_bank_perf.show()
```

### Real-world Example 2: Healthcare Domain

```python
def healthcare_facility_analysis():
    """
    Real-world example: Analyzing healthcare facility distribution and outcomes.
    Shows geographic analysis for healthcare systems.
    """
    np.random.seed(42)
    
    # Create healthcare facility data
    data = pd.DataFrame({
        'State': ['CA', 'TX', 'FL', 'NY', 'PA', 'IL', 'OH', 'GA', 'NC', 'MI',
                 'NJ', 'VA', 'WA', 'AZ', 'MA', 'TN', 'IN', 'MO', 'MD', 'WI'],
        'State_Name': ['California', 'Texas', 'Florida', 'New York', 'Pennsylvania', 
                    'Illinois', 'Ohio', 'Georgia', 'North Carolina', 'Michigan',
                    'New Jersey', 'Virginia', 'Washington', 'Arizona', 
                    'Massachusetts', 'Tennessee', 'Indiana', 'Missouri', 
                    'Maryland', 'Wisconsin'],
        'Hospitals': [450, 420, 310, 280, 220, 195, 195, 155, 145, 145,
                     125, 95, 110, 95, 85, 115, 135, 110, 60, 85],
        'Beds_Per_1000': [1.8, 2.1, 2.5, 2.8, 2.0, 2.0, 2.5, 2.1, 2.0, 2.2,
                       2.3, 1.8, 1.7, 1.9, 2.5, 2.8, 2.3, 2.5, 1.7, 1.9],
        'Doctors_Per_1000': [2.8, 2.0, 2.4, 3.5, 2.9, 2.8, 2.5, 2.3, 2.6, 2.7,
                          3.0, 2.6, 2.9, 2.5, 4.2, 2.5, 2.2, 2.3, 3.1, 2.6],
        'Avg_Wait_Time_Days': [4.2, 5.8, 3.9, 6.2, 4.5, 4.8, 4.0, 4.2, 3.8, 4.5,
                           4.0, 3.5, 3.2, 3.5, 4.8, 5.2, 3.8, 4.2, 3.2, 3.0],
        'Quality_Score': [85, 78, 80, 82, 84, 79, 81, 77, 83, 80,
                        86, 82, 88, 79, 90, 76, 78, 79, 87, 84],
        'Medicaid_Coverage': [75, 65, 68, 72, 70, 68, 72, 65, 68, 70,
                        72, 65, 62, 68, 72, 68, 65, 70, 72, 66]
    })
    
    data['Cost_Per_Capita'] = data['Beds_Per_1000'] * 2000 + data['Doctors_Per_1000'] * 3000
    data['Cost_Per_Capita'] += np.random.normal(0, 500, len(data))
    
    # Create comprehensive healthcare visualization
    fig = make_subplots(
        rows=2, cols=2,
        specs=[[{'type': 'choropleth'}, {'type': 'bar'}],
               [{'type': 'scatter'}, {'type': 'scatter'}]],
        subplot_titles=('Healthcare Quality by State', 'Top States by Quality',
                     'Quality vs Wait Time', 'Quality vs Cost'),
        vertical_spacing=0.12,
        horizontal_spacing=0.1
    )
    
    # 1. Choropleth - Quality Score
    fig.add_trace(
        go.Choropleth(
            locations=data['State'],
            locationmode='USA-states',
            z=data['Quality_Score'],
            colorscale='RdYlGn',
            colorbar=dict(title='Quality', x=0.45, len=0.4, y=0.8)
        ),
        row=1, col=1
    )
    
    # 2. Bar chart - Top quality states
    top_quality = data.nlargest(10, 'Quality_Score')
    fig.add_trace(
        go.Bar(
            x=top_quality['State_Name'],
            y=top_quality['Quality_Score'],
            marker_color=top_quality['Quality_Score'],
            marker_colorscale='RdYlGn'
        ),
        row=1, col=2
    )
    
    # 3. Scatter - Quality vs Wait Time
    fig.add_trace(
        go.Scatter(
            x=data['Avg_Wait_Time_Days'],
            y=data['Quality_Score'],
            mode='markers+text',
            marker=dict(
                size=data['Hospitals']/5,
                color=data['Doctors_Per_1000'],
                colorscale='Blues',
                line_color='black'
            ),
            text=data['State'],
            textposition='top center'
        ),
        row=2, col=1
    )
    
    # 4. Scatter - Quality vs Cost
    fig.add_trace(
        go.Scatter(
            x=data['Cost_Per_Capita'],
            y=data['Quality_Score'],
            mode='markers+text',
            marker=dict(
                size=data['Hospitals']/5,
                color=data['Medicaid_Coverage'],
                colorscale='Greens',
                line_color='black'
            ),
            text=data['State'],
            textposition='top center'
        ),
        row=2, col=2
    )
    
    fig.update_layout(
        title_text='Healthcare System Performance by State',
        showlegend=False,
        height=900
    )
    
    fig.update_geos(
        scope='usa',
        showland=True,
        lataxis_range=[25, 50],
        lonaxis_range=[-130, -65]
    )
    
    # Print summary
    print("=" * 60)
    print("HEALTHCARE FACILITY ANALYSIS")
    print("=" * 60)
    print(f"\nTotal States: {len(data)}")
    print(f"Total Hospitals: {data['Hospitals'].sum()}")
    print(f"Average Quality Score: {data['Quality_Score'].mean():.1f}")
    print(f"Average Wait Time: {data['Avg_Wait_Time_Days'].mean():.1f} days")
    print(f"Average Doctors per 1000: {data['Doctors_Per_1000'].mean():.1f}")
    print(f"Best Quality State: {data.loc[data['Quality_Score'].idxmax(), 'State_Name']}")
    print(f"Shortest Wait Time: {data.loc[data['Avg_Wait_Time_Days'].idxmin(), 'State_Name']}")
    
    return fig, data

fig_health, df_health = healthcare_facility_analysis()
fig_health.show()
```

## V. OUTPUT_RESULTS

### Expected Outputs

1. **Global GDP Choropleth**: Interactive world map with countries colored by GDP. Darker colors indicate higher GDP values.

2. **Bubble Map**: Interactive map with circles at city locations, sized by population, colored by GDP.

3. **US State Map**: US map with states colored by economic metrics.

4. **Animated Map**: Animation showing GDP changes over time from 2015-2023.

5. **Banking Example**:
   - US state map showing bank assets by region
   - Branch locations with size indicating number of branches
   - Bar chart showing cities by total assets

6. **Healthcare Example**:
   - US state map with quality scores
   - Scatter plots showing relationships between quality, wait time, and costs

## VI. VISUALIZATION

### Flow Chart: Geographic Visualization Process

```
+----------------------+     +-----------------------+
|  COLLECT SPATIAL DATA  |---->|   GEOCODE LOCATIONS     |
+----------------------+     +-----------------------+
                                                         |
                                                         v
+----------------------+     +-----------------------+
|   CHOOSE MAP TYPE    |---->|  SELECT COLOR SCALE   |
+----------------------+     +-----------------------+
                                                         |
                                                         v
+----------------------+     +-----------------------+
|   CREATE BASEMAP    |---->|  ADD DATA LAYERS     |
+----------------------+     +-----------------------+
                                                         |
                                                         v
+----------------------+     +-----------------------+
|  ADD INTERACTIVITY   |---->|  GENERATE OUTPUT      |
+----------------------+     +-----------------------+


MAP TYPE SELECTION:
                 
+---> POINT DATA (Cities, Branches)
|     +---> Bubble/Symbol Map
|
+---> REGIONAL DATA (States, Countries)
|     +---> Choropleth Map
|
+---> DENSITY DATA
|     +---> Heat Map
|
+---> FLOW DATA (Migration, Trade)
|     +---> Flow Map
|
+---> TIME SERIES DATA
      +---> Animated Map


GEOGRAPHIC BINDING OPTIONS:
                         
+---> Country Names -> 'country names'
+---> Country Codes (ISO) -> 'ISO-3'
+---> US States -> 'USA-states'
+---> Latitude/Longitude -> Direct coordinates


COLOR SCALE.selection:
                    
+---> Sequential (Single hue) -> Continuous increasing values
|
+---> Diverging (Two hues) -> Data with meaningful center
|
+---> Categorical (Distinct) -> Discrete categories
```

## VII. ADVANCED_TOPICS

### Extensions and Variations

1. **3D Geographic Visualization**:
```python
def create_3d_geo_visualization():
    """Create 3D geographic visualizations."""
    # Create 3D scatter on geographic projection
    fig = go.Figure(data=[
        go.Scatter3d(
            x=locations['longitude'],
            y=locations['latitude'],
            z=locations['value'],
            mode='markers',
            marker=dict(
                size=locations['value'] / 100,
                color=locations['value'],
                colorscale='Viridis'
            )
        )
    ])
    
    fig.update_layout(
        scene=dict(
            xaxis_title='Longitude',
            yaxis_title='Latitude',
            zaxis_title='Value'
        )
    )
```

2. **Mapping with Multiple Layers**:
```python
def create_multi_layer_map():
    """Create maps with multiple overlay layers."""
    # Add different data layers
    fig = go.Figure()
    
    # Layer 1: Base choropleth
    fig.add_trace(go.Choropleth(
        locations=data['State'],
        locationmode='USA-states',
        z=data['Value1']
    ))
    
    # Layer 2: Markers (overlaid)
    fig.add_trace(go.Scattergeo(
        lat=locations['Lat'],
        lon=locations['Lon'],
        mode='markers'
    ))
```

3. **Custom Projections**:
```python
def use_custom_projection():
    """Use world projections beyond default options."""
    fig = px.choropleth(
        data,
        locations='Country',
        locationmode='country names',
        color='Value',
        projection='orthographic'  # or 'conic', 'azimuthal equal area'
    )
```

### Optimization Techniques

1. **Data Aggregation**:
```python
def optimize_geo_data(data):
    """Aggregate data for better visualization."""
    # Group by region instead of individual locations
    data_agg = data.groupby('Region').agg({
        'Value': 'sum',
        'Count': 'count'
    }).reset_index()
    return data_agg
```

2. **Lazy Loading**:
```python
def implement_lazy_loading():
    """Implement lazy loading for large datasets."""
    # Use Plotly's progressive rendering
    fig = px.choropleth(
        data,
        locations='Country',
        locationmode='country names',
        animation_frame='Year',
        range_color=[min_val, max_val]
    )
```

### Common Pitfalls and Solutions

1. **Geographic Binding Errors**:
   - **Problem**: Locations not matching correctly
   - **Solution**: Use consistent naming conventions; check ISO codes

2. **Missing Data Displayed as Zero**:
   - **Problem**: Missing values shown as zero, misleading viewers
   - **Solution**: Use `nan` values and set explicit missing data handling

3. **Overcrowded Maps**:
   - **Problem**: Too many points making map unreadable
   - **Solution**: Aggregate data or use clustering

4. **Inappropriate Color Scales**:
   - **Problem**: Color scale not matching data characteristics
   - **Solution**: Use sequential for continuous, diverging for with center

5. **Projection Distortion**:
   - **Problem**: Map projections distort areas misleadingly
   - **Solution**: Use equal-area projections when comparing areas

## VIII. CONCLUSION

### Key Takeaways

1. **Geographic Visualization is Essential**: Location-based insights drive decisions in finance, healthcare, retail, and public policy.

2. **Choose the Right Map Type**: Match visualization to data type - choropleth for regional, bubble for point data.

3. **Technology Matters**: Plotly provides excellent interactive visualizations; Folium adds specialized mapping capabilities.

4. **Context is Critical**: Basemaps, labels, and reference points add necessary geographic context.

5. **Interactive is Better**: Interactive maps enable deeper exploration and better understanding.

### Next Steps

1. Explore GeoPandas for advanced geographic data handling
2. Learn to create custom map projections
3. Master map-based dashboards with Dash
4. Study spatial analysis techniques
5. Practice with real-world location datasets

### Further Reading

**Books**:
- "Mapping with Data" by Meghan R. G. Ryan
- "Web Cartography" by Antti Lehto, Susanne Binachi

**Online Resources**:
- Plotly Geographic Plotting: plotly.com/python/v2/maps/
- Folium Documentation: python-visualization.github.io/folium/
- GeoPandas: geopandas.org/

**Practice**:
- Kaggle geographic datasets
- US Census Bureau data
- World Bank geographic data