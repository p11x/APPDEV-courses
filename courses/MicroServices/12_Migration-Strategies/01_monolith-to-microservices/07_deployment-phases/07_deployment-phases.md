# Deployment Phases for Migration

## Overview

A well-planned deployment strategy is critical for successful microservices migration. Each deployment phase must be carefully orchestrated to minimize risk, maintain business continuity, and enable quick rollback if issues arise. The deployment approach typically follows an incremental pattern, starting with low-risk deployments and progressively moving to more complex scenarios.

Deployment phases should align with the migration strategy and testing approach. Each phase typically involves deploying new services, shifting traffic from the monolith, validating functionality, and decommissioning corresponding monolith components. The goal is to achieve a smooth transition where users experience no disruption.

## Phase Planning

### Phase 1: Infrastructure Setup

This initial phase focuses on preparing the infrastructure for microservices. Activities include provisioning Kubernetes clusters, setting up service mesh, configuring monitoring and logging, establishing CI/CD pipelines, and implementing security controls.

Infrastructure should be in place before any service deployment begins. This ensures that new services can be deployed reliably and operated effectively.

### Phase 2: Pilot Service Deployment

The first microservice to be deployed is typically a pilot service with low risk and clear boundaries. This phase validates the entire deployment pipeline and operational procedures before proceeding to more complex services.

The pilot service should be something that provides immediate value but doesn't impact critical business functions. This allows the team to learn and refine processes without significant risk.

### Phase 3: Incremental Service Deployment

After the pilot, additional services are deployed incrementally. Each deployment follows a pattern: deploy the service, run in parallel with monolith, gradually shift traffic, validate functionality, and decommission monolith code.

The order of service deployment should be based on dependency analysis, business priority, and risk assessment.

### Phase 4: Full Migration

This phase involves completing migration of all planned services, retiring the monolith, and establishing the new system as the primary platform.

## Implementation Example

```python
#!/usr/bin/env python3
"""
Migration Deployment Manager
Manages deployment phases for microservices migration
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional
from enum import Enum
from datetime import datetime
import json


class PhaseType(Enum):
    INFRASTRUCTURE = "infrastructure"
    PILOT = "pilot"
    INCREMENTAL = "incremental"
    FULL_MIGRATION = "full_migration"


class PhaseStatus(Enum):
    PLANNED = "planned"
    IN_PROGRESS = "in_progress"
    COMPLETED = "completed"
    FAILED = "failed"
    ROLLED_BACK = "rolled_back"


@dataclass
class DeploymentPhase:
    """Represents a deployment phase"""
    phase_id: str
    name: str
    phase_type: PhaseType
    description: str
    services: List[str] = field(default_factory=list)
    status: PhaseStatus = PhaseStatus.PLANNED
    started_at: Optional[datetime] = None
    completed_at: Optional[datetime] = None
    success_criteria: List[str] = field(default_factory=list)


class DeploymentManager:
    """Manages deployment phases for migration"""
    
    def __init__(self):
        self.phases: Dict[str, DeploymentPhase] = {}
        self.current_phase: Optional[str] = None
    
    def create_phase(
        self,
        phase_id: str,
        name: str,
        phase_type: PhaseType,
        description: str,
        services: List[str] = None,
        success_criteria: List[str] = None
    ) -> DeploymentPhase:
        """Create a new deployment phase"""
        
        phase = DeploymentPhase(
            phase_id=phase_id,
            name=name,
            phase_type=phase_type,
            description=description,
            services=services or [],
            success_criteria=success_criteria or []
        )
        
        self.phases[phase_id] = phase
        return phase
    
    def start_phase(self, phase_id: str):
        """Start a deployment phase"""
        
        phase = self.phases.get(phase_id)
        if not phase:
            raise ValueError(f"Phase {phase_id} not found")
        
        phase.status = PhaseStatus.IN_PROGRESS
        phase.started_at = datetime.now()
        self.current_phase = phase_id
        
        print(f"Starting phase: {phase.name}")
    
    def complete_phase(self, phase_id: str):
        """Mark a phase as completed"""
        
        phase = self.phases.get(phase_id)
        if not phase:
            raise ValueError(f"Phase {phase_id} not found")
        
        phase.status = PhaseStatus.COMPLETED
        phase.completed_at = datetime.now()
        self.current_phase = None
        
        print(f"Completed phase: {phase.name}")
    
    def get_migration_status(self) -> Dict:
        """Get overall migration status"""
        
        total_phases = len(self.phases)
        completed = sum(
            1 for p in self.phases.values()
            if p.status == PhaseStatus.COMPLETED
        )
        
        return {
            "total_phases": total_phases,
            "completed": completed,
            "current_phase": self.current_phase,
            "progress_percentage": (
                completed / total_phases * 100
                if total_phases > 0 else 0
            )
        }


# Example usage
if __name__ == "__main__":
    manager = DeploymentManager()
    
    # Define phases
    manager.create_phase(
        phase_id="phase_1",
        name="Infrastructure Setup",
        phase_type=PhaseType.INFRASTRUCTURE,
        description="Provision and configure infrastructure",
        services=["kubernetes", "monitoring", "ci-cd"]
    )
    
    manager.create_phase(
        phase_id="phase_2", 
        name="Pilot Service",
        phase_type=PhaseType.PILOT,
        description="Deploy first microservice",
        services=["user-service"]
    )
    
    manager.create_phase(
        phase_id="phase_3",
        name="Incremental Deployment",
        phase_type=PhaseType.INCREMENTAL,
        description="Deploy remaining services",
        services=["order-service", "catalog-service", "payment-service"]
    )
    
    manager.create_phase(
        phase_id="phase_4",
        name="Full Migration",
        phase_type=PhaseType.FULL_MIGRATION,
        description="Complete migration and retire monolith",
        services=["all"]
    )
    
    # Execute phases
    for phase_id in ["phase_1", "phase_2", "phase_3", "phase_4"]:
        manager.start_phase(phase_id)
        # Simulate work
        manager.complete_phase(phase_id)
    
    # Get status
    status = manager.get_migration_status()
    print(f"\nMigration Status: {json.dumps(status, indent=2)}")
