# Topic: Production Deployment Considerations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Production Deployment Considerations

I. INTRODUCTION
    Production deployment of machine learning models involves converting trained
    models into production-ready systems that can serve predictions at scale.
    This includes model serialization, API development, containerization,
    monitoring, versioning, and maintaining model performance over time.

II. CORE_CONCEPTS
    - Model serialization (pickle, joblib, ONNX)
    - Model serving architectures
    - REST API development
    - Containerization (Docker)
    - Model versioning
    - Monitoring and logging
    - A/B testing
    - Feature engineering pipelines

III. IMPLEMENTATION
    - Model saving and loading
    - Building prediction pipelines
    - API endpoints for prediction
    - Docker container setup
    - Model monitoring utilities
    - Version management

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Credit Risk Model Deployment
    - Healthcare: Patient Triage Model Deployment

V. OUTPUT_RESULTS
    - Saved models and pipelines
    - API documentation
    - Performance metrics
    - Monitoring dashboards

VI. TESTING
    - Unit tests for predictions
    - Integration tests
    - Load testing
    - Model validation

VII. ADVANCED_TOPICS
    - Model compression
    - Edge deployment
    - Real-time streaming
    - Feature stores

VIII. CONCLUSION
    - Deployment best practices
    - Common pitfalls to avoid
    - Scaling considerations
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_classification, make_regression
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier, RandomForestRegressor
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import Pipeline
from sklearn.metrics import accuracy_score, mean_squared_error, r2_score
import pickle
import joblib
import json
import os
from datetime import datetime
import warnings
warnings.filterwarnings('ignore')


def generate_sample_data(n_samples=1000, classification=True, random_state=42):
    """
    Generate sample data for deployment examples.
    """
    if classification:
        X, y = make_classification(
            n_samples=n_samples,
            n_features=10,
            n_informative=5,
            n_classes=2,
            random_state=random_state
        )
    else:
        X, y = make_regression(
            n_samples=n_samples,
            n_features=10,
            n_informative=5,
            noise=10,
            random_state=random_state
        )
    
    feature_names = [f'feature_{i}' for i in range(X.shape[1])]
    return X, y, feature_names


def train_production_model(X_train, y_train, model_type='classifier'):
    """
    Train a model ready for production deployment.
    
    Parameters:
    -----------
    X_train : ndarray
        Training features
    y_train : ndarray
        Training labels
    model_type : str
        'classifier' or 'regressor'
    
    Returns:
    --------
    pipeline : Pipeline
        Complete pipeline with scaler and model
    """
    print(f"\n{'='*60}")
    print(f"TRAINING PRODUCTION MODEL")
    print(f"{'='*60}")
    
    if model_type == 'classifier':
        model = RandomForestClassifier(
            n_estimators=100,
            max_depth=10,
            min_samples_split=5,
            random_state=42,
            n_jobs=-1
        )
    else:
        model = RandomForestRegressor(
            n_estimators=100,
            max_depth=10,
            min_samples_split=5,
            random_state=42,
            n_jobs=-1
        )
    
    pipeline = Pipeline([
        ('scaler', StandardScaler()),
        ('model', model)
    ])
    
    pipeline.fit(X_train, y_train)
    
    train_score = pipeline.score(X_train, y_train)
    print(f"Training score: {train_score:.4f}")
    
    return pipeline


def save_model(pipeline, filepath, format='joblib'):
    """
    Save model for production deployment.
    
    Parameters:
    -----------
    pipeline : Pipeline
        Trained pipeline
    filepath : str
        Path to save the model
    format : str
        'joblib', 'pickle', or 'onnx'
    """
    print(f"\n{'='*60}")
    print(f"SAVING MODEL")
    print(f"{'='*60}")
    print(f"Format: {format}")
    print(f"Path: {filepath}")
    
    os.makedirs(os.path.dirname(filepath), exist_ok=True)
    
    if format == 'joblib':
        joblib.dump(pipeline, filepath)
    elif format == 'pickle':
        with open(filepath, 'wb') as f:
            pickle.dump(pipeline, f)
    
    file_size = os.path.getsize(filepath)
    print(f"Model saved successfully!")
    print(f"File size: {file_size / 1024:.2f} KB")


def load_model(filepath, format='joblib'):
    """
    Load model from file.
    
    Parameters:
    -----------
    filepath : str
        Path to the saved model
    format : str
        'joblib' or 'pickle'
    
    Returns:
    --------
    pipeline : Pipeline
        Loaded pipeline
    """
    print(f"\nLoading model from: {filepath}")
    
    if format == 'joblib':
        pipeline = joblib.load(filepath)
    elif format == 'pickle':
        with open(filepath, 'rb') as f:
            pipeline = pickle.load(f)
    
    print(f"Model loaded successfully!")
    
    return pipeline


