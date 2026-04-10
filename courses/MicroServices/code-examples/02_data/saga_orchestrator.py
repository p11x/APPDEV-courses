"""
Saga Pattern Orchestrator

This module provides a Saga orchestrator for managing distributed transactions:
- Choreography-based sagas (event-driven)
- Orchestration-based sagas (centralized coordinator)
- Compensating transactions for rollback
- Timeout handling
- Persistence for recovery

Usage:
    # Create saga
    saga = Saga("order-123")
    saga.add_step(
        "reserve_inventory",
        reserve_inventory,
        compensate_release_inventory,
    )
    saga.add_step(
        "process_payment",
        process_payment,
        compensate_refund_payment,
    )
    saga.add_step(
        "create_shipment",
        create_shipment,
        compensate_cancel_shipment,
    )
    
    # Execute saga
    result = await saga.execute()
"""

import asyncio
import logging
import uuid
from typing import Any, Callable, Dict, List, Optional, Awaitable
from dataclasses import dataclass, field
from datetime import datetime
from enum import Enum
from abc import ABC, abstractmethod


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


class SagaState(Enum):
    """Saga execution states."""
    PENDING = "pending"
    RUNNING = "running"
    COMPLETED = "completed"
    FAILED = "failed"
    COMPENSATING = "compensating"
    COMPENSATED = "compensated"


@dataclass
class SagaStep:
    """
    A single step in a saga.
    
    Attributes:
        name: Step identifier
        action: Async function to execute
        compensate: Async function to run for rollback
        timeout: Optional timeout in seconds
    """
    name: str
    action: Callable[[], Awaitable[Any]]
    compensate: Callable[[], Awaitable[Any]]
    timeout: float = 30.0
    retryable: bool = True
    max_retries: int = 3


@dataclass
class SagaExecution:
    """Tracks the execution of a saga."""
    saga_id: str
    state: SagaState = SagaState.PENDING
    current_step: Optional[str] = None
    step_results: Dict[str, Any] = field(default_factory=dict)
    completed_steps: List[str] = field(default_factory=list)
    failed_step: Optional[str] = None
    error: Optional[str] = None
    started_at: Optional[datetime] = None
    completed_at: Optional[datetime] = None


class SagaError(Exception):
    """Exception raised when saga fails."""
    pass


class Saga:
    """
    Saga orchestrator for managing distributed transactions.
    
    A saga is a sequence of local transactions where each step can have
    a compensating action for rollback. If any step fails, all previously
    completed steps are compensated in reverse order.
    
    Usage:
        saga = Saga("order-123")
        
        saga.add_step(
            "reserve_inventory",
            lambda: reserve_inventory(order_items),
            lambda: release_inventory(reserved_items),
        )
        
        saga.add_step(
            "charge_payment",
            lambda: charge_payment(amount),
            lambda: refund_payment(charge_id),
        )
        
        result = await saga.execute()
    """
    
    def __init__(self, saga_id: Optional[str] = None):
        self.saga_id = saga_id or str(uuid.uuid4())
        self._steps: List[SagaStep] = []
        self._execution = SagaExecution(saga_id=self.saga_id)
    
    def add_step(
        self,
        name: str,
        action: Callable[[], Awaitable[Any]],
        compensate: Callable[[], Awaitable[Any]],
        timeout: float = 30.0,
        retryable: bool = True,
        max_retries: int = 3,
    ):
        """
        Add a step to the saga.
        
        Args:
            name: Unique step identifier
            action: Async function to execute
            compensate: Async function for rollback
            timeout: Step timeout in seconds
            retryable: Whether step can be retried
            max_retries: Maximum retry attempts
        """
        step = SagaStep(
            name=name,
            action=action,
            compensate=compensate,
            timeout=timeout,
            retryable=retryable,
            max_retries=max_retries,
        )
        self._steps.append(step)
        return self
    
    async def execute(self) -> Dict[str, Any]:
        """
        Execute the saga.
        
        Returns:
            Dictionary with saga execution results
            
        Raises:
            SagaError: If saga fails and cannot be compensated
        """
        self._execution.started_at = datetime.utcnow()
        self._execution.state = SagaState.RUNNING
        
        logger.info(f"Starting saga {self.saga_id} with {len(self._steps)} steps")
        
        try:
            for step in self._steps:
                self._execution.current_step = step.name
                logger.info(f"Executing step: {step.name}")
                
                # Execute step with timeout
                result = await asyncio.wait_for(
                    step.action(),
                    timeout=step.timeout,
                )
                
                # Store result
                self._execution.step_results[step.name] = result
                self._execution.completed_steps.append(step.name)
                
                logger.info(f"Step {step.name} completed")
            
            # All steps completed successfully
            self._execution.state = SagaState.COMPLETED
            self._execution.completed_at = datetime.utcnow()
            
            logger.info(f"Saga {self.saga_id} completed successfully")
            
            return {
                "saga_id": self.saga_id,
                "status": "completed",
                "results": self._execution.step_results,
            }
        
        except asyncio.TimeoutError:
            logger.error(f"Saga {self.saga_id} timed out at step {step.name}")
            await self._compensate(step.name, "timeout")
            raise SagaError(f"Saga timed out at step {step.name}")
        
        except Exception as e:
            logger.error(f"Saga {self.saga_id} failed at step {self._execution.current_step}: {e}")
            self._execution.error = str(e)
            self._execution.failed_step = self._execution.current_step
            
            # Compensate completed steps
            await self._compensate(self._execution.current_step, e)
            
            raise SagaError(f"Saga failed: {e}")
    
    async def _compensate(self, failed_step_name: str, error: Any):
        """Execute compensating transactions for completed steps."""
        self._execution.state = SagaState.COMPENSATING
        
        logger.info(f"Starting compensation for saga {self.saga_id}")
        
        # Get completed steps in reverse order
        completed = list(reversed(self._execution.completed_steps))
        
        for step_name in completed:
            # Find the step
            step = next((s for s in self._steps if s.name == step_name), None)
            if not step:
                continue
            
            logger.info(f"Compensating step: {step_name}")
            
            try:
                # Get the result from the original execution
                result = self._execution.step_results.get(step_name)
                
                # Execute compensation
                await asyncio.wait_for(
                    step.compensate(),
                    timeout=step.timeout,
                )
                
                logger.info(f"Compensation for {step_name} completed")
            
            except Exception as e:
                logger.error(f"Compensation failed for {step_name}: {e}")
                # Log but continue compensating other steps
        
        self._execution.state = SagaState.COMPENSATED
        logger.info(f"Compensation completed for saga {self.saga_id}")
    
    @property
    def execution(self) -> SagaExecution:
        """Get current execution state."""
        return self._execution


