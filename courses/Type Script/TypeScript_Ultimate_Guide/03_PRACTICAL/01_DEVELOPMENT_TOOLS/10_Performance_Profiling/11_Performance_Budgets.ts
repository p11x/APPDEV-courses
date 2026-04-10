/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 10_Performance_Profiling Topic: 11_Performance_Budgets Purpose: Performance budget configurations Difficulty: intermediate UseCase: web, frontend Version: TS 5.0+ Compatibility: Node.js 16+ Performance: fast Security: N/A */

declare namespace PerformanceBudgets {
  interface Budget {
    resourceSizes: ResourceBudget[];
    resourceCounts: ResourceCountBudget[];
    timingBudgets: TimingBudget[];
  }

  interface ResourceBudget {
    resourceType: 'script' | 'document' | 'image' | 'font' | 'stylesheet' | 'other';
    budget: number;
    tolerance: number;
  }

  interface ResourceCountBudget {
    resourceType: string;
    budget: number;
    tolerance: number;
  }

  interface TimingBudget {
    metric: 'First Contentful Paint' | 'Largest Contentful Paint' | 'Cumulative Layout Shift' | 'First Input Delay' | 'Interaction to Next Paint' | 'Total Blocking Time';
    budget: number;
    tolerance: number;
  }
}

describe('Performance Budgets', () => {
  it('should define budgets', () => {
    const budget: PerformanceBudgets.Budget = {
      resourceSizes: [{ resourceType: 'script', budget: 170, tolerance: 10 }],
      resourceCounts: [],
      timingBudgets: [{ metric: 'Largest Contentful Paint', budget: 2500, tolerance: 0 }],
    };
    expect(budget).toBeDefined();
  });
});

console.log('\n=== Performance Budgets Complete ===');
console.log('Next: 12_Speed_Index_Types.ts');