def create_prediction_api(pipeline, feature_names):
    """
    Create a prediction API function.
    
    Parameters:
    -----------
    pipeline : Pipeline
        Trained pipeline
    feature_names : list
        Names of features
    
    Returns:
    --------
    predict_api : function
        API function for predictions
    """
    print(f"\n{'='*60}")
    print(f"CREATING PREDICTION API")
    print(f"{'='*60}")
    
    def predict(input_data):
        """
        Make predictions on input data.
        
        Parameters:
        -----------
        input_data : DataFrame or ndarray
            Input features
        
        Returns:
        --------
        predictions : dict
            Prediction results with metadata
        """
        if isinstance(input_data, pd.DataFrame):
            X = input_data[feature_names].values
        else:
            X = np.array(input_data)
        
        if len(X.shape) == 1:
            X = X.reshape(1, -1)
        
        start_time = datetime.now()
        predictions = pipeline.predict(X)
        probabilities = None
        
        if hasattr(pipeline.named_steps['model'], 'predict_proba'):
            probabilities = pipeline.predict_proba(X)
        
        end_time = datetime.now()
        latency_ms = (end_time - start_time).total_seconds() * 1000
        
        result = {
            'predictions': predictions.tolist(),
            'prediction_proba': probabilities.tolist() if probabilities is not None else None,
            'latency_ms': round(latency_ms, 2),
            'timestamp': end_time.isoformat(),
            'model_version': '1.0.0',
            'input_shape': list(X.shape)
        }
        
        return result
    
    return predict


def create_batch_prediction_api(pipeline, feature_names):
    """
    Create batch prediction API.
    
    Parameters:
    -----------
    pipeline : Pipeline
        Trained pipeline
    feature_names : list
        Names of features
    
    Returns:
    --------
    batch_predict : function
        Batch prediction function
    """
    print(f"\n{'='*60}")
    print(f"CREATING BATCH PREDICTION API")
    print(f"{'='*60}")
    
    def batch_predict(input_data, batch_size=100):
        """
        Make predictions on batch data.
        
        Parameters:
        -----------
        input_data : DataFrame or ndarray
            Input features
        batch_size : int
            Size of each batch
        
        Returns:
        --------
        results : dict
            Batch prediction results
        """
        if isinstance(input_data, pd.DataFrame):
            X = input_data[feature_names].values
        else:
            X = np.array(input_data)
        
        n_samples = X.shape[0]
        predictions = []
        probabilities = []
        
        start_time = datetime.now()
        
        for i in range(0, n_samples, batch_size):
            batch = X[i:i+batch_size]
            batch_preds = pipeline.predict(batch)
            predictions.extend(batch_preds.tolist())
            
            if hasattr(pipeline.named_steps['model'], 'predict_proba'):
                batch_probs = pipeline.predict_proba(batch)
                probabilities.extend(batch_probs.tolist())
        
        end_time = datetime.now()
        total_time_ms = (end_time - start_time).total_seconds() * 1000
        avg_latency_ms = total_time_ms / n_samples
        
        results = {
            'predictions': predictions,
            'prediction_proba': probabilities if probabilities else None,
            'total_samples': n_samples,
            'total_time_ms': round(total_time_ms, 2),
            'avg_latency_ms': round(avg_latency_ms, 4),
            'throughput_samples_per_sec': round(1000 / avg_latency_ms, 2),
            'timestamp': end_time.isoformat(),
            'model_version': '1.0.0'
        }
        
        print(f"Batch prediction complete:")
        print(f"  Samples: {n_samples}")
        print(f"  Total time: {total_time_ms:.2f}ms")
        print(f"  Avg latency: {avg_latency_ms:.4f}ms")
        print(f"  Throughput: {results['throughput_samples_per_sec']:.2f} samples/sec")
        
        return results
    
    return batch_predict


