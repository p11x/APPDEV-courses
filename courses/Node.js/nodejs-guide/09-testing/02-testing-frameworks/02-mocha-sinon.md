# Mocha, Chai, and Sinon Testing Setup

## What You'll Learn

- Mocha framework configuration
- Chai assertion library patterns
- Sinon mocking, stubbing, and spying
- Combining Mocha + Chai + Sinon

## Mocha Setup

```bash
npm install --save-dev mocha chai sinon
```

```json
// .mocharc.json
{
    "spec": "test/**/*.test.js",
    "recursive": true,
    "timeout": 10000,
    "reporter": "spec",
    "require": ["test/setup.js"],
    "exit": true
}
```

```javascript
// test/setup.js — Global test setup
import chai from 'chai';
import sinonChai from 'sinon-chai';

chai.use(sinonChai);
global.expect = chai.expect;
```

## Chai Assertions

```javascript
import { expect } from 'chai';

describe('Chai Assertions', () => {
    // BDD style: expect
    it('equality assertions', () => {
        expect(1 + 1).to.equal(2);
        expect({ a: 1 }).to.deep.equal({ a: 1 });
        expect('hello').to.be.a('string');
        expect([1, 2, 3]).to.have.lengthOf(3);
    });

    it('inclusion assertions', () => {
        expect([1, 2, 3]).to.include(2);
        expect('hello world').to.include('world');
        expect({ a: 1, b: 2 }).to.have.property('a');
        expect({ a: 1 }).to.have.keys('a');
    });

    it('boolean assertions', () => {
        expect(true).to.be.true;
        expect(false).to.be.false;
        expect(null).to.be.null;
        expect(undefined).to.be.undefined;
        expect('').to.be.empty;
    });

    it('number assertions', () => {
        expect(5).to.be.above(3);
        expect(5).to.be.at.least(5);
        expect(5).to.be.below(10);
        expect(5).to.be.within(1, 10);
    });

    it('promise assertions', async () => {
        await expect(Promise.resolve('ok')).to.eventually.equal('ok');
        await expect(Promise.reject(new Error('fail'))).to.be.rejectedWith('fail');
    });

    it('error assertions', () => {
        expect(() => { throw new Error('oops'); }).to.throw('oops');
        expect(() => { throw new Error('oops'); }).to.throw(Error);
    });
});
```

## Sinon Mocking

```javascript
import sinon from 'sinon';
import { expect } from 'chai';

describe('Sinon Mocking', () => {
    describe('Spies', () => {
        it('should track function calls', () => {
            const callback = sinon.spy();
            [1, 2, 3].forEach(callback);

            expect(callback).to.have.been.calledThrice;
            expect(callback).to.have.been.calledWith(1);
            expect(callback).to.have.been.calledWith(2);
        });
    });

    describe('Stubs', () => {
        it('should replace function implementation', () => {
            const stub = sinon.stub();
            stub.returns(42);
            stub.onFirstCall().returns(1);
            stub.onSecondCall().returns(2);

            expect(stub()).to.equal(1);
            expect(stub()).to.equal(2);
            expect(stub()).to.equal(42);
        });

        it('should stub async functions', async () => {
            const stub = sinon.stub();
            stub.resolves({ id: 1, name: 'Alice' });

            const result = await stub();
            expect(result.name).to.equal('Alice');
        });

        it('should stub rejections', async () => {
            const stub = sinon.stub();
            stub.rejects(new Error('Database error'));

            await expect(stub()).to.be.rejectedWith('Database error');
        });
    });

    describe('Mocks', () => {
        it('should set expectations on calls', () => {
            const service = { save: () => {} };
            const mock = sinon.mock(service);

            mock.expects('save').once().withArgs({ name: 'Alice' });
            service.save({ name: 'Alice' });

            mock.verify();
        });
    });

    describe('Fakes', () => {
        it('should create fake timers', () => {
            const clock = sinon.useFakeTimers();

            let called = false;
            setTimeout(() => { called = true; }, 1000);

            clock.tick(1000);
            expect(called).to.be.true;

            clock.restore();
        });

        it('should fake server responses', () => {
            const server = sinon.fakeServer.create();

            server.respondWith('GET', '/api/users', [
                200,
                { 'Content-Type': 'application/json' },
                JSON.stringify([{ id: 1 }]),
            ]);

            // Make request...
            server.restore();
        });
    });
});
```

## Testing a Service with Mocha + Chai + Sinon

```javascript
import { expect } from 'chai';
import sinon from 'sinon';
import { UserService } from '../../src/services/user.service.js';

describe('UserService (Mocha)', () => {
    let service;
    let mockRepo;
    let mockEmail;

    beforeEach(() => {
        mockRepo = {
            findByEmail: sinon.stub(),
            create: sinon.stub(),
            findById: sinon.stub(),
            update: sinon.stub(),
            delete: sinon.stub(),
        };
        mockEmail = {
            sendWelcome: sinon.stub().resolves(),
        };
        service = new UserService(mockRepo, mockEmail);
    });

    afterEach(() => {
        sinon.restore();
    });

    describe('createUser', () => {
        it('should create user and send welcome email', async () => {
            mockRepo.findByEmail.resolves(null);
            mockRepo.create.resolves({ id: 1, name: 'Alice', email: 'alice@test.com' });

            const result = await service.createUser({
                name: 'Alice',
                email: 'alice@test.com',
            });

            expect(result).to.have.property('id', 1);
            expect(mockRepo.create).to.have.been.calledOnce;
            expect(mockEmail.sendWelcome).to.have.been.calledWith('alice@test.com');
        });

        it('should throw when email exists', async () => {
            mockRepo.findByEmail.resolves({ id: 1 });

            await expect(
                service.createUser({ email: 'taken@test.com' })
            ).to.be.rejectedWith('Email already exists');

            expect(mockRepo.create).to.not.have.been.called;
        });
    });
});
```

## Common Mistakes

- Forgetting to call sinon.restore() in afterEach
- Not verifying mock expectations
- Using stubs when spies would suffice
- Not handling async stubs correctly

## Cross-References

- See [Jest](./01-jest-comprehensive.md) for Jest comparison
- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for patterns
- See [API Testing](../06-api-testing/01-rest-graphql.md) for API testing

## Next Steps

Continue to [Unit Testing: Edge Cases](../03-unit-testing/02-edge-cases-middleware.md).
