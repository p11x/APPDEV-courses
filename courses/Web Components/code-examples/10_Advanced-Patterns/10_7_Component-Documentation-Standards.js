/**
 * Component Documentation Standards - Storybook integration and documentation
 * patterns for Web Components with auto-docs, stories, and API references
 * @module advanced-patterns/10_7_Component-Documentation-Standards
 * @version 1.0.0
 * @example <docs-component></docs-component>
 */

class DocumentationGenerator {
  constructor() {
    this.components = new Map();
    this.templates = new Map();
  }

  register(component) {
    const docs = this.extractDocs(component);
    this.components.set(component.tagName, docs);
  }

  extractDocs(component) {
    const tagName = component.tagName;
    const docs = {
      tagName,
      description: this.extractDescription(component),
      properties: this.extractProperties(component),
      events: this.extractEvents(component),
      slots: this.extractSlots(component),
      cssProperties: this.extractCSSProps(component),
      examples: [],
    };

    return docs;
  }

  extractDescription(component) {
    return component.description || `Web Component: ${component.tagName}`;
  }

  extractProperties(component) {
    const props = [];
    const proto = component.prototype || Object.getPrototypeOf(component);

    const observed = proto.observedAttributes || [];

    for (const attr of observed) {
      props.push({
        name: attr,
        attribute: attr,
        type: 'string',
        default: '',
        required: false,
      });
    }

    return props;
  }

  extractEvents(component) {
    const events = [];
    const proto = component.prototype || Object.getPrototypeOf(component);

    return events;
  }

  extractSlots(component) {
    const slots = [
      { name: 'default', description: 'Default content' },
    ];

    return slots;
  }

  extractCSSProps(component) {
    const css = [];
    const style = component_styles?.get(component.tagName);

    if (style?.cssProps) {
      for (const [name, value] of Object.entries(style.cssProps)) {
        css.push({ name, default: value });
      }
    }

    return css;
  }

  generateMarkdown(componentName) {
    const docs = this.components.get(componentName);
    if (!docs) return '';

    const lines = [];

    lines.push(`# ${docs.tagName}`);
    lines.push('');
    lines.push(docs.description);
    lines.push('');

    if (docs.properties?.length) {
      lines.push('## Properties');
      lines.push('');
      lines.push('| Property | Attribute | Type | Default | Required |');
      lines.push('|----------|-----------|------|---------|----------|');
      for (const prop of docs.properties) {
        lines.push(`| ${prop.name} | ${prop.attribute} | ${prop.type} | ${prop.default} | ${prop.required} |`);
      }
      lines.push('');
    }

    if (docs.events?.length) {
      lines.push('## Events');
      lines.push('');
      lines.push('| Event | Detail | Bubbles |');
      lines.push('|-------|--------|--------|');
      for (const event of docs.events) {
        lines.push(`| ${event.name} | ${event.detail} | ${event.bubbles} |`);
      }
      lines.push('');
    }

    if (docs.slots?.length) {
      lines.push('## Slots');
      lines.push('');
      lines.push('| Name | Description |');
      lines.push('|------|-------------|');
      for (const slot of docs.slots) {
        lines.push(`| ${slot.name} | ${slot.description} |`);
      }
      lines.push('');
    }

    return lines.join('\n');
  }

  generateStorybookCSF(componentName) {
    const docs = this.components.get(componentName);
    if (!docs) return null;

    return {
      title: docs.tagName,
      component: docs.tagName,
      argTypes: this.generateArgTypes(docs),
      args: this.generateArgs(docs),
    };
  }

  generateArgTypes(docs) {
    const argTypes = {};

    for (const prop of docs.properties || []) {
      argTypes[prop.name] = {
        control: { type: prop.type === 'boolean' ? 'boolean' : 'text' },
        description: prop.description,
      };
    }

    return argTypes;
  }

  generateArgs(docs) {
    const args = {};

    for (const prop of docs.properties || []) {
      if (prop.default) {
        args[prop.name] = prop.default;
      }
    }

    return args;
  }

  generateCustomElementsManifest() {
    const modules = [];

    for (const [tagName, docs] of this.components) {
      modules.push({
        kind: 'web-component',
        name: tagName,
        description: docs.description,
        properties: docs.properties,
        events: docs.events,
        slots: docs.slots,
        cssProperties: docs.cssProperties,
      });
    }

    return { modules };
  }
}

class StoryGenerator {
  constructor() {
    this.stories = new Map();
    this.variants = new Map();
  }

  create(componentName, storyConfig) {
    const stories = this.stories.get(componentName) || [];
    stories.push(storyConfig);
    this.stories.set(componentName, stories);
  }