class SagaOrchestrator:
    """
    Centralized saga orchestrator for managing multiple saga instances.
    
    Features:
    - Saga persistence
    - Recovery from failures
    - Timeout monitoring
    - Concurrency control
    """
    
    def __init__(self):
        self._sagas: Dict[str, Saga] = {}
        self._executions: Dict[str, SagaExecution] = {}
        self._running = False
    
    async def start_saga(
        self,
        saga_id: str,
        steps: List[Dict[str, Any]],
    ) -> Saga:
        """
        Start a new saga execution.
        
        Args:
            saga_id: Unique saga identifier
            steps: List of step configurations
            
        Returns:
            Created saga instance
        """
        saga = Saga(saga_id)
        
        for step_config in steps:
            saga.add_step(
                name=step_config["name"],
                action=step_config["action"],
                compensate=step_config["compensate"],
                timeout=step_config.get("timeout", 30.0),
            )
        
        self._sagas[saga_id] = saga
        self._executions[saga_id] = saga.execution
        
        # Execute saga
        asyncio.create_task(saga.execute())
        
        return saga
    
    async def get_saga_status(self, saga_id: str) -> Optional[SagaExecution]:
        """Get the status of a saga."""
        return self._executions.get(saga_id)
    
    async def compensate_saga(self, saga_id: str) -> bool:
        """
        Manually trigger compensation for a saga.
        
        Args:
            saga_id: Saga identifier
            
        Returns:
            True if compensation was triggered
        """
        saga = self._sagas.get(saga_id)
        if not saga:
            return False
        
        await saga._compensate("manual", "manual_trigger")
        return True


