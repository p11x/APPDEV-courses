# Topic: Geographic Data Visualization
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Geographic Data Visualization

I. INTRODUCTION
This module covers geographic data visualization techniques including choropleth maps,
bubble maps, heatmaps, and interactive geographic visualizations using matplotlib,
seaborn, and plotly libraries.

II. CORE CONCEPTS
- Geographic coordinate systems
- Choropleth maps for regional data
- Bubble and marker maps
- Heatmaps on geographic backgrounds
- Interactive geographic visualizations

III. IMPLEMENTATION
- matplotlib for basic geographic plotting
- plotly for interactive maps
- pandas for data manipulation

IV. EXAMPLES
- Banking: Regional banking performance, branch locations
- Healthcare: Disease spread, healthcare facility distribution

V. OUTPUT RESULTS
- Geographic visualizations with proper projections

VI. TESTING
- Unit tests for mapping functions
- Integration tests for multi-layer maps

VII. ADVANCED TOPICS
- Custom map projections
- Clustering for large datasets
- Real-time map updates

VIII. CONCLUSION
Best practices for geographic data visualization
"""

import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
import warnings
warnings.filterwarnings('ignore')

plt.style.use('seaborn-v0_8-whitegrid')


def generate_sample_geo_data(n=500, seed=42):
    """Generate sample geographic data."""
    np.random.seed(seed)
    
    us_states = {
        'California': (36.7783, -119.4179), 'Texas': (31.9686, -99.9018),
        'Florida': (27.6648, -81.5158), 'New York': (40.7128, -74.0060),
        'Illinois': (40.6331, -89.3985), 'Pennsylvania': (41.2033, -77.1945),
        'Ohio': (40.4173, -82.9071), 'Georgia': (32.1656, -82.9001),
        'North Carolina': (35.7596, -79.0193), 'Michigan': (44.3148, -85.6024),
        'New Jersey': (40.0583, -74.4057), 'Virginia': (37.4316, -78.6569),
        'Washington': (47.7511, -120.7401), 'Arizona': (34.0489, -111.0937),
        'Massachusetts': (42.4072, -71.3824), 'Tennessee': (35.5175, -86.5804),
        'Indiana': (40.2672, -86.1349), 'Missouri': (37.9643, -91.8318),
        'Maryland': (39.0458, -76.6413), 'Wisconsin': (43.7844, -88.7879),
        'Colorado': (39.5501, -105.7821), 'Minnesota': (46.7296, -94.6859),
        'South Carolina': (33.8361, -81.1633), 'Alabama': (32.3182, -86.9023),
        'Louisiana': (30.9843, -91.9623), 'Kentucky': (37.8393, -84.2700),
        'Oregon': (43.8041, -120.5542), 'Oklahoma': (35.0078, -97.0929),
        'Connecticut': (41.6032, -73.0877), 'Nevada': (38.8026, -116.4194),
        'Iowa': (41.8780, -93.0977), 'Arkansas': (35.2010, -91.8318),
        'Mississippi': (32.3547, -89.3985), 'Kansas': (39.0119, -98.4842),
        'Nevada': (38.8026, -116.4194), 'New Mexico': (34.5199, -105.8701),
        'Nebraska': (41.4925, -99.9018), 'West Virginia': (38.5976, -80.4549),
        'Idaho': (44.0682, -114.7420), 'Hawaii': (19.8968, -155.5828),
        'New Hampshire': (43.1939, -71.5724), 'Maine': (45.2538, -69.4455),
        'Montana': (46.8797, -110.3626), 'Rhode Island': (41.5801, -71.4774),
        'Delaware': (38.9108, -75.5277), 'South Dakota': (43.9695, -99.9018),
        'North Dakota': (47.5515, -101.0020), 'Alaska': (64.2008, -152.4937),
        'Vermont': (44.5588, -72.5778), 'Wyoming': (43.0760, -107.2903),
        'DC': (38.9072, -77.0369)
    }
    
    data = []
    for _ in range(n):
        state = np.random.choice(list(us_states.keys()))
        lat, lon = us_states[state]
        lat += np.random.normal(0, 1.5)
        lon += np.random.normal(0, 1.5)
        
        data.append({
            'State': state,
            'Latitude': lat,
            'Longitude': lon,
            'Value': np.random.uniform(1000, 100000),
            'Category': np.random.choice(['A', 'B', 'C']),
            'Year': np.random.choice([2020, 2021, 2022, 2023])
        })
    
    return pd.DataFrame(data)


def generate_us_state_data():
    """Generate US state-level aggregated data."""
    state_info = {
        'State': ['California', 'Texas', 'Florida', 'New York', 'Illinois', 'Pennsylvania',
                  'Ohio', 'Georgia', 'North Carolina', 'Michigan', 'New Jersey', 'Virginia',
                  'Washington', 'Arizona', 'Massachusetts', 'Tennessee', 'Indiana',
                  'Missouri', 'Maryland', 'Wisconsin', 'Colorado', 'Minnesota',
                  'South Carolina', 'Alabama', 'Louisiana', 'Kentucky', 'Oregon',
                  'Oklahoma', 'Connecticut', 'Nevada', 'Iowa', 'Arkansas'],
        'Population': [39538223, 29145505, 21538187, 20201249, 12812508, 13002700,
                       11799448, 10711908, 10488088, 10077331, 9288994, 8631393,
                       7705281, 7151502, 7029917, 6910840, 6785528, 6154913,
                       6177224, 5893718, 5773714, 5703961, 5110915, 5024279,
                       4657757, 4505836, 4237256, 3959359, 3605944, 3301891,
                       3190369, 3011524],
        'GDP': [3250000, 2100000, 1400000, 1700000, 900000, 800000,
                700000, 650000, 600000, 550000, 600000, 550000,
                500000, 400000, 450000, 400000, 350000, 350000,
                400000, 350000, 400000, 380000, 250000, 250000,
                280000, 250000, 250000, 200000, 300000, 180000,
                175000, 150000],
        'Median_Income': [80000, 68000, 65000, 75000, 70000, 68000,
                       61000, 65000, 60000, 64000, 85000, 80000,
                       78000, 62000, 85000, 60000, 58000, 56000,
                       86000, 64000, 77000, 73000, 56000, 52000,
                       52000, 56000, 67000, 56000, 78000, 69000,
                       62000, 52000]
    }
    
    return pd.DataFrame(state_info)


def create_choropleth_map(data, value_col, title='US Choropleth Map'):
    """Create choropleth map using Plotly."""
    fig = px.choropleth(
        data, locations='State', locationmode='USA-states',
        color=value_col, scope='usa',
        color_continuous_scale='Viridis',
        title=title
    )
    
    fig.update_layout(
        geo=dict(bgcolor='rgba(0,0,0,0)', lakecolor='rgba(0,0,0,0)'),
        title_font_size=16,
        title_x=0.5
    )
    
    return fig


def create_scatter_geo_map(data, lat_col='Latitude', lon_col='Longitude',
                         size_col='Value', color_col='Category',
                         title='Geographic Scatter Map'):
    """Create scatter map on geographic background."""
    fig = px.scatter_geo(
        data, lat=lat_col, lon=lon_col,
        size=size_col, color=color_col,
        title=title,
        color_discrete_sequence=['red', 'blue', 'green']
    )
    
    fig.update_layout(
        geo=dict(
            scope='usa',
            projection_type='albers usa',
            bgcolor='rgba(0,0,0,0)',
            lakecolor='rgba(0,0,0,0)'
        ),
        title_font_size=16,
        title_x=0.5
    )
    
    return fig


def create_bubble_map(data, lat_col='Latitude', lon_col='Longitude',
                    size_col='Value', title='Bubble Map'):
    """Create bubble map with sized markers."""
    fig = go.Figure()
    
    unique_categories = data['Category'].unique()
    colors = {'A': 'red', 'B': 'blue', 'C': 'green'}
    
    for category in unique_categories:
        subset = data[data['Category'] == category]
        
        fig.add_trace(go.Scattergeo(
            lon=subset[lon_col],
            lat=subset[lat_col],
            text=subset[size_col],
            mode='markers',
            marker=dict(
                size=subset[size_col] / 3000,
                color=colors.get(category, 'gray'),
                opacity=0.7,
                line=dict(width=1, color='white')
            ),
            name=f'Category {category}'
        ))
    
    fig.update_layout(
        title=title,
        geo=dict(
            scope='usa',
            projection_type='albers usa',
            bgcolor='rgba(0,0,0,0)',
            lakecolor='rgba(0,0,0,0)',
            showland=True,
            landcolor='rgb(243, 243, 243)',
            showlakes=True,
            lakecolor='rgb(255, 255, 255)'
        ),
        title_font_size=16,
        title_x=0.5
    )
    
    return fig


def create_animated_geo_map(data, time_col='Year', lat_col='Latitude',
                          lon_col='Longitude', size_col='Value',
                          title='Animated Geographic Map'):
    """Create animated geographic map over time."""
    fig = px.scatter_geo(
        data, lat=lat_col, lon=lon_col,
        size=size_col, color=size_col,
        animation_frame=time_col,
        title=title,
        color_continuous_scale='Viridis'
    )
    
    fig.update_layout(
        geo=dict(
            scope='usa',
            projection_type='albers usa',
            bgcolor='rgba(0,0,0,0)'
        ),
        title_font_size=16
    )
    
    fig.layout.updatemenus[0].buttons[0].args[1]['frame']['duration'] = 1000
    fig.layout.updatemenus[0].buttons[0].args[1]['transition']['duration'] = 500
    
    return fig


def create_density_heatmap(data, lat_col='Latitude', lon_col='Longitude',
                         title='Density Heatmap'):
    """Create density heatmap on geographic background."""
    fig = px.density_mapbox(
        data, lat=lat_col, lon=lon_col,
        z=data['Value'], radius=20,
        title=title,
        color_continuous_scale='Viridis'
    )
    
    fig.update_layout(
        mapbox=dict(
            style='carto-positron',
            center=dict(lat=39, lon=-98),
            zoom=3
        ),
        title_font_size=16,
        title_x=0.5
    )
    
    return fig


def create_mapbox_map(data, lat_col='Latitude', lon_col='Longitude',
                    color_col='Category', size_col='Value',
                    title='Mapbox Interactive Map'):
    """Create Mapbox-style interactive map."""
    fig = px.scatter_mapbox(
        data, lat=lat_col, lon=lon_col,
        color=color_col, size=size_col,
        title=title,
        zoom=3, center=dict(lat=39, lon=-98),
        mapbox_style='carto-positron'
    )
    
    fig.update_layout(
        title_font_size=16,
        title_x=0.5,
        margin=dict(l=0, r=0, t=40, b=0)
    )
    
    return fig


def create_layered_map(data, lat_col='Latitude', lon_col='Longitude'):
    """Create layered map with multiple data types."""
    fig = go.Figure()
    
    fig.add_trace(go.Scattergeo(
        lon=data[lon_col],
        lat=data[lat_col],
        text=data['Category'],
        mode='markers',
        marker=dict(
            size=data['Value'] / 5000,
            color=data['Category'].map({'A': 'red', 'B': 'blue', 'C': 'green'}),
            opacity=0.7
        ),
        name='Points'
    ))
    
    state_data = generate_us_state_data()
    fig.add_trace(go.Choropleth(
        locations=state_data['State'],
        z=state_data['GDP'],
        locationmode='USA-states',
        colorscale='Blues',
        colorbar_title='GDP'
    ))
    
    fig.update_layout(
        title='Layered Geographic Map',
        geo=dict(
            scope='usa',
            projection_type='albers usa',
            showlakes=True,
            lakecolor='rgb(255, 255, 255)'
        ),
        title_font_size=16,
        title_x=0.5
    )
    
    return fig


def create_state_boundaries_map():
    """Create map with state boundaries."""
    fig = go.Figure(go.Choropleth(
        locations=['CA', 'TX', 'FL', 'NY', 'IL', 'PA', 'OH', 'GA', 'NC', 'MI'],
        z=[1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
        locationmode='USA-states',
        colorscale='Viridis'
    ))
    
    fig.update_layout(
        title='State Boundaries Map',
        geo=dict(
            scope='usa',
            projection_type='albers usa',
            showlakes=True
        ),
        title_font_size=16,
        title_x=0.5
    )
    
    return fig


def core_implementation():
    """Core geographic visualization implementation."""
    data = generate_sample_geo_data(300)
    state_data = generate_us_state_data()
    
    results = {}
    
    results['choropleth'] = create_choropleth_map(state_data, 'Population', 'US Population')
    results['scatter'] = create_scatter_geo_map(data, 'Latitude', 'Longitude', 'Value', 'Category')
    results['bubble'] = create_bubble_map(data, 'Latitude', 'Longitude', 'Value')
    results['density'] = create_density_heatmap(data[:100], 'Latitude', 'Longitude')
    results['mapbox'] = create_mapbox_map(data, 'Latitude', 'Longitude', 'Category', 'Value')
    
    return results


def banking_example():
    """Banking/Finance application - Regional Banking Performance."""
    np.random.seed(123)
    
    state_data = generate_us_state_data()
    
    branch_data = pd.DataFrame({
        'State': np.repeat(state_data['State'], 10),
        'Latitude': np.random.uniform(25, 48, len(state_data) * 10),
        'Longitude': np.random.uniform(-125, -70, len(state_data) * 10),
        'Branches': np.random.poisson(5, len(state_data) * 10),
        'Total_Deposits': np.random.uniform(1000000, 50000000, len(state_data) * 10),
        'Loans_Approved': np.random.poisson(100, len(state_data) * 10),
        'Default_Rate': np.random.uniform(1, 10, len(state_data) * 10)
    })
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    choropleth = create_choropleth_map(state_data, 'Population', 'Population by State')
    choropleth.show()
    
    ax2 = fig.add_subplot(2, 3, 2)
    bubble = create_bubble_map(branch_data, 'Latitude', 'Longitude', 'Total_Deposits')
    bubble.show()
    
    ax3 = fig.add_subplot(2, 3, 3)
    scatter_fig = create_scatter_geo_map(
        branch_data, 'Latitude', 'Longitude',
        'Total_Deposits', 'Default_Rate',
        'Branch Performance'
    )
    scatter_fig.show()
    
    ax4 = fig.add_subplot(2, 3, 4)
    density = create_density_heatmap(branch_data, 'Latitude', 'Longitude')
    density.show()
    
    ax5 = fig.add_subplot(2, 3, 5)
    state_summary = state_data.nlargest(10, 'GDP')
    ax5.barh(state_summary['State'], state_summary['GDP'], color='steelblue')
    ax5.set_xlabel('GDP (Millions $)')
    ax5.set_title('Top 10 States by GDP')
    ax5.invert_yaxis()
    
    ax6 = fig.add_subplot(2, 3, 6)
    ax6.scatter(state_data['Population'] / 1000000,
               state_data['Median_Income'],
               c=state_data['GDP'] / 1000,
               s=100, cmap='viridis', alpha=0.7)
    ax6.set_xlabel('Population (Millions)')
    ax6.set_ylabel('Median Income ($)')
    ax6.set_title('Population vs Income')
    
    plt.tight_layout()
    
    print("=" * 60)
    print("REGIONAL BANKING PERFORMANCE ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Branches: {len(branch_data)}")
    print(f"Total Deposits: ${branch_data['Total_Deposits'].sum():,.0f}")
    print(f"Average Default Rate: {branch_data['Default_Rate'].mean():.2f}%")
    print(f"\nTop 5 States by Population:")
    print(state_data.nlargest(5, 'Population')[['State', 'Population']])
    
    return fig, branch_data


def healthcare_example():
    """Healthcare application - Healthcare Facility Distribution."""
    np.random.seed(456)
    
    states = ['California', 'Texas', 'Florida', 'New York', 'Illinois',
              'Pennsylvania', 'Ohio', 'Georgia', 'North Carolina', 'Michigan']
    
    healthcare_data = pd.DataFrame({
        'State': states,
        'Latitude': [36.78, 31.97, 27.66, 40.71, 40.63, 41.20, 40.42, 32.17, 35.76, 44.31],
        'Longitude': [-119.42, -99.90, -81.52, -74.01, -89.40, -77.19, -82.91, -82.90, -79.02, -85.60],
        'Hospitals': np.random.randint(50, 500, len(states)),
        'Beds': np.random.randint(5000, 50000, len(states)),
        'Doctors': np.random.randint(5000, 50000, len(states)),
        'COVID_Cases': np.random.randint(10000, 500000, len(states)),
        'Vaccination_Rate': np.random.uniform(40, 80, len(states)),
        'Avg_Wait_Time': np.random.uniform(10, 60, len(states))
    })
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    
    fig1 = px.choropleth(
        healthcare_data, locations='State', locationmode='USA-states',
        color='Hospitals', scope='usa',
        title='Hospitals by State'
    )
    fig1.show()
    
    fig2 = go.Figure()
    fig2.add_trace(go.Scattergeo(
        lon=healthcare_data['Longitude'],
        lat=healthcare_data['Latitude'],
        text=healthcare_data['Hospitals'],
        marker=dict(
            size=healthcare_data['Hospitals'] / 10,
            color='red',
            opacity=0.7,
            line=dict(width=1, color='white')
        ),
        mode='markers'
    ))
    
    fig2.update_layout(
        title='Hospital Distribution',
        geo=dict(scope='usa', projection_type='albers usa')
    )
    fig2.show()
    
    ax3 = fig.add_subplot(2, 3, 3)
    ax3.barh(healthcare_data['State'], healthcare_data['COVID_Cases'],
             color='red', alpha=0.7)
    ax3.set_xlabel('COVID Cases')
    ax3.set_title('COVID Cases by State')
    ax3.invert_yaxis()
    
    ax4 = fig.add_subplot(2, 3, 4)
    ax4.scatter(healthcare_data['Hospitals'], healthcare_data['Doctors'],
               c=healthcare_data['Vaccination_Rate'], s=100, cmap='viridis')
    ax4.set_xlabel('Number of Hospitals')
    ax4.set_ylabel('Number of Doctors')
    ax4.set_title('Hospitals vs Doctors')
    
    ax5 = fig.add_subplot(2, 3, 5)
    ax5.hist(healthcare_data['Avg_Wait_Time'], bins=10, edgecolor='white',
            alpha=0.7, color='steelblue')
    ax5.set_xlabel('Average Wait Time (minutes)')
    ax5.set_ylabel('Count')
    ax5.set_title('Distribution of Wait Times')
    
    ax6 = fig.add_subplot(2, 3, 6)
    ax6.scatter(healthcare_data['Vaccination_Rate'],
                healthcare_data['COVID_Cases'] / 1000,
                s=100, c='green', alpha=0.7)
    ax6.set_xlabel('Vaccination Rate (%)')
    ax6.set_ylabel('COVID Cases (thousands)')
    ax6.set_title('Vaccination vs Cases')
    
    plt.tight_layout()
    
    print("=" * 60)
    print("HEALTHCARE FACILITY ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Hospitals: {healthcare_data['Hospitals'].sum()}")
    print(f"Total Beds: {healthcare_data['Beds'].sum()}")
    print(f"Total Doctors: {healthcare_data['Doctors'].sum()}")
    print(f"\nAverage Vaccination Rate: {healthcare_data['Vaccination_Rate'].mean():.1f}%")
    print(f"Average Wait Time: {healthcare_data['Avg_Wait_Time'].mean():.1f} minutes")
    
    return fig, healthcare_data


def create_multi_region_map(data_list, titles):
    """Create map with multiple regions overlaid."""
    fig = go.Figure()
    
    colors = ['red', 'blue', 'green', 'orange']
    
    for i, (data, title) in enumerate(zip(data_list, titles)):
        fig.add_trace(go.Scattergeo(
            lon=data['Longitude'],
            lat=data['Latitude'],
            text=data.get('Value', ''),
            mode='markers',
            marker=dict(
                size=data['Value'] / 5000 if 'Value' in data.columns else 10,
                color=colors[i],
                opacity=0.7
            ),
            name=title
        ))
    
    fig.update_layout(
        title='Multi-Region Map',
        geo=dict(
            scope='usa',
            projection_type='albers usa',
            showlakes=True
        )
    )
    
    return fig


def main():
    """Main execution function."""
    print("Executing Geographic Data Visualization implementation")
    print("=" * 60)
    
    data = generate_sample_geo_data(300)
    print(f"Generated geographic data: {data.shape[0]} locations")
    
    state_data = generate_us_state_data()
    print(f"Generated state data: {len(state_data)} states")
    
    create_choropleth_map(state_data, 'Population', 'US Population Map')
    create_scatter_geo_map(data, 'Latitude', 'Longitude', 'Value', 'Category')
    create_bubble_map(data, 'Latitude', 'Longitude', 'Value')
    create_density_heatmap(data[:100], 'Latitude', 'Longitude')
    create_mapbox_map(data, 'Latitude', 'Longitude', 'Category', 'Value')
    
    plt.show()
    
    return data


if __name__ == "__main__":
    main()