def create_model_metadata(pipeline, feature_names, metrics, model_type='classifier'):
    """
    Create model metadata for deployment.
    
    Parameters:
    -----------
    pipeline : Pipeline
        Trained pipeline
    feature_names : list
        Names of features
    metrics : dict
        Model metrics
    model_type : str
        Type of model
    
    Returns:
    --------
    metadata : dict
        Model metadata
    """
    metadata = {
        'model_name': 'Production_Model',
        'model_version': '1.0.0',
        'model_type': model_type,
        'created_date': datetime.now().isoformat(),
        'feature_names': feature_names,
        'n_features': len(feature_names),
        'metrics': metrics,
        'pipeline_steps': [step[0] for step in pipeline.steps],
        'model_params': pipeline.named_steps['model'].get_params(),
        'scaler_mean': pipeline.named_steps['scaler'].mean_.tolist(),
        'scaler_std': pipeline.named_steps['scaler'].scale_.tolist()
    }
    
    print(f"\n{'='*60}")
    print(f"MODEL METADATA")
    print(f"{'='*60}")
    print(f"Model: {metadata['model_name']}")
    print(f"Version: {metadata['model_version']}")
    print(f"Type: {metadata['model_type']}")
    print(f"Features: {metadata['n_features']}")
    print(f"Created: {metadata['created_date']}")
    
    return metadata


def save_metadata(metadata, filepath):
    """
    Save model metadata to JSON.
    
    Parameters:
    -----------
    metadata : dict
        Model metadata
    filepath : str
        Path to save metadata
    """
    os.makedirs(os.path.dirname(filepath), exist_ok=True)
    
    with open(filepath, 'w') as f:
        json.dump(metadata, f, indent=2)
    
    print(f"Metadata saved to: {filepath}")


def create_prediction_explainer(pipeline, feature_names):
    """
    Create prediction explainer using feature importance.
    
    Parameters:
    -----------
    pipeline : Pipeline
        Trained pipeline
    feature_names : list
        Names of features
    
    Returns:
    --------
    explain_prediction : function
        Function to explain predictions
    """
    feature_importance = pipeline.named_steps['model'].feature_importances_
    
    print(f"\n{'='*60}")
    print(f"FEATURE IMPORTANCE")
    print(f"{'='*60}")
    
    importance_df = pd.DataFrame({
        'feature': feature_names,
        'importance': feature_importance
    }).sort_values('importance', ascending=False)
    
    for _, row in importance_df.iterrows():
        print(f"  {row['feature']}: {row['importance']:.4f}")
    
    def explain_prediction(input_data):
        """
        Explain a single prediction.
        
        Parameters:
        -----------
        input_data : ndarray
            Input features
        
        Returns:
        --------
        explanation : dict
            Prediction explanation
        """
        if len(input_data.shape) == 1:
            input_data = input_data.reshape(1, -1)
        
        prediction = pipeline.predict(input_data)[0]
        
        feature_values = input_data[0]
        
        contributions = feature_values * feature_importance
        
        feature_contributions = sorted(
            [
                {'feature': fn, 'value': fv, 'contribution': cont}
                for fn, fv, cont in zip(feature_names, feature_values, contributions)
            ],
            key=lambda x: abs(x['contribution']),
            reverse=True
        )
        
        return {
            'prediction': int(prediction),
            'feature_contributions': feature_contributions,
            'top_contributing_features': feature_contributions[:5]
        }
    
    return explain_prediction


def monitor_model_performance(y_true, y_pred, predictions_history, window_size=100):
    """
    Monitor model performance in production.
    
    Parameters:
    -----------
    y_true : ndarray
        True labels
    y_pred : ndarray
        Predicted labels
    predictions_history : list
        History of predictions
    window_size : int
        Size of monitoring window
    
    Returns:
    --------
    monitoring_data : dict
        Monitoring metrics
    """
    print(f"\n{'='*60}")
    print(f"MODEL MONITORING")
    print(f"{'='*60}")
    
    accuracy = accuracy_score(y_true, y_pred)
    
    n_recent = min(window_size, len(predictions_history))
    recent_predictions = predictions_history[-n_recent:]
    
    accuracy_trend = []
    for i in range(10, len(recent_predictions)):
        window = recent_predictions[i-10:i]
        accuracy_trend.append(sum(window) / len(window))
    
    monitoring_data = {
        'accuracy': accuracy,
        'n_predictions': len(y_pred),
        'window_size': window_size,
        'recent_accuracy': accuracy_trend[-1] if accuracy_trend else None,
        'accuracy_trend': accuracy_trend,
        'timestamp': datetime.now().isoformat()
    }
    
    print(f"Overall accuracy: {accuracy:.4f}")
    print(f"Recent accuracy (last {n_recent}): {monitoring_data['recent_accuracy']:.4f}")
    
    visualize_monitoring(monitoring_data)
    
    return monitoring_data


