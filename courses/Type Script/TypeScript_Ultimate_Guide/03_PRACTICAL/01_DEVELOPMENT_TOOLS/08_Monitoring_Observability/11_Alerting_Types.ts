/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 11_Alerting_Types Purpose: Alert configuration type definitions Difficulty: intermediate UseCase: enterprise, devops Version: TS 5.0+ Compatibility: Node.js 16+, Alertmanagers Performance: fast Security: N/A */

declare namespace AlertingTypes {
  interface Alert {
    labels: Labels;
    annotations: Annotations;
    startsAt: Date;
    endsAt?: Date;
    generatorURL?: string;
    fingerprint?: string;
  }

  interface Labels {
    [key: string]: string;
  }

  interface Annotations {
    [key: string]: string;
  }

  interface AlertManager {
    send(alerts: Alert[]): Promise<void>;
    formatAlert(alert: Alert): string;
  }

  interface AlertRule {
    alert: string;
    expr: string;
    for: string;
    labels?: Labels;
    annotations?: Annotations;
  }

  interface AlertConfiguration {
    groups: AlertGroup[];
  }

  interface AlertGroup {
    name: string;
    interval?: string;
    rules: AlertRule[];
  }

  interface PagerDutyConfig {
    integrationKey: string;
    pagerdutyUrl?: string;
    clientName?: string;
    clientUrl?: string;
  }

  interface SlackConfig {
    webhookUrl: string;
    channel?: string;
    username?: string;
    iconEmoji?: string;
    title?: string;
    text?: string;
  }

  interface WebhookConfig {
    url: string;
    method?: string;
    headers?: Record<string, string>;
  }

  interface SMTPConfig {
    host: string;
    port: number;
    username?: string;
    password?: string;
    from: string;
    to: string[];
    subject?: string;
  }
}

import { AlertManager } from 'prometheus-alertmanager';

const amClient = new AlertManager({
  endpoint: 'http://localhost:9093',
});

describe('Alerting Types', () => {
  describe('Alert Creation', () => {
    it('should create alert', () => {
      const alert: AlertingTypes.Alert = {
        labels: { alertname: 'HighCPUUsage', severity: 'critical' },
        annotations: { description: 'CPU usage above 90%' },
        startsAt: new Date(),
      };
      expect(alert).toBeDefined();
    });
  });
});

console.log('\n=== Alerting Types Complete ===');
console.log('Next: 10_Performance_Profiling/02_Memory_Profiling.ts');