  generateStory(storyConfig) {
    const {
      name,
      component,
      args = {},
      argTypes = {},
      parameters = {},
    } = storyConfig;

    return {
      title: name,
      component,
      argTypes: argTypes || {},
      args,
      parameters: {
        ...parameters,
        backgrounds: { default: 'light' },
      },
    };
  }

  generateVariant(componentName, name, props = {}) {
    const variants = this.variants.get(componentName) || [];
    variants.push({ name, props });
    this.variants.set(componentName, variants);
  }

  getStories(componentName) {
    return this.stories.get(componentName) || [];
  }

  getVariants(componentName) {
    return this.variants.get(componentName) || [];
  }

  generateAllStories() {
    const allStories = [];

    for (const [componentName, stories] of this.stories) {
      for (const story of stories) {
        allStories.push(this.generateStory({
          ...story,
          name: `${componentName}/${story.name}`,
        }));
      }
    }

    return allStories;
  }
}

class APIDocGenerator {
  constructor() {
    this.sections = new Map();
  }

  addSection(title, content) {
    this.sections.set(title, content);
  }

  generate() {
    const sections = [];

    for (const [title, content] of this.sections) {
      sections.push(`## ${title}\n\n${content}`);
    }

    return sections.join('\n\n');
  }

  generateFromClass(componentClass) {
    this.sections.clear();

    this.addSection('Overview', this.getOverview(componentClass));
    this.addSection('Usage', this.getUsageExample(componentClass));
    this.addSection('API', this.getAPI(componentClass));
    this.addSection('Examples', this.getExamples(componentClass));

    return this.generate();
  }

  getOverview(componentClass) {
    return componentClass.description || `Component: ${componentClass.tagName}`;
  }

  getUsageExample(componentClass) {
    return `\`\`\`html
<${componentClass.tagName}></${componentClass.tagName}>
\`\`\``;
  }

  getAPI(componentClass) {
    const props = componentClass.observedAttributes || [];
    let api = '### Properties\n\n';

    for (const attr of props) {
      api += `- \`${attr}\`: attribute\n`;
    }

    return api;
  }

  getExamples(componentClass) {
    return 'See Storybook stories for working examples.';
  }
}

class PlaygroundManager {
  constructor() {
    this.configs = new Map();
  }

  addConfig(name, config) {
    this.configs.set(name, config);
  }

  getConfig(name) {
    return this.configs.get(name);
  }

  generatePlaygroundUrl(name, baseUrl = 'https://webcomponents.dev') {
    const config = this.configs.get(name);
    if (!config) return null;

    return `${baseUrl}/playground?component=${config.component}`;
  }

  generateCodeSandbox(config) {
    const files = {
      'index.html': this.generateHTML(config),
      'src/index.js': this.generateJS(config),
      'package.json': this.generatePackage(config),
    };

    return {
      title: config.component,
      files,
    };
  }

  generateHTML(config) {
    return `<!DOCTYPE html>
<html>
<head>
  <script type="module" src="./src/index.js"></script>
</head>
<body>
  <${config.component}></${config.component}>
</body>
</html>`;
  }

  generateJS(config) {
    if (config.import) {
      return `import '${config.import}';`;
    }
    return '';
  }

  generatePackage(config) {
    return JSON.stringify({
      name: config.component,
      version: '1.0.0',
      scripts: {
        start: 'vite',
      },
      dependencies: config.dependencies || {},
    }, null, 2);
  }
}

class LiveDemoGenerator {
  constructor() {
    this.demos = new Map();
  }

  createDemo(componentName, config) {
    this.demos.set(componentName, config);
  }

  generateHTML(componentName) {
    const demo = this.demos.get(componentName);
    if (!demo) return `<${componentName}></${componentName}>`;

    const attrs = Object.entries(demo.attributes || {})
      .map(([k, v]) => `${k}="${v}"`)
      .join(' ');

    return `<${componentName} ${attrs}></${componentName}>`;
  }

  generateIframe(componentName, options = {}) {
    const html = this.generateHTML(componentName);
    const src = `data:text/html;charset=utf-8,${encodeURIComponent(html)}`;

    return `<iframe
  src="${src}"
  width="${options.width || '100%'}"
  height="${options.height || '300px'}"
  frameborder="0"
></iframe>`;
  }
}

const docsGenerator = new DocumentationGenerator();
const storyGenerator = new StoryGenerator();
const apiDocGenerator = new APIDocGenerator();
const playgroundManager = new PlaygroundManager();
const liveDemoGenerator = new LiveDemoGenerator();

export { DocumentationGenerator, StoryGenerator, APIDocGenerator, PlaygroundManager, LiveDemoGenerator };
export { docsGenerator, storyGenerator, apiDocGenerator, playgroundManager, liveDemoGenerator };

export default docsGenerator;