def visualize_monitoring(monitoring_data):
    """
    Visualize model monitoring data.
    
    Parameters:
    -----------
    monitoring_data : dict
        Monitoring metrics
    """
    plt.figure(figsize=(10, 4))
    
    plt.plot(monitoring_data['accuracy_trend'])
    plt.axhline(y=monitoring_data['accuracy'], color='r', linestyle='--', label='Overall')
    plt.xlabel('Window')
    plt.ylabel('Accuracy')
    plt.title('Model Accuracy Trend')
    plt.legend()
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.show()


def create_dockerfile():
    """
    Create Dockerfile for model deployment.
    """
    dockerfile_content = '''
FROM python:3.9-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY model.joblib .
COPY metadata.json .

COPY app.py .

EXPOSE 8000

CMD ["python", "app.py"]
'''
    
    print(f"\n{'='*60}")
    print(f"DOCKERFILE CONTENT")
    print(f"{'='*60}")
    print(dockerfile_content)
    
    return dockerfile_content


def create_requirements_file():
    """
    Create requirements.txt for deployment.
    """
    requirements = '''scikit-learn>=1.0.0
pandas>=1.3.0
numpy>=1.20.0
joblib>=1.1.0
flask>=2.0.0
gunicorn>=20.1.0
'''
    
    print(f"\n{'='*60}")
    print(f"REQUIREMENTS.TXT CONTENT")
    print(f"{'='*60}")
    print(requirements)
    
    return requirements


def banking_example():
    """
    Banking/Finance example: Credit Risk Model Deployment.
    """
    print(f"\n{'='*60}")
    print(f"BANKING EXAMPLE: Credit Risk Model Deployment")
    print(f"{'='*60}")
    
    np.random.seed(42)
    n_samples = 1000
    
    income = np.random.uniform(25000, 200000, n_samples)
    credit_score = np.random.uniform(500, 850, n_samples)
    debt = np.random.uniform(0, 50000, n_samples)
    employment_years = np.random.uniform(0, 30, n_samples)
    loan_amount = np.random.uniform(1000, 50000, n_samples)
    existing_loans = np.random.randint(0, 5, n_samples)
    
    risk = np.zeros(n_samples)
    for i in range(n_samples):
        score = 0
        if credit_score[i] > 700:
            score += 2
        if income[i] > 50000:
            score += 1
        if debt[i] / income[i] < 0.3:
            score += 2
        if employment_years[i] > 2:
            score += 1
        if existing_loans[i] < 2:
            score += 1
        
        risk[i] = 1 if score >= 5 else 0
    
    feature_names = ['income', 'credit_score', 'debt', 'employment_years', 'loan_amount', 'existing_loans']
    X = np.column_stack([income, credit_score, debt, employment_years, loan_amount, existing_loans])
    y = risk
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    print(f"\nDataset: {X.shape[0]} samples, {X.shape[1]} features")
    
    pipeline = train_production_model(X_train, y_train, model_type='classifier')
    
    y_pred = pipeline.predict(X_test)
    accuracy = accuracy_score(y_test, y_pred)
    
    metrics = {
        'accuracy': accuracy,
        'n_samples_train': len(y_train),
        'n_samples_test': len(y_test)
    }
    
    print(f"\nTest Accuracy: {accuracy:.4f}")
    
    metadata = create_model_metadata(pipeline, feature_names, metrics, model_type='classifier')
    
    save_metadata(metadata, 'models/credit_risk_metadata.json')
    
    explain_prediction = create_prediction_explainer(pipeline, feature_names)
    
    sample_input = X_test[0]
    explanation = explain_prediction(sample_input)
    print(f"\nPrediction explanation for sample:")
    print(f"  Prediction: {explanation['prediction']}")
    print(f"  Top features: {explanation['top_contributing_features']}")
    
    return pipeline, metrics


