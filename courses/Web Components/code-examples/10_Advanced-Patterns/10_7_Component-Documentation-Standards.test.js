/**
 * @group unit
 * @group advanced-patterns
 */
import { expect, fixture, html } from '@open-wc/testing';
import './10_7_Component-Documentation-Standards.js';

describe('DocumentationGenerator', () => {
  let generator;

  beforeEach(() => {
    generator = new DocumentationGenerator();
  });

  it('should register components', () => {
    const component = { tagName: 'my-element' };
    generator.register(component);
    expect(generator.components.has('my-element')).to.be.true;
  });

  it('should extract description', () => {
    const component = { tagName: 'my-element', description: 'A test element' };
    const docs = generator.extractDocs(component);
    expect(docs.description).to.equal('A test element');
  });

  it('should extract properties', () => {
    const component = { tagName: 'my-element' };
    const docs = generator.extractDocs(component);
    expect(docs.properties).to.be.an('array');
  });

  it('should generate markdown', () => {
    generator.register({ tagName: 'test-element', description: 'Test' });
    const md = generator.generateMarkdown('test-element');
    expect(md).to.include('# test-element');
  });

  it('should generate Storybook CSF', () => {
    generator.register({ tagName: 'test-element', description: 'Test' });
    const csf = generator.generateStorybookCSF('test-element');
    expect(csf).to.have.property('title');
    expect(csf).to.have.property('component');
  });

  it('should generate custom elements manifest', () => {
    generator.register({ tagName: 'test-element', description: 'Test' });
    const manifest = generator.generateCustomElementsManifest();
    expect(manifest.modules).to.be.an('array');
  });
});

describe('StoryGenerator', () => {
  let storyGen;

  beforeEach(() => {
    storyGen = new StoryGenerator();
  });

  it('should create stories', () => {
    storyGen.create('my-element', { name: 'default', component: 'my-element' });
    const stories = storyGen.getStories('my-element');
    expect(stories.length).to.equal(1);
  });

  it('should generate story', () => {
    const story = storyGen.generateStory({
      name: 'Button',
      component: 'my-button',
      args: { variant: 'primary' },
    });
    expect(story.title).to.equal('Button');
    expect(story.component).to.equal('my-button');
  });

  it('should generate variants', () => {
    storyGen.generateVariant('my-element', 'primary', { variant: 'primary' });
    const variants = storyGen.getVariants('my-element');
    expect(variants.length).to.equal(1);
  });

  it('should generate all stories', () => {
    storyGen.create('element1', { name: 'story1' });
    storyGen.create('element2', { name: 'story2' });
    const all = storyGen.generateAllStories();
    expect(all.length).to.equal(2);
  });
});

describe('APIDocGenerator', () => {
  let apiDoc;

  beforeEach(() => {
    apiDoc = new APIDocGenerator();
  });

  it('should add sections', () => {
    apiDoc.addSection('Overview', 'This is the overview');
    const output = apiDoc.generate();
    expect(output).to.include('## Overview');
  });

  it('should generate from class', () => {
    const componentClass = { tagName: 'my-element', description: 'A component' };
    const docs = apiDoc.generateFromClass(componentClass);
    expect(docs).to.include('Overview');
    expect(docs).to.include('Usage');
  });
});

describe('PlaygroundManager', () => {
  let playground;

  beforeEach(() => {
    playground = new PlaygroundManager();
  });

  it('should add config', () => {
    playground.addConfig('my-element', { component: 'my-element' });
    const config = playground.getConfig('my-element');
    expect(config.component).to.equal('my-element');
  });

  it('should generate playground URL', () => {
    playground.addConfig('my-element', { component: 'my-element' });
    const url = playground.generatePlaygroundUrl('my-element');
    expect(url).to.include('my-element');
  });

  it('should generate CodeSandbox', () => {
    playground.addConfig('my-element', { component: 'my-element' });
    const sandbox = playground.generateCodeSandbox('my-element');
    expect(sandbox.files).to.have.property('index.html');
    expect(sandbox.files).to.have.property('package.json');
  });

  it('should generate HTML', () => {
    const html = playground.generateHTML({ component: 'my-element' });
    expect(html).to.include('<my-element');
  });

  it('should generate package.json', () => {
    const pkg = playground.generatePackage({ component: 'my-element' });
    const parsed = JSON.parse(pkg);
    expect(parsed.name).to.equal('my-element');
  });
});

describe('LiveDemoGenerator', () => {
  let demo;

  beforeEach(() => {
    demo = new LiveDemoGenerator();
  });

  it('should create demos', () => {
    demo.createDemo('my-element', { attributes: { variant: 'primary' } });
    expect(demo.demos.has('my-element')).to.be.true;
  });

  it('should generate HTML', () => {
    demo.createDemo('my-element', { attributes: { variant: 'primary' } });
    const html = demo.generateHTML('my-element');
    expect(html).to.include('variant="primary"');
  });

  it('should generate iframe', () => {
    demo.createDemo('my-element', {});
    const iframe = demo.generateIframe('my-element', { width: '500px' });
    expect(iframe).to.include('<iframe');
    expect(iframe).to.include('width="500px"');
  });
});

describe('Global Exports', () => {
  it('should export docsGenerator', () => {
    expect(docsGenerator).to.exist;
  });

  it('should export storyGenerator', () => {
    expect(storyGenerator).to.exist;
  });

  it('should export apiDocGenerator', () => {
    expect(apiDocGenerator).to.exist;
  });

  it('should export playgroundManager', () => {
    expect(playgroundManager).to.exist;
  });

  it('should export liveDemoGenerator', () => {
    expect(liveDemoGenerator).to.exist;
  });
});