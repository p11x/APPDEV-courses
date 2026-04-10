---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Dataflow
Purpose: Hands-on exercises for Dataflow pipeline development
Difficulty: advanced
Prerequisites: 01_Basic_Dataflow.md, 02_Advanced_Dataflow.md
RelatedFiles: 01_Basic_Dataflow.md, 02_Advanced_Dataflow.md
UseCase: Production ETL pipelines, stream processing, data transformation
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Dataflow is essential for building production data pipelines, processing real-time streams, and implementing ETL workflows.

### Lab Goals

- Build batch pipelines
- Create streaming pipelines
- Use Flex Templates

## 📖 WHAT

### Exercise Overview

1. **Batch Pipeline**: ETL from Cloud Storage to BigQuery
2. **Streaming Pipeline**: Real-time Pub/Sub to BigQuery
3. **Flex Template**: Reusable pipeline

## 🔧 HOW

### Exercise 1: Build Batch ETL Pipeline

```python
import apache_beam as beam
import json

class ParseCSV(beam.DoFn):
    def process(self, element):
        fields = element.split(',')
        yield {
            'id': fields[0],
            'name': fields[1],
            'value': int(fields[2])
        }

class TransformData(beam.DoFn):
    def process(self, element):
        element['value_doubled'] = element['value'] * 2
        element['processed'] = True
        yield element

def run_batch_pipeline():
    import apache_beam as beam
    from apache_beam.options.pipeline_options import PipelineOptions
    
    options = PipelineOptions([
        '--runner=DataflowRunner',
        '--project=my-project',
        '--region=us-central1',
        '--temp_location=gs://my-bucket/temp',
        '--staging_location=gs://my-bucket/staging',
        '--machine_type=n1-standard-2',
        '--max_num_workers=5'
    ])
    
    with beam.Pipeline(options=options) as p:
        (p 
            | 'ReadCSV' >> beam.io.ReadFromText(
                'gs://my-bucket/input/data.csv',
                skip_header_lines=1
            )
            | 'ParseCSV' >> beam.ParDo(ParseCSV())
            | 'Transform' >> beam.ParDo(TransformData())
            | 'WriteToBigQuery' >> beam.io.WriteToBigQuery(
                'my-project:dataset.output_table',
                schema='id:STRING,name:STRING,value:INTEGER,value_doubled:INTEGER,processed:BOOLEAN',
                create_disposition=beam.io.BigQueryDisposition.CREATE_IF_NEEDED,
                write_disposition=beam.io.BigQueryDisposition.WRITE_TRUNCATE
            )
        )

if __name__ == '__main__':
    run_batch_pipeline()
```

### Exercise 2: Build Streaming Pipeline

```python
import apache_beam as beam
from apache_beam.options.pipeline_options import PipelineOptions
import json

class ParseJSON(beam.DoFn):
    def process(self, element):
        yield json.loads(element.decode('utf-8'))

class ComputeAggregates(beam.DoFn):
    def process(self, element, window=beam.DoFn.WindowParam):
        key, values = element
        window_start = window.start.to_utc_datetime().isoformat()
        total = sum(v['value'] for v in values)
        yield {
            'key': key,
            'count': len(values),
            'total': total,
            'window_start': window_start
        }

def run_streaming_pipeline():
    options = PipelineOptions([
        '--runner=DataflowRunner',
        '--project=my-project',
        '--region=us-central1',
        '--streaming',
        '--temp_location=gs://my-bucket/temp',
        '--staging_location=gs://my-bucket/staging'
    ])
    
    with beam.Pipeline(options=options) as p:
        events = (p 
            | 'ReadFromPubSub' >> beam.io.ReadFromPubSub(
                topic='projects/my-project/topics/events',
                timestamp_attribute='timestamp'
            )
            | 'ParseJSON' >> beam.ParDo(ParseJSON())
            | 'AddTimestamp' >> beam.Map(
                lambda x: beam.window.TimestampedValue(x, x.get('timestamp', 0))
            )
            | 'WindowInto' >> beam.WindowInto(
                beam.window.FixedWindows(60)
            )
            | 'KeyBy' >> beam.Map(lambda x: (x['category'], x))
            | 'GroupByKey' >> beam.GroupByKey()
            | 'ComputeAggregates' >> beam.ParDo(ComputeAggregates())
            | 'WriteToBigQuery' >> beam.io.WriteToBigQuery(
                'my-project:dataset.aggregates',
                schema='key:STRING,count:INTEGER,total:INTEGER,window_start:TIMESTAMP'
            )
        )

if __name__ == '__main__':
    run_streaming_pipeline()
```

### Exercise 3: Create Flex Template

```bash
#!/bin/bash
# Create and run Flex Template

PROJECT_ID="my-project-id"

# Create metadata file
cat > metadata.json << 'EOF'
{
    "name": "ETL Pipeline",
    "description": "Flex template for ETL",
    "parameters": [
        {"name": "inputPath", "label": "Input GCS Path"},
        {"name": "outputTable", "label": "Output BigQuery Table"}
    ]
}
EOF

# Create Python pipeline file
cat > pipeline.py << 'PYEOF'
import apache_beam as beam
import sys

def run(argv=None):
    import apache_beam as beam
    from apache_beam.options.pipeline_options import PipelineOptions
    
    class CustomOptions(PipelineOptions):
        @beam.options.validator.required_choice_property
        def inputPath(self):
            return None
            
        @beam.options.validator.required_choice_property
        def outputTable(self):
            return None
    
    options = PipelineOptions(sys.argv)
    custom_options = options.view_as(CustomOptions)
    
    with beam.Pipeline(options=options) as p:
        (p 
            | 'Read' >> beam.io.ReadFromText(custom_options.inputPath)
            | 'Write' >> beam.io.WriteToBigQuery(custom_options.outputTable)
        )

if __name__ == '__main__':
    run()
PYEOF

# Create Dockerfile
cat > Dockerfile << 'EOF'
FROM apache/beam_python3:2.50.0
WORKDIR /app
COPY pipeline.py .
RUN pip install apache-beam[gcp]
ENTRYPOINT ["python", "pipeline.py"]
EOF

# Build container image
gcloud builds submit --tag gcr.io/$PROJECT_ID/etl-flex:latest .

# Build Flex Template
gcloud dataflow flex-template build \
    gs://my-bucket/templates/etl-flex.json \
    --image-gcr-path=gcr.io/$PROJECT_ID/etl-flex:latest \
    --metadata-file=metadata.json \
    --sdk-language=PYTHON

# Run Flex Template
gcloud dataflow flex-template run etl-job \
    --template-file-gcs-location=gs://my-bucket/templates/etl-flex.json \
    --parameters inputPath=gs://my-bucket/input/*.csv \
    --parameters outputTable=my-project:dataset.output \
    --region=us-central1

echo "Flex template created and run!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Slow processing | Check worker count |
| Data errors | Validate input format |
| Memory issues | Optimize transforms |

### Validation

```bash
# Check job status
gcloud dataflow jobs list --region=us-central1

# View job logs
gcloud dataflow jobs show JOB_ID --region=us-central1
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Storage: Source/sink
- Pub/Sub: Streaming source
- BigQuery: Data sink

## 🔗 CROSS-REFERENCES

### Related Labs

- Pub/Sub
- BigQuery
- Cloud Storage

### Next Steps

- Implement monitoring
- Set up alerts
- Build CI/CD pipeline

## ✅ EXAM TIPS

- Practice Apache Beam transforms
- Know windowing types
- Understand streaming vs batch
- Use Flex Templates for reusability