def healthcare_example():
    """
    Healthcare example: Patient Triage Model Deployment.
    """
    print(f"\n{'='*60}")
    print(f"HEALTHCARE EXAMPLE: Patient Triage Model Deployment")
    print(f"{'='*60}")
    
    np.random.seed(123)
    n_samples = 1000
    
    age = np.random.uniform(18, 90, n_samples)
    bmi = np.random.uniform(18, 45, n_samples)
    heart_rate = np.random.uniform(50, 120, n_samples)
    blood_pressure = np.random.uniform(90, 200, n_samples)
    temperature = np.random.uniform(36.0, 39.0, n_samples)
    oxygen_saturation = np.random.uniform(85, 100, n_samples)
    respiratory_rate = np.random.uniform(12, 30, n_samples)
    
    triage = np.zeros(n_samples)
    for i in range(n_samples):
        score = 0
        if blood_pressure[i] > 160:
            score += 3
        elif blood_pressure[i] > 140:
            score += 2
        if oxygen_saturation[i] < 90:
            score += 3
        elif oxygen_saturation[i] < 95:
            score += 1
        if heart_rate[i] > 100:
            score += 2
        if age[i] > 65:
            score += 1
        if temperature[i] > 38.5:
            score += 1
        
        if score >= 6:
            triage[i] = 2
        elif score >= 3:
            triage[i] = 1
        else:
            triage[i] = 0
    
    feature_names = ['age', 'bmi', 'heart_rate', 'blood_pressure', 'temperature', 'oxygen_saturation', 'respiratory_rate']
    X = np.column_stack([age, bmi, heart_rate, blood_pressure, temperature, oxygen_saturation, respiratory_rate])
    y = triage
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    print(f"\nDataset: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"Triage distribution: {np.bincount(y.astype(int))}")
    
    pipeline = train_production_model(X_train, y_train, model_type='classifier')
    
    y_pred = pipeline.predict(X_test)
    accuracy = accuracy_score(y_test, y_pred)
    
    metrics = {
        'accuracy': accuracy,
        'n_samples_train': len(y_train),
        'n_samples_test': len(y_test)
    }
    
    print(f"\nTest Accuracy: {accuracy:.4f}")
    
    metadata = create_model_metadata(pipeline, feature_names, metrics, model_type='classifier')
    
    save_metadata(metadata, 'models/triage_metadata.json')
    
    return pipeline, metrics


def test_deployment_components():
    """
    Test deployment components.
    """
    print(f"\n{'='*60}")
    print(f"TESTING DEPLOYMENT COMPONENTS")
    print(f"{'='*60}")
    
    X, y, feature_names = generate_sample_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    pipeline = train_production_model(X_train, y_train)
    
    save_model(pipeline, 'models/test_model.joblib')
    
    loaded_pipeline = load_model('models/test_model.joblib')
    
    predictions = loaded_pipeline.predict(X_test)
    accuracy = accuracy_score(y_test, predictions)
    print(f"Loaded model accuracy: {accuracy:.4f}")
    
    predict_api = create_prediction_api(pipeline, feature_names)
    result = predict_api(X_test[:5])
    print(f"Single prediction: {result}")
    
    batch_api = create_batch_prediction_api(pipeline, feature_names)
    batch_result = batch_predict(X_test, batch_size=50)
    
    print(f"\n{'='*60}")
    print(f"ALL TESTS COMPLETED SUCCESSFULLY")
    print(f"{'='*60}")
    
    return True


def main():
    """
    Main function to execute production deployment examples.
    """
    print("="*60)
    print("PRODUCTION DEPLOYMENT CONSIDERATIONS IMPLEMENTATION")
    print("="*60)
    
    print("\nI. INTRODUCTION")
    print("   Production deployment involves converting trained models")
    print("   into serving systems that can make predictions at scale.")
    
    print("\nII. CORE_CONCEPTS")
    print("   - Model serialization (joblib, pickle)")
    print("   - Prediction pipelines")
    print("   - API development")
    print("   - Model monitoring")
    print("   - Version management")
    
    print("\nIII. IMPLEMENTATION")
    
    X, y, feature_names = generate_sample_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    pipeline = train_production_model(X_train, y_train)
    save_model(pipeline, 'models/production_model.joblib')
    loaded_pipeline = load_model('models/production_model.joblib')
    
    predict_api = create_prediction_api(pipeline, feature_names)
    sample_pred = predict_api(X_test[:3])
    print(f"Sample prediction: {sample_pred}")
    
    print("\nIV. EXAMPLES")
    banking_pipeline, banking_metrics = banking_example()
    healthcare_pipeline, healthcare_metrics = healthcare_example()
    
    print("\nV. OUTPUT_RESULTS")
    print("   - Saved models and pipelines")
    print("   - API for predictions")
    print("   - Model metadata")
    print("   - Feature importance")
    
    print("\nVI. TESTING")
    test_deployment_components()
    
    print("\nVII. ADVANCED_TOPICS")
    print("   - ONNX for cross-platform deployment")
    print("   - Feature stores for consistent feature engineering")
    print("   - Model compression and optimization")
    print("   - Kubernetes deployment")
    
    print("\nVIII. CONCLUSION")
    print("   - Always serialize models with preprocessing pipelines")
    print("   - Track model versions and metadata")
    print("   - Implement monitoring for production models")
    print("   - Use containers for reproducibility")
    print("   - Plan for model updates and rollback")
    print("\n   Implementation complete!")


if __name__ == "__main__":
    main()