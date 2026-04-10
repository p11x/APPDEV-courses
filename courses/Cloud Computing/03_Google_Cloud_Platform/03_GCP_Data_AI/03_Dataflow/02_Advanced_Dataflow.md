---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Dataflow
Purpose: Advanced understanding of GCP Dataflow features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_Dataflow.md
RelatedFiles: 01_Basic_Dataflow.md, 03_Practical_Dataflow.md
UseCase: Enterprise ETL pipelines, real-time stream processing
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Dataflow knowledge enables building scalable ETL pipelines, implementing real-time streaming analytics, and optimizing pipeline performance.

### Why Advanced Dataflow

- **Streaming Pipelines**: Real-time data processing
- **Custom Transforms**: Python/Java transforms
- **Splits and Unions**: Complex pipeline patterns
- **Flex Templates**: Reusable pipeline definitions

## 📖 WHAT

### Processing Modes

| Mode | Use Case | Characteristics |
|------|----------|------------------|
| Batch | Historical data | Bounded data, scheduled |
| Streaming | Real-time | Unbounded, continuous |

### Advanced Features

**Windowing**:
- Fixed windows (1min, 5min, 1hr)
- Sliding windows
- Session windows
- Global windows

**Triggers**:
- After watermark
- After count
- After processing time
- Composite triggers

## 🔧 HOW

### Example 1: Streaming Pipeline with Windowing

```python
import apache_beam as beam
from apache_beam.options.pipeline_options import PipelineOptions
import json

class ParseRecords(beam.DoFn):
    def process(self, element):
        yield json.loads(element)

class AddTimestamp(beam.DoFn):
    def process(self, element, timestamp=beam.DoFn.TimestampParam):
        yield beam.window.TimestampedValue(element, timestamp)

def run_streaming():
    options = PipelineOptions([
        '--runner=DataflowRunner',
        '--project=my-project',
        '--region=us-central1',
        '--streaming',
        '--machine_type=n1-standard-2',
        '--max_num_workers=10'
    ])
    
    with beam.Pipeline(options=options) as p:
        # Read from Pub/Sub
        events = (p 
            | 'ReadFromPubSub' >> beam.io.ReadFromPubSub(
                topic='projects/my-project/topics/input-topic',
                timestamp_attribute='timestamp'
            )
            | 'ParseRecords' >> beam.ParDo(ParseRecords())
            | 'AddTimestamp' >> beam.ParDo(AddTimestamp())
            | 'WindowInto' >> beam.WindowInto(
                beam.window.FixedWindows(60),
                trigger=beam.trigger.AfterWatermark(
                    early=beam.trigger.AfterProcessingTime(10),
                    late=beam.trigger.AfterCount(1)
                ),
                accumulation_mode=beam.trigger.AccumulationMode.ACCUMULATING
            )
            | 'CombinePerKey' >> beam.CombinePerKey(
                beam.combiners.Count.Globally()
            )
            | 'WriteToBigQuery' >> beam.io.WriteToBigQuery(
                'my-project:dataset.output_table',
                schema='timestamp:TIMESTAMP,count:INTEGER'
            )
        )

if __name__ == '__main__':
    run_streaming()
```

### Example 2: Custom Transform Pipeline

```python
import apache_beam as beam
import re

class ExtractEntities(beam.DoFn):
    def process(self, element):
        # Extract words and count
        words = re.findall(r'\w+', element['text'].lower())
        for word in words:
            yield (word, 1)

class ComputeWordStats(beam.PTransform):
    def expand(self, pcoll):
        return (pcoll
            | 'Extract' >> beam.ParDo(ExtractEntities())
            | 'Group' >> beam.GroupByKey()
            | 'Count' >> beam.Map(lambda x: (x[0], sum(x[1]))))

def run_pipeline():
    options = PipelineOptions([
        '--runner=DataflowRunner',
        '--project=my-project',
        '--region=us-central1',
        '--temp_location=gs://my-bucket/temp',
        '--staging_location=gs://my-bucket/staging'
    ])
    
    with beam.Pipeline(options=options) as p:
        lines = p | 'ReadText' >> beam.io.ReadFromText(
            'gs://my-bucket/input.txt'
        )
        
        (lines 
            | 'ParseJSON' >> beam.Map(lambda x: json.loads(x))
            | 'ExtractEntities' >> ComputeWordStats()
            | 'FormatOutput' >> beam.Map(lambda x: {'word': x[0], 'count': x[1]})
            | 'WriteToGCS' >> beam.io.WriteToText(
                'gs://my-bucket/output',
                file_name_suffix='.txt'
            )
        )

if __name__ == '__main__':
    run_pipeline()
```

### Example 3: Flex Template

```bash
# Create Flex Template specification
cat > metadata.json << 'EOF'
{
    "name": "FlexTemplate",
    "description": "Flexible Dataflow Template",
    "parameters": [
        {"name": "inputPath", "label": "Input Path", "isOptional": false},
        {"name": "outputPath", "label": "Output Path", "isOptional": false}
    ]
}
EOF

# Build Flex Template container
gcloud builds submit --tag gcr.io/my-project/dataflow-flex:latest .

# Build Flex Template
gcloud dataflow flex-template build \
    gs://my-bucket/templates/flex-template.json \
    --image-gcr-path=gcr.io/my-project/dataflow-flex:latest \
    --metadata-file=metadata.json \
    --sdk-language=PYTHON

# Run Flex Template
gcloud dataflow flex-template run flex-job \
    --template-file-gcs-location=gs://my-bucket/templates/flex-template.json \
    --parameters inputPath=gs://my-bucket/input \
    --parameters outputPath=gs://my-bucket/output \
    --region=us-central1
```

## ⚠️ COMMON ISSUES

### Troubleshooting Dataflow Issues

| Issue | Solution |
|-------|----------|
| Slow processing | Increase workers, optimize transforms |
| Memory issues | Use memory-efficient data structures |
| Late data | Adjust window and trigger settings |
| Failed jobs | Check logs, data format |

### Performance Optimization

- Use CombineGlobally for aggregations
- Avoid unnecessary GroupByKey
- Use CoGroupByKey for joins
- Enable autoscaling

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP Dataflow | AWS Glue | Azure Data Factory |
|---------|--------------|----------|-------------------|
| Serverless | Yes | Yes | Yes |
| Streaming | Yes | No | Yes |
| Windowing | Yes | No | Limited |
| Flex Templates | Yes | No | No |

## 🔗 CROSS-REFERENCES

### Related Topics

- Pub/Sub (streaming source)
- BigQuery (sink)
- Cloud Storage (source/sink)

### Study Resources

- Dataflow documentation
- Apache Beam programming guide

## ✅ EXAM TIPS

- Windowing = time-based aggregation
- Triggers control output timing
- Exactly-once processing
- Flex Templates for reusable pipelines
