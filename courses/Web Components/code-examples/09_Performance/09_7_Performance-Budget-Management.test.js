/**
 * @group unit
 * @group performance
 */
import { expect, fixture, html } from '@open-wc/testing';
import './09_7_Performance-Budget-Management.js';

describe('BudgetManager', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<budget-manager></budget-manager>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render budget manager UI', () => {
    const h3 = element.shadowRoot.querySelector('h3');
    expect(h3).to.exist;
    expect(h3.textContent).to.include('Performance Budget Manager');
  });

  it('should render budgets list', () => {
    const budgetsList = element.shadowRoot.getElementById('budgets-list');
    expect(budgetsList).to.exist;
  });

  it('should render violations section', () => {
    const violationsSection = element.shadowRoot.getElementById('violations-section');
    expect(violationsSection).to.exist;
  });

  it('should render control buttons', () => {
    const buttons = element.shadowRoot.querySelectorAll('button');
    expect(buttons.length).to.equal(3);
  });

  describe('Property Changes', () => {
    it('should set budget limits', () => {
      element.setBudget('bundleSize', 200, 150);
      const data = element.getBudgetData();
      const budget = data.budgets.find(b => b.name === 'bundleSize');
      expect(budget.limit).to.equal(200);
      expect(budget.target).to.equal(150);
    });

    it('should return budget data object', () => {
      const data = element.getBudgetData();
      expect(data).to.have.property('budgets');
      expect(data).to.have.property('violations');
      expect(data).to.have.property('history');
    });

    it('should have multiple budgets configured', () => {
      const data = element.getBudgetData();
      expect(data.budgets.length).to.be.greaterThan(5);
    });
  });

  describe('Events', () => {
    it('should setup check budgets button handler', () => {
      const checkBtn = element.shadowRoot.getElementById('check-budgets');
      expect(checkBtn).to.exist;
    });

    it('should setup reset violations button handler', () => {
      const resetBtn = element.shadowRoot.getElementById('reset-violations');
      expect(resetBtn).to.exist;
    });

    it('should setup export budgets button handler', () => {
      const exportBtn = element.shadowRoot.getElementById('export-budgets');
      expect(exportBtn).to.exist;
    });
  });

  describe('Lifecycle', () => {
    it('should initialize budgets on connectedCallback', () => {
      expect(element._budgets.size).to.be.greaterThan(0);
    });

    it('should clear violations on disconnectCallback', () => {
      element.disconnectCallback();
      expect(element._violations).to.be.an('array').that.is.empty;
    });
  });
});

describe('PerformanceBudget', () => {
  let budget;

  beforeEach(() => {
    budget = new PerformanceBudget('testMetric', 100, 'ms', 80);
  });

  it('should store budget properties', () => {
    expect(budget.name).to.equal('testMetric');
    expect(budget.fixedLimit).to.equal(100);
    expect(budget.unit).to.equal('ms');
    expect(budget.targetLimit).to.equal(80);
  });

  it('should check values', () => {
    budget.check(50);
    expect(budget.currentValue).to.equal(50);
  });

  it('should detect over budget', () => {
    budget.check(150);
    expect(budget.isOver()).to.be.true;
  });

  it('should detect near budget', () => {
    budget.check(85);
    expect(budget.isNear()).to.be.true;
    expect(budget.isOver()).to.be.false;
  });

  it('should detect on track', () => {
    budget.check(50);
    expect(budget.isOver()).to.be.false;
    expect(budget.isNear()).to.be.false;
  });

  it('should use default target if not provided', () => {
    const budgetNoTarget = new PerformanceBudget('test', 100, 'ms');
    expect(budgetNoTarget.targetLimit).to.equal(80);
  });
});