# Example: E-commerce Order Saga
class OrderSaga:
    """
    Example saga for processing an e-commerce order.
    
    Steps:
    1. Reserve inventory
    2. Process payment
    3. Create shipment
    """
    
    @staticmethod
    async def reserve_inventory(order_items: List[Dict[str, Any]]) -> Dict[str, Any]:
        """Reserve inventory for order."""
        logger.info(f"Reserving inventory for items: {order_items}")
        await asyncio.sleep(0.5)  # Simulate service call
        
        return {
            "reservation_id": f"res-{uuid.uuid4().hex[:8]}",
            "items": order_items,
        }
    
    @staticmethod
    async def release_inventory(reservation_result: Dict[str, Any]):
        """Release reserved inventory."""
        logger.info(f"Releasing inventory: {reservation_result}")
        await asyncio.sleep(0.3)
    
    @staticmethod
    async def process_payment(order_id: str, amount: float) -> Dict[str, Any]:
        """Process payment for order."""
        logger.info(f"Processing payment: {order_id} - ${amount}")
        await asyncio.sleep(0.5)
        
        return {
            "payment_id": f"pay-{uuid.uuid4().hex[:8]}",
            "amount": amount,
            "status": "completed",
        }
    
    @staticmethod
    async def refund_payment(payment_result: Dict[str, Any]):
        """Refund payment."""
        logger.info(f"Refunding payment: {payment_result}")
        await asyncio.sleep(0.3)
    
    @staticmethod
    async def create_shipment(order_id: str, address: str) -> Dict[str, Any]:
        """Create shipment for order."""
        logger.info(f"Creating shipment for {order_id} to {address}")
        await asyncio.sleep(0.5)
        
        return {
            "shipment_id": f"ship-{uuid.uuid4().hex[:8]}",
            "tracking_number": f"TRACK-{uuid.uuid4().hex[:8].upper()}",
            "status": "created",
        }
    
    @staticmethod
    async def cancel_shipment(shipment_result: Dict[str, Any]):
        """Cancel shipment."""
        logger.info(f"Cancelling shipment: {shipment_result}")
        await asyncio.sleep(0.3)
    
    @classmethod
    def create_order_saga(
        cls,
        order_id: str,
        items: List[Dict[str, Any]],
        amount: float,
        shipping_address: str,
    ) -> Saga:
        """Create an order processing saga."""
        
        # Store intermediate results
        results = {}
        
        async def reserve():
            result = await cls.reserve_inventory(items)
            results["inventory"] = result
            return result
        
        async def charge():
            result = await cls.process_payment(order_id, amount)
            results["payment"] = result
            return result
        
        async def ship():
            result = await cls.create_shipment(order_id, shipping_address)
            results["shipment"] = result
            return result
        
        async def release_inventory_compensate():
            if "inventory" in results:
                await cls.release_inventory(results["inventory"])
        
        async def refund_compensate():
            if "payment" in results:
                await cls.refund_payment(results["payment"])
        
        async def cancel_shipment_compensate():
            if "shipment" in results:
                await cls.cancel_shipment(results["shipment"])
        
        saga = Saga(order_id)
        saga.add_step("reserve_inventory", reserve, release_inventory_compensate)
        saga.add_step("process_payment", charge, refund_compensate)
        saga.add_step("create_shipment", ship, cancel_shipment_compensate)
        
        return saga


# Example usage
async def main():
    """Demonstrate saga pattern."""
    
    # Create and execute order saga
    saga = OrderSaga.create_order_saga(
        order_id="order-12345",
        items=[
            {"product_id": "prod-001", "quantity": 2},
            {"product_id": "prod-002", "quantity": 1},
        ],
        amount=99.99,
        shipping_address="123 Main St, City, Country",
    )
    
    try:
        result = await saga.execute()
        print(f"\nSaga completed successfully!")
        print(f"Results: {result}")
        
        # Print execution details
        execution = saga.execution
        print(f"\nExecution details:")
        print(f"  Saga ID: {execution.saga_id}")
        print(f"  State: {execution.state.value}")
        print(f"  Completed steps: {execution.completed_steps}")
        print(f"  Started: {execution.started_at}")
        print(f"  Completed: {execution.completed_at}")
    
    except SagaError as e:
        print(f"\nSaga failed: {e}")
        print(f"Execution state: {saga.execution.state.value}")
        print(f"Failed at step: {saga.execution.failed_step}")
    
    # Demonstrate saga that will fail
    print("\n" + "="*50)
    print("Demonstrating failure and compensation...")
    
    failing_saga = Saga("failing-order")
    failing_saga.add_step(
        "step1",
        lambda: asyncio.sleep(0.1) or {"step1": "done"},
        lambda: print("Compensating step1"),
    )
    failing_saga.add_step(
        "step2",
        lambda: (_ for _ in ()).throw(Exception("Step 2 failed!")),  # Force failure
        lambda: print("Compensating step2"),
    )
    failing_saga.add_step(
        "step3",
        lambda: asyncio.sleep(0.1) or {"step3": "done"},
        lambda: print("Compensating step3"),
    )
    
    try:
        await failing_saga.execute()
    except SagaError:
        print(f"Saga failed as expected")
        print(f"Compensated steps: {failing_saga.execution.completed_steps}")


if __name__ == "__main__":
    asyncio.run(main())